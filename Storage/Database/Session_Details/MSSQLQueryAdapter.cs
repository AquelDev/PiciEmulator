﻿using System;
using System.Data;
using System.Data.SqlClient;
using Pici.Storage.Session_Details.Interfaces;
using Pici.Core.Loggings;
namespace Pici.Storage.Database.Session_Details
{
    class MSSQLQueryAdapter : IRegularQueryAdapter
    {
        private static bool dbEnabled
        {
            get
            {
                return DatabaseManager.dbEnabled;
            }
        }

        protected MsSQLClient client;
        protected SqlCommand command;
        public DatabaseType dbType
        {
            get
            {
                return DatabaseType.MSSQL;
            }
        }

        internal MSSQLQueryAdapter(MsSQLClient client)
        {
            this.client = client;
        }

        public void addParameter(string name, byte[] data)
        {
            this.command.Parameters.Add(new SqlParameter(name, SqlDbType.Binary, data.Length));
        }

        public void addParameter(string parameterName, object val)
        {
            this.command.Parameters.AddWithValue(parameterName, val);
        }

        public bool findsResult()
        {
            if (!dbEnabled)
                return false;
            DateTime now = DateTime.Now;
            bool hasRows = false;
            try
            {
                using (SqlDataReader reader = this.command.ExecuteReader())
                {
                    hasRows = reader.HasRows;
                }
            }
            catch (Exception exception)
            {
                Writer.LogQueryError(exception, this.command.CommandText);
            }
            TimeSpan span = (TimeSpan)(DateTime.Now - now);
            DatabaseStats.totalQueryTime += span.Milliseconds;
            DatabaseStats.totalQueries++;
            return hasRows;
        }

        public int getInteger()
        {
            if (!dbEnabled)
                return 0;
            DateTime now = DateTime.Now;
            int result = 0;
            try
            {
                object obj2 = this.command.ExecuteScalar();
                if (obj2 != null)
                {
                    int.TryParse(obj2.ToString(), out result);
                }
            }
            catch (Exception exception)
            {
                Writer.LogQueryError(exception, this.command.CommandText);
            }
            TimeSpan span = (TimeSpan)(DateTime.Now - now);
            DatabaseStats.totalQueryTime += span.Milliseconds;
            DatabaseStats.totalQueries++;
            return result;
        }

        public DataRow getRow()
        {
            if (!dbEnabled)
                return null;
            DateTime now = DateTime.Now;
            DataRow Row = null;
            try
            {
                DataSet dataSet = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter(this.command))
                {
                    adapter.Fill(dataSet);
                }
                if ((dataSet.Tables.Count > 0) && (dataSet.Tables[0].Rows.Count == 1))
                {
                    Row = dataSet.Tables[0].Rows[0];
                }
            }
            catch (Exception exception)
            {
                Writer.LogQueryError(exception, this.command.CommandText);
            }
            TimeSpan span = (TimeSpan)(DateTime.Now - now);
            DatabaseStats.totalQueryTime += span.Milliseconds;
            DatabaseStats.totalQueries++;
            return Row;
        }

        public string getString()
        {
            if (!dbEnabled)
                return string.Empty;
            DateTime now = DateTime.Now;
            string str = string.Empty;
            try
            {
                object obj2 = this.command.ExecuteScalar();
                if (obj2 != null)
                {
                    str = obj2.ToString();
                }
            }
            catch (Exception exception)
            {
                Writer.LogQueryError(exception, this.command.CommandText);
            }
            TimeSpan span = (TimeSpan)(DateTime.Now - now);
            DatabaseStats.totalQueryTime += span.Milliseconds;
            DatabaseStats.totalQueries++;
            return str;
        }

        public DataTable getTable()
        {
            DateTime now = DateTime.Now;
            DataTable dataTable = new DataTable();
            if (!dbEnabled)
                return dataTable;
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(this.command))
                {
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception exception)
            {
                Writer.LogQueryError(exception, this.command.CommandText);
            }
            TimeSpan span = (TimeSpan)(DateTime.Now - now);
            DatabaseStats.totalQueryTime += span.Milliseconds;
            DatabaseStats.totalQueries++;
            return dataTable;
        }

        public long insertQuery()
        {
            if (!dbEnabled)
                return 0;
            DateTime now = DateTime.Now;
            long lastInsertedId = 0L;
            try
            {
                lastInsertedId = (long)command.ExecuteScalar();
            }
            catch (Exception exception)
            {
                Writer.LogQueryError(exception, this.command.CommandText);
            }
            TimeSpan span = (TimeSpan)(DateTime.Now - now);
            DatabaseStats.totalQueryTime += span.Milliseconds;
            DatabaseStats.totalQueries++;
            return lastInsertedId;
        }

        public void runFastQuery(string query)
        {
            if (!dbEnabled)
                return;
            DateTime now = DateTime.Now;
            this.setQuery(query);
            this.runQuery();
            TimeSpan span = (TimeSpan)(DateTime.Now - now);
            DatabaseStats.totalQueryTime += span.Milliseconds;
            DatabaseStats.totalQueries++;
        }

        public void runQuery()
        {
            if (!dbEnabled)
                return;
            DateTime now = DateTime.Now;
            try
            {
                this.command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                Writer.LogQueryError(exception, this.command.CommandText);
            }
            TimeSpan span = (TimeSpan)(DateTime.Now - now);
            DatabaseStats.totalQueryTime += span.Milliseconds;
            DatabaseStats.totalQueries++;
        }

        public void setQuery(string query)
        {
            this.command.Parameters.Clear();
            this.command.CommandText = query;
        }
    }
}
