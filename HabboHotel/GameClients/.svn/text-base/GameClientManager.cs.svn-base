using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Butterfly.Core;
using Butterfly.HabboHotel.Misc;
using Butterfly.Messages;
using ConnectionManager;
using Database_Manager.Database.Session_Details.Interfaces;
using System.Threading;
using Butterfly.HabboHotel.Users.Messenger;

namespace Butterfly.HabboHotel.GameClients
{
    class GameClientManager
    {
        #region Fields
        private Dictionary<uint, GameClient> clients;
        
        private Queue clientsAddQueue;
        private Queue clientsToRemove;
        private Queue creditQueuee;
        private Queue badgeQueue;
        private Queue authorizedPacketSending;
        private Queue broadcastQueue;

        private Hashtable usernameRegister;
        private Hashtable userIDRegister;

        private Hashtable usernameIdRegister;
        private Hashtable idUsernameRegister;

        private int pingInterval;

        private bool cycleCreditsEnabled;
        private int cycleCreditsAmount;
        private int cycleCreditsTime;
        private DateTime cycleCreditsLastUpdate;

        private bool cyclePixelsEnabled;
        private int cyclePixelsAmount;
        private int cyclePixelsTime;
        private DateTime cyclePixelsLastUpdate;

        internal int creditsOnLogin;
        internal int pixelsOnLogin;

        #endregion

        #region Return values
        internal int connectionCount
        {
            get
            {
                return clients.Count;
            }
        }

        internal GameClient GetClientByUserID(uint userID)
        {
            if (userIDRegister.ContainsKey(userID))
                return (GameClient)userIDRegister[userID];
            return null;
        }

        internal GameClient GetClientByUsername(string username)
        {
            if (usernameRegister.ContainsKey(username.ToLower()))
                return (GameClient)usernameRegister[username.ToLower()];
            return null;
        }

        internal GameClient GetClient(uint clientID)
        {
            if (clients.ContainsKey(clientID))
                return (GameClient)clients[clientID];
            return null;
        }

        internal int ClientCount
        {
            get
            {
                return clients.Count;
            }
        }

        internal string GetNameById(uint Id)
        {
            GameClient client = GetClientByUserID(Id);

            if (client != null)
                return client.GetHabbo().Username;

            string username;

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT username FROM users WHERE id = " + Id);
                username = dbClient.getString();
            }

            return username;
        }

        internal IEnumerable<GameClient> GetClientsById(Dictionary<uint, MessengerBuddy>.KeyCollection users)
        {
            foreach (uint id in users)
            {
                GameClient client = GetClientByUserID(id);
                if (client != null)
                    yield return client;
            }
        }

        #endregion

        #region Constructor
        internal GameClientManager()
        {
            clients = new Dictionary<uint, GameClient>();
            clientsAddQueue = new Queue();
            clientsToRemove = new Queue();
            creditQueuee = new Queue();
            badgeQueue = new Queue();
            authorizedPacketSending = new Queue();
            broadcastQueue = new Queue();
            timedOutConnections = new Queue();
            usernameRegister = new Hashtable();
            userIDRegister = new Hashtable();

            usernameIdRegister = new Hashtable();
            idUsernameRegister = new Hashtable();

            Thread timeOutThread = new Thread(new ThreadStart(HandleTimeouts));
            timeOutThread.Start();

            pingInterval = int.Parse(ButterflyEnvironment.GetConfig().data["client.ping.interval"]);

            if (ButterflyEnvironment.GetConfig().data.ContainsKey("game.pixel.enabled"))
            {
                cyclePixelsEnabled = (ButterflyEnvironment.GetConfig().data["game.pixel.enabled"] == "true");
                cyclePixelsAmount = int.Parse(ButterflyEnvironment.GetConfig().data["game.pixel.amount"]);
                cyclePixelsTime = int.Parse(ButterflyEnvironment.GetConfig().data["game.pixel.time"]) * 1000;
                pixelsOnLogin = int.Parse(ButterflyEnvironment.GetConfig().data["game.login.pixel.receiveamount"]);
            }
            else
            {
                cyclePixelsEnabled = false;
                cyclePixelsAmount = 0;
                cyclePixelsTime = 0;
                pixelsOnLogin = 0;
            }
            
            if (ButterflyEnvironment.GetConfig().data.ContainsKey("game.credits.enabled"))
            {
                cycleCreditsEnabled = (ButterflyEnvironment.GetConfig().data["game.credits.enabled"] == "true");
                cycleCreditsAmount = int.Parse(ButterflyEnvironment.GetConfig().data["game.credits.amount"]);
                cycleCreditsTime = int.Parse(ButterflyEnvironment.GetConfig().data["game.credits.time"]) * 1000;
                creditsOnLogin = int.Parse(ButterflyEnvironment.GetConfig().data["game.login.credits.receiveamount"]);
            }
            else
            {
                cycleCreditsEnabled = false;
                cycleCreditsAmount = 0;
                cycleCreditsTime = 0;
                creditsOnLogin = 0;
            }
        }
        #endregion

