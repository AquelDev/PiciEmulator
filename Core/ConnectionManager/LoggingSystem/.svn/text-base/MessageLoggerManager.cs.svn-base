using System;
using System.Collections;
using ConnectionManager.LoggingSystem;
using Database_Manager.Database;
using Database_Manager.Database.Session_Details.Interfaces;

namespace ConnectionManager.Messages
{
    class MessageLoggerManager
    {
        private static Queue loggedMessages;
        private static bool enabled;
        private static DateTime timeSinceLastPacket;

        internal static bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
                if (enabled)
                {
                    loggedMessages = new Queue();
                }
            }
        }

        internal static void AddMessage(byte[] data, int connectionID, LogState state)
        {
            if (!enabled)
                return;

            string message;
            switch (state)
            {
                case LogState.ConnectionClose:
                    {
                        message = "CONOPEN";
                        break;
                    }

                case LogState.ConnectionOpen:
                    {
                        message = "CONCLOSE";
                        break;
                    }

                default:
                    {
                        message = System.Text.Encoding.Default.GetString(data);
                        break;
                    }
            }

            lock (loggedMessages.SyncRoot)
            {
                Message loggedMessage = new Message(connectionID, GenerateTimestamp(), message);
                loggedMessages.Enqueue(loggedMessage);
            }
        }

        private static int GenerateTimestamp()
        {
            DateTime now = DateTime.Now;
            TimeSpan timeSpent = now - timeSinceLastPacket;
            timeSinceLastPacket = now;

            return (int)timeSpent.TotalMilliseconds;
        }

        internal static void Save()
        {
            if (!enabled)
                return;

            lock (loggedMessages.SyncRoot)
            {
                int totalMessages = loggedMessages.Count;

                if (loggedMessages.Count > 0)
                {
                    DatabaseManager dbManager = new DatabaseManager(1, 1, DatabaseType.MySQL);
                    //To-do: Init dbManager from configuration file

                    using (IQueryAdapter dbClient = dbManager.getQueryreactor())
                    {
                        while (loggedMessages.Count > 0)
                        {
                            Message message = (Message)loggedMessages.Dequeue();

                            dbClient.setQuery("INSERT INTO system_packetlog (connectionid, timestamp, data) VALUES @connectionid @timestamp, @data");
                            dbClient.addParameter("connectionid", message.ConnectionID);
                            dbClient.addParameter("timestamp", message.GetTimestamp);
                            dbClient.addParameter("data", message.GetData);

                            dbClient.runQuery();

                        }
                    }
                }
            }
        }
    }
}
