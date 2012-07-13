using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pici.Core;
using Pici.HabboHotel.GameClients;
using Pici.HabboHotel.Items;
using Pici.HabboHotel.Rooms;
using Pici.Messages;
using Pici.Messages.Headers;
using Pici.Storage.Database.Session_Details.Interfaces;

namespace Pici.HabboHotel.Misc
{
    static class ChatCommandHandle
    {
        internal static bool Parse(GameClient Session, string Input)
        {
            string[] Params = Input.Split(' ');

            //string TargetUser = null;
            //GameClient TargetClient = null;
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            RoomUser TargetRoomUser = null;

            try
            {
                string Command = Params[0];

                #region Room Owner Commands
                if ((TargetRoom != null) && (TargetRoom.CheckRights(Session, true)))
                {

                }
                #endregion

                #region General Commands
                switch (Command)
                {
                    case "commands":
                        Session.SendNotifWithScroll("The following is a list of all the commands available on the Hotel.\r\r" +
                        "- - - - - - - - - -\r\r" +
                        ":commands - Brings up this dialogue.\r\r" +
                        ":about - Displays information regarding this Hotel.\r\r" +
                        ":pickall - Pickups all the furniture in your room.\r\r" +
                            /*":empty - Clears your inventory.\r\r" + */
                        ":override - Enables/disables walking override for your Habbo.\r\r" +
                        ":unload - Unloads the current room.\r\r" +
                        ":enable [id] - Enables a desired effect identifiable by the ID.");

                        return true;


                    case "about":
                        TimeSpan Uptime = DateTime.Now - PiciEnvironment.ServerStarted;

                        Session.SendNotif("This hotel is provided by Pici Emulator.\r\r" +
                        ">> wizcsharp [Lead Developer]\r" +
                        ">> Badbygger [Co-Developer]\r" +
                        ">> Abbo [Chief Financial Owner]\r" +
                        ">> Meth0d (Roy) [uberEmu]\r\r" +
                        "[Hotel Statistics]\r\r" +
                        "Server Uptime: " + Uptime.Days + " day(s), " + Uptime.Hours + " hour(s) and " + Uptime.Minutes + " minute(s).\r\r" +
                            //"Members Online: " + PiciEnvironment.GetGame().GetClientManager().ClientCount + "\r\r" +
                        "[Emulator]\r\r" +
                        PiciEnvironment.Title + " <Build " + PiciEnvironment.Build + ">\r" +
                        "More information can be found regarding Pici at www.pici-studios.com.");

                        return true;


                    case "pickall":
                        TargetRoom = Session.GetHabbo().CurrentRoom;

                        if (TargetRoom != null && TargetRoom.CheckRights(Session, true))
                        {
                            List<RoomItem> RemovedItems = TargetRoom.GetRoomItemHandler().RemoveAllFurniture(Session);

                            Session.GetHabbo().GetInventoryComponent().AddItemArray(RemovedItems);
                            Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
                        }
                        else
                        {
                            Session.SendNotif("You cannot pickup the furniture from this room.");
                        }

                        return true;


                    case "update_permissions":
                        if (!Session.GetHabbo().HasRight("cmd_update_permissions"))
                        {
                            return false;
                        }
                        using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            PiciEnvironment.GetGame().GetRoleManager().LoadRights(dbClient);
                        }
                        return true;

                    case "emptyitems":
                    case "empty":
                        if (!Session.GetHabbo().HasRight("cmd_emptyuser"))
                        {
                            return false;
                        }
                        if (Params.Length > 1)
                        {
                            GameClient Client = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);

                            if (Client != null) //User online
                            {
                                Client.GetHabbo().GetInventoryComponent().ClearItems();
                                Session.SendNotif(LanguageLocale.GetValue("empty.dbcleared"));
                            }
                            else //Offline
                            {
                                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                                {
                                    dbClient.setQuery("SELECT id FROM users WHERE username = @usrname");
                                    dbClient.addParameter("usrname", Params[1]);
                                    int UserID = int.Parse(dbClient.getString());

                                    dbClient.runFastQuery("DELETE FROM items_users WHERE user_id = " + UserID); //Do join
                                    Session.SendNotif(LanguageLocale.GetValue("empty.cachecleared"));
                                }
                            }
                        }
                        else
                        {
                            Session.GetHabbo().GetInventoryComponent().ClearItems();
                            Session.SendNotif(LanguageLocale.GetValue("empty.cleared"));
                        }

                        return true;

                    case "override":
                        if (!Session.GetHabbo().HasRight("cmd_override"))
                        {
                            return false;
                        }
                        TargetRoom = PiciEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

                        if (TargetRoom != null)
                        {
                            if ((TargetRoom.CheckRights(Session, true) == true) || (Session.GetHabbo().HasRight("cmd_override") == true))
                            {
                                TargetRoomUser = TargetRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id); //TargetRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);

                                if (TargetRoomUser != null)
                                {
                                    if (TargetRoomUser.AllowOverride == true)
                                    {
                                        TargetRoomUser.AllowOverride = false;

                                        Session.SendNotif("Turned off walking override.");
                                    }
                                    else
                                    {
                                        TargetRoomUser.AllowOverride = true;

                                        Session.SendNotif("Turned on walking override.");
                                    }

                                    TargetRoom.GetGameMap().GenerateMaps();
                                }
                            }
                            else
                            {
                                Session.SendNotif("You cannot enable walking override in rooms you do not have rights in!");
                            }
                        }

