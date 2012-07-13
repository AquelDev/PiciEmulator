using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Butterfly.Core;

namespace Butterfly.Storage
{
    internal class DatabaseManagerOld
    {
        #region Fields
        private readonly string connectionString;
        private readonly DatabasePropertyOld databaseSettings;
        private readonly ReaderWriterLockSlim lockObject;

        private Dictionary<int, DatabaseClientOld> databaseClients;
        private Task clientChecker;
        #endregion

        #region Constructor
        internal DatabaseManagerOld(DatabasePropertyOld settings)
        {
            this.connectionString = settings.GenerateConnectionString();
            this.databaseSettings = settings;
            this.databaseClients = new Dictionary<int, DatabaseClientOld>();
            this.lockObject = new ReaderWriterLockSlim();

            this.clientChecker = new Task(new Action(MonitorConnections));
            this.clientChecker.Start();

            SetClientAmount(settings.minPoolSize + 100);
        }
        #endregion

        #region Methods
        internal void DestroyClients()
        {
            lockObject.EnterWriteLock();

            foreach (DatabaseClientOld client in databaseClients.Values)
                client.Destroy();

            databaseClients.Clear();

            lockObject.ExitWriteLock();
        }

        private void MonitorConnections()
        {
            while (!ButterflyEnvironment.ShutdownStarted)
            {
                try
                {
                    lockObject.EnterReadLock();

                    int timeStamp = ButterflyEnvironment.GetUnixTimestamp();
                    foreach (DatabaseClientOld client in databaseClients.Values.Where(p => (timeStamp - p.ActivityStamp) >= 60 && !p.isWorking && p.State == ConnectionState.Open))
                    {
                        client.Disconnect();
                    }

                    lockObject.ExitReadLock();
                }
                catch (Exception ex)
                {
                    Logging.LogThreadException(ex.ToString(), "DatabaseManager task");
                }
                Thread.Sleep(10000); // 10 seconds
            }

        }
        internal DatabaseClientOld GetClient()
        {
            lockObject.EnterReadLock();
            DatabaseClientOld returnClient = null;

            foreach (DatabaseClientOld client in databaseClients.Values)
            {
                if (client.isWorking)
                    continue;
                if (client.State == ConnectionState.Closed)
                    client.Connect();
                if (client.State == ConnectionState.Open)
                {
                    returnClient = client;
                    returnClient.UpdateLastActivity();
                    break;
                }
            }

            lockObject.ExitReadLock();
            if (returnClient != null)
                return returnClient;

            if (databaseClients.Count < databaseSettings.maxPoolSize)
            {
                lockObject.EnterWriteLock();
                int newID = databaseClients.Count + 1;
                DatabaseClientOld newClient = new DatabaseClientOld((uint)newID, connectionString, this);
                databaseClients.Add(newID, newClient);
                newClient.Connect();
                newClient.UpdateLastActivity();

                lockObject.ExitWriteLock();
                return newClient;
            }

            DatabaseClientOld anonymousClient = new DatabaseClientOld(0, connectionString, this);
            anonymousClient.Connect();
            anonymousClient.UpdateLastActivity();
            return anonymousClient;
        }

        internal void SetClientAmount(uint amount)
        {
            lockObject.EnterWriteLock();
            for (int i = 0; i < amount; i++)
            {
                int newID = databaseClients.Count + 1;
                DatabaseClientOld newClient = new DatabaseClientOld((uint)newID, connectionString, this);
                databaseClients.Add(newID, newClient);
                newClient.Connect();
            }
            lockObject.ExitWriteLock();
        }

        #endregion

        #region Values
        internal int ConnectionCount
        {
            get
            {
                lockObject.EnterReadLock();
                int connectionCount = databaseClients.Count(p => p.Value.State != ConnectionState.Closed);
                lockObject.ExitReadLock();

                return connectionCount;
            }
        }


        internal List<DatabaseClientOld> GetClients()
        {
            List<DatabaseClientOld> clients = new List<DatabaseClientOld>();
            
            lockObject.EnterReadLock();
            clients.AddRange(databaseClients.Values);
            lockObject.ExitReadLock();

            return clients;
        }
        #endregion
    }
}