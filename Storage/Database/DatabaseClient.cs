namespace Database_Manager.Database
{
    using Database_Manager.Database.Session_Details;
    using Database_Manager.Database.Session_Details.Interfaces;
    using MySql.Data.MySqlClient;
    using System;
    using System.Data;
    using System.Threading;

    public class DatabaseClient : IDisposable
    {
        private MySqlConnection connection;
        //private int connectionID;
        private DatabaseManager dbManager;
        private IQueryAdapter info;
        //private DateTime lastActivity;
        //private static readonly int MAX_IDLE_CONNECTION_TIME = 0x493e0; //300.000 0x493e0
        //private static Random rnd = new Random();
        //private DateTime timeConnected;
        //private ConnectionState state;

        public DatabaseClient(DatabaseManager dbManager, int id)
        {
            this.dbManager = dbManager;
            //this.connectionID = id;
            //this.lastActivity = DateTime.Now;
            //this.state = ConnectionState.Closed;
            this.connection = new MySqlConnection(dbManager.getConnectionString());
            //this.connection.StateChange += new StateChangeEventHandler(this.connecionStateChanged);
        }

        //private void connecionStateChanged(object sender, StateChangeEventArgs e)
        //{
        //    this.state = e.CurrentState;
        //}

        public void connect()
        {
            this.connection.Open();
            //this.timeConnected = DateTime.Now;
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

        //public ConnectionState getConnectionState()
        //{
        //    TimeSpan span = DateTime.Now - this.lastActivity;
        //    int totalMilliseconds = (int) span.TotalMilliseconds;
        //    if (totalMilliseconds >= MAX_IDLE_CONNECTION_TIME)
        //    {
        //        return ConnectionState.Broken;
        //    }
        //    return this.state;
        //}

        //public bool pendingReset()
        //{
        //    TimeSpan span = DateTime.Now - this.lastActivity;
        //    int totalMilliseconds = (int)span.TotalMilliseconds;
        //    if (totalMilliseconds >= MAX_IDLE_CONNECTION_TIME)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //internal int getID()
        //{
        //    return this.connectionID;
        //}

        //internal DateTime getLastAction()
        //{
        //    return this.lastActivity;
        //}

        internal MySqlCommand getNewCommand()
        {
            //this.lastActivity = DateTime.Now;
            return this.connection.CreateCommand();
        }

        internal IQueryAdapter getQueryReactor()
        {
            return this.info;
        }

        internal MySqlTransaction getTransaction()
        {
            //this.lastActivity = DateTime.Now;
            return this.connection.BeginTransaction();
        }

        internal bool isAvailable()
        {
            return (this.info == null);
        }

        public void prepare(bool autoCommit)
        {
            if (autoCommit)
            {
                this.info = new TransactionQueryReactor(this);
            }
            else
            {
                this.info = new NormalQueryReactor(this);
            }
        }

        internal void reportDone()
        {
            this.Dispose();
        }
    }
}

