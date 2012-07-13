﻿namespace Pici.Storage.Database.Session_Details
{
    using MySql.Data.MySqlClient;
    using System;
    using Pici.Storage.Database.Session_Details.Interfaces;
    using Pici.Storage.Session_Details.Interfaces;
    using Pici.Storage.Database.Database_Exceptions;

    internal class TransactionQueryReactor : QueryAdapter, IQueryAdapter, IRegularQueryAdapter, IDisposable
    {
        private bool finishedTransaction;
        private MySqlTransaction transaction;

        internal TransactionQueryReactor(MySqlClient client) : base(client)
        {
            this.initTransaction();
        }

        public void Dispose()
        {
            if (!this.finishedTransaction)
            {
                throw new TransactionException("The transaction needs to be completed by commit() or rollback() before you can dispose this item.");
            }
            base.command.Dispose();
            base.client.reportDone();
        }

        public void doCommit()
        {
            try
            {
                this.transaction.Commit();
                this.finishedTransaction = true;
            }
            catch (MySqlException exception)
            {
                throw new TransactionException(exception.Message);
            }
        }

        public void doRollBack()
        {
            try
            {
                this.transaction.Rollback();
                this.finishedTransaction = true;
            }
            catch (MySqlException exception)
            {
                throw new TransactionException(exception.Message);
            }
        }

        internal bool getAutoCommit()
        {
            return false;
        }

        private void initTransaction()
        {
            base.command = base.client.getNewCommand();
            this.transaction = base.client.getTransaction();
            base.command.Transaction = this.transaction;
            base.command.Connection = this.transaction.Connection;
            this.finishedTransaction = false;
        }
    }
}