        #region Threads

        internal void OnCycle()
        {
            DateTime time;
            TimeSpan spent;

            time = DateTime.Now;
            CheckCycleUpdates(); // Pixel lolz
            spent = DateTime.Now - time;
            if (spent.TotalSeconds > 3)
                Console.WriteLine("CheckPixelUpdates took really long time to cycle! " + (int)spent.TotalSeconds + "s");

            time = DateTime.Now;
            TestClientConnections(); // User DC <== Causes lagg
            spent = DateTime.Now - time;
            if (spent.TotalSeconds > 3)
                Console.WriteLine("TestClientConnections took really long time to cycle! " + (int)spent.TotalSeconds + "s");

            time = DateTime.Now;
            RemoveClients();
            spent = DateTime.Now - time;
            if (spent.TotalSeconds > 3)
                Console.WriteLine("RemoveClients took really long time to cycle! " + (int)spent.TotalSeconds + "s");

            time = DateTime.Now;
            AddClients();
            spent = DateTime.Now - time;
            if (spent.TotalSeconds > 3)
                Console.WriteLine("AddClients took really long time to cycle! " + (int)spent.TotalSeconds + "s");

            time = DateTime.Now;
            GiveCredits(); //Give credit
            spent = DateTime.Now - time;
            if (spent.TotalSeconds > 3)
                Console.WriteLine("GiveCredits took really long time to cycle! " + (int)spent.TotalSeconds + "s");

            time = DateTime.Now;
            GiveBadges(); //Give badge
            spent = DateTime.Now - time;
            if (spent.TotalSeconds > 3)
                Console.WriteLine("GiveBadgestook really long time to cycle! " + (int)spent.TotalSeconds + "s");

            time = DateTime.Now;
            BroadcastPacketsWithRankRequirement(); //On disconnect 
            spent = DateTime.Now - time;
            if (spent.TotalSeconds > 3)
                Console.WriteLine("BroadcastPacketsWithRankRequirement worker took really long time to cycle! " + (int)spent.TotalSeconds + "s");

            time = DateTime.Now;
            BroadcastPackets(); //On disconnect
            spent = DateTime.Now - time;
            if (spent.TotalSeconds > 3)
                Console.WriteLine("BroadcastPackets worker took really long time to cycle! " + (int)spent.TotalSeconds + "s");
        }

        private static DateTime pixelLastExecution;
        private void CheckCycleUpdates()
        {
            if (cyclePixelsEnabled)
            {
                TimeSpan sinceLastTime = DateTime.Now - cyclePixelsLastUpdate;

                if (sinceLastTime.TotalMilliseconds >= cyclePixelsTime)
                {
                    cyclePixelsLastUpdate = DateTime.Now;
                    try
                    {
                        foreach (GameClient client in clients.Values)
                        {
                            if (client.GetHabbo() == null)
                                continue;

                            PixelManager.GivePixels(client, cyclePixelsAmount);
                        }
                    }

                    catch (Exception e)
                    {
                        Logging.LogThreadException(e.ToString(), "GCMExt.cyclePixelsEnabled task");
                    }
                }
            }

            if (cycleCreditsEnabled)
            {
                TimeSpan sinceLastTime = DateTime.Now - cycleCreditsLastUpdate;

                if (sinceLastTime.TotalMilliseconds >= cycleCreditsTime)
                {
                    cycleCreditsLastUpdate = DateTime.Now;
                    try
                    {
                        foreach (GameClient client in clients.Values)
                        {
                            if (client.GetHabbo() == null)
                                continue;

                            client.GetHabbo().Credits += cycleCreditsAmount;
                            client.GetHabbo().UpdateCreditsBalance();
                        }
                    }

                    catch (Exception e)
                    {
                        Logging.LogThreadException(e.ToString(), "GCMExt.cycleCreditsEnabled task");
                    }
                }
            }

            CheckEffects();
        }

