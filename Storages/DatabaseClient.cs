using System;
using System.Data;
using Butterfly.Core;
using MySql.Data.MySqlClient;

namespace Butterfly.Storage
{
    class DatabaseClientOld : IDisposable
    {
        #region Fields
        private readonly uint handleID;
        private int lastActivityTimestamp;

        private readonly DatabaseManagerOld manager;
        private readonly MySqlConnection connection;
        private MySqlCommand sqlCommand;

        internal bool isWorking;
        #endregion

        #region Properties
        internal uint Handle
        {
            get { return handleID; }
        }

        internal bool Anonymous
        {
            get { return (handleID == 0); }
        }

        internal int ActivityStamp
        {
            get { return lastActivityTimestamp; }
        }
        
        internal ConnectionState State
        {
            get { return (connection != null) ? connection.State : ConnectionState.Broken; }

        }
        #endregion

        #region Constructor
        internal DatabaseClientOld(uint Handle, string connectionString, DatabaseManagerOld pManager)
        {
            handleID = Handle;
            manager = pManager;

            connection = new MySqlConnection(connectionString);
            sqlCommand = connection.CreateCommand();
            lastActivityTimestamp = ButterflyEnvironment.GetUnixTimestamp();
        }
        #endregion

        #region Methods
        internal void Connect()
        {
            try
            {
                connection.Open();
            }
            catch (MySqlException mex)
            {
                throw new DatabaseExceptionOld("Failed to open connection for database client " + handleID + ", exception message: " + mex.Message);
            }
        }

        internal void Disconnect()
        {
            try
            {
                connection.Close();
            }
            catch { }
        }
        
        internal void Destroy()
        {
            Disconnect();
            connection.Dispose();
            sqlCommand.Dispose();
        }

        internal void UpdateLastActivity()
        {
            isWorking = true;
            lastActivityTimestamp = ButterflyEnvironment.GetUnixTimestamp();
        }

        #region Database methods
        internal void AddParamWithValue(string sParam, object val)
        {
            sqlCommand.Parameters.AddWithValue(sParam, val);
        }

        internal void ExecuteQuery(string sQuery)
        {
            try
            {
                sqlCommand.CommandText = sQuery;
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Logging.LogQueryError(ex, sQuery);
                throw ex;
            }
        }

        internal DataTable ReadDataTable(string sQuery)
        {
            DataTable pDataTable = new DataTable();
            try
            {
                sqlCommand.CommandText = sQuery;

                using (MySqlDataAdapter pAdapter = new MySqlDataAdapter(sqlCommand))
                {
                    pAdapter.Fill(pDataTable);
                }
            }
            catch (Exception ex)
            {
                Logging.LogQueryError(ex, sQuery);
                throw ex;
            }

            return pDataTable;
        }

        internal DataRow ReadDataRow(string sQuery)
        {
            try
            {
                DataTable pDataTable = ReadDataTable(sQuery);
                if (pDataTable != null && pDataTable.Rows.Count > 0)
                    return pDataTable.Rows[0];
            }
            catch (Exception ex)
            {
                Logging.LogQueryError(ex, sQuery);
                throw ex;
            }
            return null;
        }

        internal String ReadString(string sQuery)
        {
            sqlCommand.CommandText = sQuery;
            String result = sqlCommand.ExecuteScalar().ToString();

            return result;
        }

        internal Int32 ReadInt32(string sQuery)
        {
            sqlCommand.CommandText = sQuery;
            Int32 result = (Int32)sqlCommand.ExecuteScalar();

            return result;
        }

        internal UInt32 ReadUInt32(string sQuery)
        {
            sqlCommand.CommandText = sQuery;
            UInt32 result = (UInt32)sqlCommand.ExecuteScalar();

            return result;
        }
        #endregion

        #region IDisposable members
        /// <summary>
        /// Returns the DatabaseClient to the DatabaseManager, where the connection will stay alive for 30 seconds of inactivity.
        /// </summary>
        public void Dispose()
        {
            if (!Anonymous)
                sqlCommand.Parameters.Clear();
            else
                Destroy();

            isWorking = false;
        }
        #endregion
        #endregion
    }
}
