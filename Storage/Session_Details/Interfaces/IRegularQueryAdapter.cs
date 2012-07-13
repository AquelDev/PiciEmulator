namespace Pici.Storage.Session_Details.Interfaces
{
    using System;
    using System.Data;
    using Pici.Storage.Database;

    public interface IRegularQueryAdapter
    {
        void addParameter(string name, object query);
        bool findsResult();
        int getInteger();
        DataRow getRow();
        string getString();
        DataTable getTable();
        void runFastQuery(string query);
        void setQuery(string query);
        DatabaseType dbType { get; }
    }
}