        private Queue timedOutConnections;

        private static DateTime pingLastExecution;
        private void TestClientConnections()
        {
            TimeSpan sinceLastTime = DateTime.Now - pingLastExecution;

            if (sinceLastTime.TotalMilliseconds >= pingInterval)
            {
                try
                {
                    ServerMessage PingMessage = new ServerMessage(50);

                    List<GameClient> ToPing = new List<GameClient>();
                    //List<GameClient> ToDisconnect = new List<GameClient>();


                    TimeSpan noise;
                    TimeSpan sinceLastPing;
                    
                    foreach (GameClient client in clients.Values)
                    {
                        noise = DateTime.Now - pingLastExecution.AddMilliseconds(pingInterval); //For finding out if there is any lagg
                        sinceLastPing = DateTime.Now - client.TimePingedReceived;

                        if (sinceLastPing.TotalMilliseconds - noise.TotalMilliseconds < pingInterval + 10000)
                        {
                            ToPing.Add(client);
                        }
                        else
                        {
                            lock (timedOutConnections.SyncRoot)
                            {
                                timedOutConnections.Enqueue(client);
                            }
                            //ToDisconnect.Add(client);
                            //Console.WriteLine(client.ConnectionID + " => Connection timed out");
                        }
                    }
                    DateTime start = DateTime.Now;

                    byte[] PingMessageBytes = PingMessage.GetBytes();
                    foreach (GameClient Client in ToPing)
                    {
                        try
                        {
                            Client.GetConnection().SendUnsafeData(PingMessageBytes);
                        }
                        catch
                        {
                            //ToDisconnect.Add(Client);
                            lock (timedOutConnections.SyncRoot)
                            {
                                timedOutConnections.Enqueue(Client);
                            }
                        }
                    }

                    TimeSpan spent = DateTime.Now - start;
                    if (spent.TotalSeconds > 3)
                    {
                        Console.WriteLine("Spent seconds on testing: " + (int)spent.TotalSeconds);
                    }


                    //start = DateTime.Now;
                    //foreach (GameClient client in ToDisconnect)
                    //{
                    //    try
                    //    {
                    //        client.Disconnect();
                    //    }
                    //    catch { }
                    //}
                    //spent = DateTime.Now - start;
                    if (spent.TotalSeconds > 3)
                    {
                        Console.WriteLine("Spent seconds on disconnecting: " + (int)spent.TotalSeconds);
                    }

                    //ToDisconnect.Clear();
                    //ToDisconnect = null;
                    ToPing.Clear();
                    ToPing = null;

                    
                }
                catch (Exception e) { Logging.LogThreadException(e.ToString(), "Connection checker task"); }
                pingLastExecution = DateTime.Now;
            }
        }

