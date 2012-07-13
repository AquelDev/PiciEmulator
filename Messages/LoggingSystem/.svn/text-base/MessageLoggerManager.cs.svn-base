using System;
using System.Collections;
using Butterfly.Messages.LoggingSystem;
using Database_Manager.Database.Session_Details.Interfaces;
using Butterfly;

namespace Butterfly.Messages
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
            TimeSpan timeSpent = DateTime.Now - timeSinceLastPacket;
            timeSinceLastPacket = DateTime.Now;
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
                    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
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
