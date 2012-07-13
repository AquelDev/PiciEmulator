using System;
using MySql.Data.MySqlClient;

namespace Butterfly.Storage
{
    /// <summary>
    /// Represents a storage database.
    /// </summary>
    internal struct DatabasePropertyOld
    {
        #region Fields
        private readonly string hostname;
        private readonly string databaseName;

        private readonly uint port;
        private readonly string username;
        private readonly string password;

        internal readonly uint minPoolSize;
        internal readonly uint maxPoolSize;

        #endregion

        #region Constructor
        internal DatabasePropertyOld(string hostname, string databaseName, uint port, string username, string password, uint minPoolSize, uint maxPoolSize)
        {
            this.hostname = hostname;
            this.databaseName = databaseName;
            this.port = port;
            this.username = username;
            this.password = password;
            this.minPoolSize = minPoolSize;
            this.maxPoolSize = maxPoolSize;
        }
        #endregion

        internal string GenerateConnectionString()
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder();

            connectionStringBuilder.Server = hostname;
            connectionStringBuilder.Port = port;
            connectionStringBuilder.UserID = username;
            connectionStringBuilder.Password = password;

            connectionStringBuilder.Database = databaseName;
            connectionStringBuilder.MinimumPoolSize = minPoolSize;
            connectionStringBuilder.MaximumPoolSize = maxPoolSize;

            return connectionStringBuilder.ToString();
        }
    }
}