        private void HandleTimeouts()
        {
            while (true)
            {
                try
                {
                    while (timedOutConnections.Count > 0)
                    {
                        GameClient client = null;
                        lock (timedOutConnections.SyncRoot)
                        {
                            if (timedOutConnections.Count > 0)
                                client = (GameClient)timedOutConnections.Dequeue();
                        }

                        if (client != null)
                        {
                            client.Disconnect();
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.LogThreadException(e.ToString(), "HandleTimeouts");
                }

                Thread.Sleep(5000);
            }
        }        
        #endregion

        #region Collection modyfying
        private void AddClients()
        {
            if (clientsAddQueue.Count > 0)
            {
                lock (clientsAddQueue.SyncRoot)
                {
                    while (clientsAddQueue.Count > 0)
                    {
                        GameClient client = (GameClient)clientsAddQueue.Dequeue();
                        clients.Add(client.ConnectionID, client);
                        client.StartConnection();
                    }
                }
            }
        }

        private void RemoveClients()
        {
            if (clientsToRemove.Count > 0)
            {
                lock (clientsToRemove.SyncRoot)
                {
                    while (clientsToRemove.Count > 0)
                    {
                        uint clientID = (uint)clientsToRemove.Dequeue();
                        clients.Remove(clientID);
                    }
                }
            }
        }

        private void GiveCredits()
        {
            if (creditQueuee.Count > 0)
            {
                lock (creditQueuee.SyncRoot)
                {
                    while (creditQueuee.Count > 0)
                    {
                        int amount = (int)creditQueuee.Dequeue();
                        foreach (GameClient client in clients.Values)
                        {
                            if (client.GetHabbo() == null)
                                continue;
                            try
                            {
                                client.GetHabbo().Credits += amount;
                                client.GetHabbo().UpdateCreditsBalance();
                                client.SendNotif(LanguageLocale.GetValue("user.creditsreceived") + ": " + amount);
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        private void GiveBadges()
        {
            if (badgeQueue.Count > 0)
            {
                lock (badgeQueue.SyncRoot)
                {
                    while (badgeQueue.Count > 0)
                    {
                        string badgeID = (string)badgeQueue.Dequeue();
                        foreach (GameClient client in clients.Values)
                        {
                            if (client.GetHabbo() == null)
                                continue;
                            try
                            {
                                client.GetHabbo().GetBadgeComponent().GiveBadge(badgeID, true);
                                client.SendNotif(LanguageLocale.GetValue("user.badgereceived"));
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        private void BroadcastPacketsWithRankRequirement()
        {
            if (authorizedPacketSending.Count > 0)
            {
                lock (authorizedPacketSending.SyncRoot)
                {
                    while (authorizedPacketSending.Count > 0)
                    {
                        FusedPacket packet = (FusedPacket)authorizedPacketSending.Dequeue();
                        foreach (GameClient client in clients.Values)
                        {
                            try
                            {
                                if (client.GetHabbo() != null && client.GetHabbo().HasFuse(packet.requirements))
                                    client.SendMessage(packet.content);
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        private void BroadcastPackets()
        {
            if (broadcastQueue.Count > 0)
            {
                lock (broadcastQueue.SyncRoot)
                {
                    while (broadcastQueue.Count > 0)
                    {
                        ServerMessage message = (ServerMessage)broadcastQueue.Dequeue();
                        byte[] bytes = message.GetBytes();

                        foreach (GameClient client in clients.Values)
                        {
                            try
                            {
                                client.GetConnection().SendData(bytes);
                            }
                            catch
                            { }
                        }
                    }
                }
            }
        }
        #endregion

        #region Methods
        internal void CreateAndStartClient(uint clientID, ConnectionInformation connection)
        {
            GameClient client = new GameClient(clientID, connection);
            if (clients.ContainsKey(clientID))
                clients.Remove(clientID);

            lock (clientsAddQueue.SyncRoot)
            {
                clientsAddQueue.Enqueue(client);
            }
        }

        internal void DisposeConnection(uint clientID)
        {
            GameClient Client = GetClient(clientID);
            if (Client != null)
            {
                Client.Stop();
            }

            lock (clientsToRemove.SyncRoot)
            {
                clientsToRemove.Enqueue(clientID);
            }
        }

        internal void QueueBroadcaseMessage(ServerMessage message)
        {
            lock (broadcastQueue.SyncRoot)
            {
                broadcastQueue.Enqueue(message);
            }
        }

        internal void QueueBroadcaseMessage(ServerMessage message, string requirements)
        {
            FusedPacket packet = new FusedPacket(message, requirements);
            lock (authorizedPacketSending.SyncRoot)
            {
                authorizedPacketSending.Enqueue(packet);
            }
        }

        private void BroadcastMessage(ServerMessage message)
        {
            lock (broadcastQueue.SyncRoot)
            {
                broadcastQueue.Enqueue(message);
            }
        }

        internal void QueueCreditsUpdate(int amount)
        {
            lock (creditQueuee.SyncRoot)
            {
                creditQueuee.Enqueue(amount);
            }
        }

        internal void QueueBadgeUpdate(string badge)
        {
            lock (badgeQueue.SyncRoot)
            {
                badgeQueue.Enqueue(badge);
            }
        }

        private void GiveAllOnlineCredits(int amount)
        {
            lock (creditQueuee.SyncRoot)
            {
                creditQueuee.Enqueue(amount);
            }
        }

        internal static void UnregisterConnection(uint connectionID)
        {
        }

        private void CheckEffects()
        {
            foreach (GameClient client in clients.Values)
            {
                if (client.GetHabbo() == null || client.GetHabbo().GetAvatarEffectsInventoryComponent() == null)
                    continue;

                client.GetHabbo().GetAvatarEffectsInventoryComponent().CheckExpired();
            }
        }

        internal void LogClonesOut(uint UserID)
        {
            GameClient client = GetClientByUserID(UserID);
            if (client != null)
                client.Disconnect();
        }

        internal void RegisterClient(GameClient client, uint userID, string username)
        {
            if (usernameRegister.ContainsKey(username.ToLower()))
                usernameRegister[username.ToLower()] = client;
            else
                usernameRegister.Add(username.ToLower(), client);

            if (userIDRegister.ContainsKey(userID))
                userIDRegister[userID] = client;
            else
                userIDRegister.Add(userID, client);

            if (!usernameIdRegister.ContainsKey(username))
                usernameIdRegister.Add(username, userID);
            if (!idUsernameRegister.ContainsKey(userID))
                idUsernameRegister.Add(userID, username);

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (dbClient.dbType == Database_Manager.Database.DatabaseType.MySQL)
                    dbClient.setQuery("REPLACE INTO user_online VALUES (" + userID + ")");
                else
                    dbClient.setQuery("IF NOT EXISTS (SELECT userid FROM user_online WHERE userid = " + userID + ") " +
                                      "   INSERT INTO user_online VALUES (" + userID + ")");
            }
        }

        internal void UnregisterClient(uint userid, string username)
        {
            userIDRegister.Remove(userid);
            usernameRegister.Remove(username.ToLower());

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("DELETE FROM user_online WHERE id = " + userid);
            }
        }

        internal void CloseAll()
        {
            StringBuilder QueryBuilder = new StringBuilder();
            bool RunUpdate = false;
            int Count = 0;

            foreach (GameClient client in clients.Values)
            {
                if (client.GetHabbo() != null)
                    Count++;
            }
            if (Count < 1)
                Count = 1;

            int current = 0;
            int ClientLength = clients.Count;
            foreach (GameClient client in clients.Values)
            {
                current++;
                if (client.GetHabbo() != null)
                {
                    try
                    {
                        client.GetHabbo().GetInventoryComponent().RunDBUpdate();
                        QueryBuilder.Append(client.GetHabbo().GetQueryString);
                        RunUpdate = true;

                        Console.Clear();
                        Console.WriteLine("<<- SERVER SHUTDOWN ->> IVNENTORY SAVE: " + String.Format("{0:0.##}", ((double)current / Count) * 100) + "%");
                    }
                    catch { }
                }
            }

            if (RunUpdate)
            {
                try
                {
                    if (QueryBuilder.Length > 0)
                    {
                        using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            dbClient.runFastQuery(QueryBuilder.ToString());
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.HandleException(e, "GameClientManager.CloseAll()");
                }
            }
            

            Console.WriteLine("Done saving users inventory!");
            Console.WriteLine("Closing server connections...");
            try
            {
                int i = 0;
                foreach (GameClient client in clients.Values)
                {
                    i++;
                    if (client.GetConnection() != null)
                    {
                        try
                        {
                            client.GetConnection().Dispose();
                        }
                        catch { }

                        Console.Clear();
                        Console.WriteLine("<<- SERVER SHUTDOWN ->> CONNECTION CLOSE: " + String.Format("{0:0.##}", ((double)i / ClientLength) * 100) + "%");
                    }
                }
            }
            catch (Exception e)
            {
                Logging.LogCriticalException(e.ToString());
            }
            clients.Clear();
            Console.WriteLine("Connections closed!");
        }
        #endregion
    }
}
