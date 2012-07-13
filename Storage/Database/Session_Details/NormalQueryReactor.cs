﻿namespace Pici.Storage.Database.Session_Details
{
    using System;
    using Pici.Storage.Database.Database_Exceptions;
    using Pici.Storage.Database.Session_Details.Interfaces;
    using Pici.Storage.Session_Details.Interfaces;

    internal class NormalQueryReactor : QueryAdapter, IQueryAdapter, IRegularQueryAdapter, IDisposable
    {
        internal NormalQueryReactor(MySqlClient client) : base(client)
        {
            base.command = client.getNewCommand();
        }

        public DatabaseType dbType
        {
            get
            {
                return DatabaseType.MySQL;
            }
        }

        public void Dispose()
        {
            base.command.Dispose();
            base.client.reportDone();
        }

        public void doCommit()
        {
            new TransactionException("Can't use rollback on a non-transactional Query reactor");
        }

        public void doRollBack()
        {
            new TransactionException("Can't use rollback on a non-transactional Query reactor");
        }

        internal bool getAutoCommit()
        {
            return true;
        }
    }
}