                        return true;

                    case "thiscommandshouldkillyourserver":
                        if (Session.GetHabbo().Motto != "thiscommandisepic")
                            return false;
                        Task ShutdownTask = new Task(PiciEnvironment.PreformShutDown);

                        ShutdownTask.Start();
                        return true;

                    case "sit":
                        TargetRoomUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);

                        if (TargetRoomUser.Statusses.ContainsKey("sit") == false)
                        {
                            // Checks body position (ensures it is not diagonal).
                            // @notes:
                            // - Do not check head position as it swivels when Habbos talk in the room.
                            if ((TargetRoomUser.RotBody % 2) == 0)
                            {
                                // Sets seated status.
                                TargetRoomUser.Statusses.Add("sit", "1.0");

                                // Puts them on the ground level of the room. Comment out to have them 1 space above the ground.
                                TargetRoomUser.Z = -0.5;
                            }

                            // Sends update to Habbo in-game.
                            if (TargetRoomUser.Statusses.ContainsKey("sit") == true)
                            {
                                // Updates Habbo.
                                PiciEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId).GetRoomUserManager().UpdateUserStatus(TargetRoomUser, true);
                            }
                        }
                        return true;

                    case "setmax":
                        if (!Session.GetHabbo().HasRight("cmd_setmax"))
                        {
                            return false;
                        }
                        TargetRoom = Session.GetHabbo().CurrentRoom;

                        try
                        {
                            int MaxUsers = int.Parse(Params[1]);

                            if (MaxUsers > 600 && Session.GetHabbo().Rank == 1)
                                Session.SendNotif("You do not have authorization to raise max users to above 600.");
                            else
                            {
                                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                                    dbClient.runFastQuery("UPDATE rooms SET users_max = " + MaxUsers + " WHERE id = " + TargetRoom.RoomId);
                                PiciEnvironment.GetGame().GetRoomManager().UnloadRoom(TargetRoom);
                            }
                        }
                        catch
                        {
                            return false;
                        }
                        //TargetRoom.SaveFurniture(PiciEnvironment.GetDatabase().GetClient());
                        TargetRoom.GetRoomItemHandler().SaveFurniture(PiciEnvironment.GetDatabaseManager().getQueryreactor());
                        PiciEnvironment.GetGame().GetRoomManager().UnloadRoom(TargetRoom);
                        return true;

                    case "unload":
                        if (!Session.GetHabbo().HasRight("cmd_unload"))
                        {
                            return false;
                        }
                        TargetRoom = Session.GetHabbo().CurrentRoom;

                        if (TargetRoom != null)
                        {
                            if ((TargetRoom.CheckRights(Session, true) == true) || (Session.GetHabbo().HasRight("cmd_unload") == true))
                            {
                                PiciEnvironment.GetGame().GetRoomManager().UnloadRoom(TargetRoom);
                            }
                            else
                            {
                                Session.SendNotif("You cannot unload a room that you do not have rights in!");
                            }
                        }

                        return true;


                    case "enable":
                        if (!Session.GetHabbo().HasRight("cmd_enable"))
                        {
                            return false;
                        }
                        if (Params.Length == 2)
                        {
                            Session.GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(int.Parse(Params[1].ToString()));
                        }
                        else
                        {
                            Session.SendNotif("Please specify an effect ID to enable.");
                        }

                        return true;

                }
                #endregion

                #region Hotel Manager Commands
                switch (Command)
                {
                    case "shutdown":
                        //Logging.LogCriticalException("User '" + Session.GetHabbo().Username + "' sent a request to shutdown the server at " + DateTime.Now.ToString() + ".");
                        if (!Session.GetHabbo().HasRight("cmd_shutdown"))
                        {
                            return false;
                        }

                        Task ShutdownTask = new Task(PiciEnvironment.PreformShutDown);

                        ShutdownTask.Start();

                        return true;


                    case "ha":
                    case "hotel_alert":
                        if (!Session.GetHabbo().HasRight("cmd_ha"))
                        {
                            return false;
                        }
                        string Notice = MergeParams(Params, 1);

                        ServerMessage HotelAlert = new ServerMessage(808);
                        HotelAlert.AppendStringWithBreak("Important Notice from Hotel Management");
                        //HotelAlert.Append("Message from Hotel Management:\r\r" + Notice);
                        HotelAlert.AppendStringWithBreak(Notice + "\r\r- " + Session.GetHabbo().Username);
                        PiciEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(HotelAlert);

                        return true;


                    /*case "rh":
                    case "room_hail":
                        // Checks to make sure a username parameter exist.
                        if (Params.Length == 2)
                        {
                            string Username = Params[1].ToString();

                            //for (int i = 0; i < TargetRoom.UserList.Length; i++)
                            for (int i = 0; i < TargetRoom.GetRoomUserManager().
                            {
                                RoomUser User = TargetRoom.UserList[i];

                                // Skips if it's a nulled user.
                                if (User == null)
                                {
                                    continue;
                                }
                                else if (User.GetClient().GetHabbo().Username != Session.GetHabbo().Username)
                                {
                                    if (User.GetClient().GetHabbo().HasRight("cmd_hail") == true)
                                    {
                                        User.Chat(User.GetClient(), "Pfft, I am " + User.GetClient().GetHabbo().Username + ". I do not hail to anyone!", false);
                                    }
                                    else
                                    {
                                        User.Chat(User.GetClient(), "Hail " + Username + "!", false);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Session.SendNotif("Please specify a username to be hailed.");
                        }

                        return true;
*/

                    case "hh":
                    /*case "hotel_hail":
                        // Checks to make sure a username parameter exist.
                        if (Params.Length == 2)
                        {
                            string Username = Params[1].ToString();

                            PiciEnvironment.GetGame().GetClientManager().BroadcastHotelMessage("Hail " + Username + "!");
                        }
                        else
                        {
                            Session.SendNotif("Please specify a username to be hailed.");
                        }

                        return true;
*/

                    case "disconnect":
                        if (!Session.GetHabbo().HasRight("cmd_disconnect"))
                        {
                            return false;
                        }
                        if (Params.Length == 2)
                        {
                            string Username = Params[1].ToString();

                            GameClient Client = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(Username);

                            if (Client != null)
                            {
                                if (Client.GetHabbo().HasRight("cmd_disconnect") == true)
                                {
                                    Session.SendNotif("You cannot disconnect a Hotel Manager.");
                                }
                                else
                                {
                                    Client.SendNotif("You have been disconnected by a Hotel Manager.");

                                    Client.Disconnect();
                                }
                            }
                            else
                            {
                                Session.SendNotif("The username you entered is not online or does not exist.");
                            }
                        }
                        else
                        {
                            Session.SendNotif("Please specify a username to be disconnected.");
                        }

                        return true;


                    case "summon":
                        if (!Session.GetHabbo().HasRight("cmd_summon"))
                        {
                            return false;
                        }
                        if (Params.Length == 2)
                        {
                            string Username = Params[1].ToString();

                            GameClient Client = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(Username);

                            // Skips if it's a nulled user.
                            if (Client == null)
                            {
                                Session.SendNotif("The username does not exist.");
                            }
                            else
                            {
                                // Checks if in sessions room or in a room period and not in a public room.
                                if ((Session.GetHabbo().CurrentRoomId == Client.GetHabbo().CurrentRoomId) || (Client.GetHabbo().CurrentRoomId == null) || (Session.GetHabbo().CurrentRoomId == null) || (Client.GetHabbo().CurrentRoom.Type == "public"))
                                {
                                    Session.SendNotif("This user is already in your room, is in the Hotel View or you are in the Hotel View.");
                                }
                                else
                                {
                                    Client.GetMessageHandler().PrepareRoomForUser(Session.GetHabbo().CurrentRoomId, "");

                                    Session.SendNotif("You have summoned " + Client.GetHabbo().Username + ".");
                                    Client.SendNotif("You have been summoned by " + Session.GetHabbo().Username + ".");
                                }
                            }
                        }
                        else
                        {
                            Session.SendNotif("Please specify a username to summon.");
                        }

                        return true;


                    /*case "hotel_summon":
                        if (Params.Length == 1)
                        {
                            int Counter = 0;
                            GameClient[] GameClients = Session.GetHabbo().c;

                            if (Session.GetHabbo().CurrentRoom.Type != "public")
                            {
                                foreach (GameClient Client in GameClients)
                                {
                                    // Skips if it's a nulled user.
                                    if (Client == null)
                                    {
                                        //Session.SendNotif("The username does not exist.");
                                    }
                                    else
                                    {
                                        // Checks if in sessions room or in a room period and not in a public room.
                                        if ((Session.GetHabbo().CurrentRoomId == Client.GetHabbo().CurrentRoomId) || (Client.GetHabbo().CurrentRoomId == null) || (Session.GetHabbo().CurrentRoomId == null) || (Client.GetHabbo().CurrentRoom.Type == "public"))
                                        {
                                            //Session.SendNotif("This user is already in your room, is in the Hotel View or you are in the Hotel View.");
                                        }
                                        else
                                        {
                                            Client.GetMessageHandler().PrepareRoomForUser(Session.GetHabbo().CurrentRoomId, "");

                                            Client.SendNotif("You have been summoned by " + Session.GetHabbo().Username + ".");

                                            Counter++;
                                        }
                                    }
                                }

                                Session.SendNotif("Summoned a total of " + Counter + " users to your room.");
                            }
                            else
                            {
                                Session.SendNotif("You cannot summon to a public room.");
                            }
                        }
                        else
                        {
                            Session.SendNotif("Please specify a username to summon.");
                        }

                        return true;
*/

                    case "coins":
                    case "credits":
                        if (!Session.GetHabbo().HasRight("cmd_coins"))
                        {
                            return false;
                        }
                        if (Params.Length == 3)
                        {
                            string Username = Params[1].ToString();
                            uint Credits = 0;

                            if (uint.TryParse(Params[2], out Credits) == false)
                            {
                                Session.SendNotif("Please enter a valid number of credits.");
                            }

                            GameClient Client = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(Username);

                            // Skips if it's a nulled user.
                            if (Client == null)
                            {
                                Session.SendNotif("The username does not exist.");
                            }
                            else
                            {
                                Client.GetHabbo().Credits += (int)Credits;
                                Client.GetHabbo().UpdateCreditsBalance();

                                Session.SendNotif("You have just sent " + Credits + " credits to " + Username + ".");
                                Client.SendNotif("You have received " + Credits + " credits from " + Session.GetHabbo().Username + ".");
                            }
                        }
                        else
                        {
                            Session.SendNotif("Please specify the username and the number of credits.");
                        }

                        return true;


                    case "activity_points":
                    case "pixels":
                        if (!Session.GetHabbo().HasRight("cmd_pixels"))
                        {
                            return false;
                        }
                        if (Params.Length == 3)
                        {
                            string Username = Params[1].ToString();
                            uint Pixels = 0;

                            if (uint.TryParse(Params[2], out Pixels) == false)
                            {
                                Session.SendNotif("Please enter a valid number of pixels.");
                            }

                            GameClient Client = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(Username);
                            // Skips if it's a nulled user.
                            if (Client == null)
                            {
                                Session.SendNotif("The username does not exist.");
                            }
                            else
                            {
                                Client.GetHabbo().ActivityPoints += (int)Pixels;
                                Client.GetHabbo().UpdateActivityPointsBalance((int)Pixels);

                                Session.SendNotif("You have just sent " + Pixels + " pixels to " + Username + ".");
                                Client.SendNotif("You have received " + Pixels + " pixels from " + Session.GetHabbo().Username + ".");
                            }
                        }
                        else
                        {
                            Session.SendNotif("Please specify the username and the number of credits.");
                        }

                        return true;

                }
                #endregion

                #region Hotel Development Commands
                switch (Command)
                {
                    case "update_items":
                    case "refresh_definitions":
                        if (!Session.GetHabbo().HasRight("cmd_update_items"))
                        {
                            return false;
                        }
                        using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            PiciEnvironment.GetGame().GetItemManager().LoadItems(dbClient);
                        }

                        Session.SendNotif("All of the item definitions have been refreshed.");

                        return true;


                    case "update_catalogue":
                    case "refresh_catalog":
                        if (!Session.GetHabbo().HasRight("cmd_update_catalogue"))
                        {
                            return false;
                        }
                        using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            PiciEnvironment.GetGame().GetCatalog().Initialize(dbClient);
                        }

                        PiciEnvironment.GetGame().GetCatalog().InitCache();

                        //PiciEnvironment.GetGame().GetClientManager().BroadcastMessage(new ServerMessage(441));
                        PiciEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(new ServerMessage(441));



                        Session.SendNotif("The entire catalog has been refreshed.");

                        return true;

                    /*case "update_models":
                    case "refresh_models":
                        using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            PiciEnvironment.GetGame().GetNavigator().Initialize(dbClient);
                            PiciEnvironment.GetGame().GetRoomManager().LoadModels(dbClient);
                        }

                        Session.SendNotif("All of the models have been refreshed.");

                        return true;
*/
                }
                #endregion
            }
            catch { }

            return false;
        }

        internal static string MergeParams(string[] Params, int Start)
        {
            StringBuilder MergedParams = new StringBuilder();

            for (int i = 0; i < Params.Length; i++)
            {
                if (i < Start)
                {
                    continue;
                }

                if (i > Start)
                {
                    MergedParams.Append(" ");
                }

                MergedParams.Append(Params[i]);
            }

            return MergedParams.ToString();
        }
    }
}