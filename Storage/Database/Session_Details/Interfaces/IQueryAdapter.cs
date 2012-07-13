namespace Pici.Storage.Database.Session_Details.Interfaces
{
    using System;
    using Pici.Storage.Session_Details.Interfaces;

    public interface IQueryAdapter : IRegularQueryAdapter, IDisposable
    {
        void doCommit();
        void doRollBack();
        long insertQuery();
        void runQuery();
    }
}

