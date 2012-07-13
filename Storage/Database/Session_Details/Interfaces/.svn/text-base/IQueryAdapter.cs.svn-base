namespace Database_Manager.Database.Session_Details.Interfaces
{
    using Database_Manager.Session_Details.Interfaces;
    using System;

    public interface IQueryAdapter : IRegularQueryAdapter, IDisposable
    {
        void doCommit();
        void doRollBack();
        long insertQuery();
        void runQuery();
    }
}

