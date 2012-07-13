//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Data;

//using Uber.Storage;
//using Uber.HabboHotel.Users.Authenticator;
//using Uber.HabboHotel.Support;
//using Uber.Core;
//using Uber.Collections;

//namespace Uber.HabboHotel.GameClients
//{
//    // Extended functionality for GameClientManager

//    partial class GameClientManager
//    {
//        internal void LogClonesOut(string Username)
//        {
//            for (int i = 0; i < Clients.Length; i++)
//            {
//                GameClient Client = Clients[i];
//                if (Client == null)
//                    continue;
//                if (Client.GetHabbo() != null && Client.GetHabbo().Username == Username)
//                    Client.Disconnect();
//            }
//        }

//        internal string GetNameById(uint Id)
//        {
//            GameClient Cl = GetClientByHabbo(Id);

//            if (Cl != null)
//            {
//                return Cl.GetHabbo().Username;
//            }

//            DataRow Row = null;

//            using (DatabaseClient dbClient = ButterflyEnvironment.GetDatabase().GetClient())
//            {
//                Row = dbClient.getRow("SELECT username FROM users WHERE id = '" + Id + "' LIMIT 1");
//            }

//            if (Row == null)
//            {
//                return "Unknown User";
//            }

//            return (string)Row[0];
//        }

//        internal void DeployHotelCreditsUpdate()
//        {
//            for (int i = 0; i < Clients.Length; i++)
//            {
//                GameClient Client = Clients[i];
//                if (Client.GetHabbo() == null)
//                {
//                    continue;
//                }

//                int newCredits = 0;

//                using (DatabaseClient dbClient = ButterflyEnvironment.GetDatabase().GetClient())
//                {
//                    newCredits = (int)dbClient.getRow("SELECT credits FROM users WHERE id = '" + Client.GetHabbo().Id + "' LIMIT 1")[0];
//                }

//                int oldBalance = Client.GetHabbo().Credits;

//                Client.GetHabbo().Credits = newCredits;

//                if (oldBalance < 3000)
//                {
//                    Client.GetHabbo().UpdateCreditsBalance(false);
//                    Client.SendNotif("Uber Credits Update" + Convert.ToChar(13) + "-----------------------------------" + Convert.ToChar(13) + "We have refilled your credits up to 3000 - enjoy! The next credits update will occur in 3 hours.", "http://uber.meth0d.org/credits");
//                }
//                else if (oldBalance >= 3000)
//                {
//                    Client.SendNotif("Uber Credits Update" + Convert.ToChar(13) + "-----------------------------------" + Convert.ToChar(13) + "Sorry! Because your credit balance is 3000 or higher, we have not refilled your credits. The next credits update will occur in 3 hours.", "http://uber.meth0d.org/credits");
//                }

//            }

//        }

//        internal void CheckForAllBanConflicts()
//        {
//            Dictionary<GameClient, ModerationBanException> ConflictsFound = new Dictionary<GameClient, ModerationBanException>();
//            for (int i = 0; i < Clients.Length; i++)
//            {
//                GameClient Client = Clients[i];
//                if (Client == null)
//                    continue;
//                try
//                {
//                    ButterflyEnvironment.GetGame().GetBanManager().CheckForBanConflicts(Client);
//                }

//                catch (ModerationBanException e)
//                {
//                    ConflictsFound.Add(Client, e);
//                }

//            }

//            foreach (KeyValuePair<GameClient, ModerationBanException> Data in ConflictsFound)
//            {
//                Data.Key.SendBanMessage(Data.Value.Message);
//                Data.Key.Disconnect();
//            }
//        }

//        internal void CheckPixelUpdates()
//        {
//            try
//            {
//                if (Clients == null)
//                    return;

//                for (int i = 0; i < Clients.Length; i++)
//                {
//                    GameClient Client = Clients[i];
//                    if (Client == null)
//                        continue;
//                    if (Client.GetHabbo() == null || !ButterflyEnvironment.GetGame().GetPixelManager().NeedsUpdate(Client))
//                        continue;

//                    ButterflyEnvironment.GetGame().GetPixelManager().GivePixels(Client);
//                }
//            }

//            catch (Exception e)
//            {
//                //Logging.WriteLine("[GCMExt.CheckPixelUpdates]: " + e.Message);
//                Logging.LogException("[GCMExt.CheckPixelUpdates]: " + e.ToString());
//            }
//        }
//    }
//}
