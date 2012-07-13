﻿using System;
using Pici.Storage.Database.Session_Details;
using Pici.Storage.Database.Session_Details.Interfaces;
using MySql.Data.MySqlClient;

namespace Pici.Storage.Database
{
    public class MySqlClient : IDatabaseClient
    {
        private MySqlConnection connection;
        private DatabaseManager dbManager;
        private IQueryAdapter info;

        public MySqlClient(DatabaseManager dbManager, int id)
        {
            this.dbManager = dbManager;
            this.connection = new MySqlConnection(dbManager.getConnectionString());
        }

        public void connect()
        {
            this.connection.Open();
        }

        public void disconnect()
        {
            try
            {
                this.connection.Close();
            }
            catch 
            { }
        }

        public void Dispose()
        {
            this.info = null;
            disconnect();
            dbManager.FreeConnection(this);
        }

        internal MySqlCommand getNewCommand()
        {
            return this.connection.CreateCommand();
        }

        public IQueryAdapter getQueryReactor()
        {
            return this.info;
        }

        internal MySqlTransaction getTransaction()
        {
            return this.connection.BeginTransaction();
        }

        public bool isAvailable()
        {
            return (this.info == null);
        }

        public void prepare()
        {
            this.info = new NormalQueryReactor(this);
        }

        public void reportDone()
        {
            this.Dispose();
        }
    }
}

