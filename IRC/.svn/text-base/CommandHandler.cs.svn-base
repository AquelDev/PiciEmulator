//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Butterfly;
//using Butterfly.Core;
//using Butterfly.HabboHotel;
//using Butterfly.HabboHotel.Rooms;
//using Butterfly.HabboHotel.Users;
//using Butterfly.Messages;
//using Butterfly.Net;
//using Butterfly.Core;
//using Butterfly.Messages;
//using Database_Manager.Database.Session_Details.Interfaces;
//using System.Text;
//using Butterfly.HabboHotel.GameClients;
//using System.Threading;
//using Butterfly.IRC.Messages;
//using Butterfly.HabboHotel.Pets;
//using System.Collections;

//namespace Butterfly.IRC
//{
//    static class CommandHandler
//    {
//        internal static List<IServerMessage> InvokeCommand(string inputData, User user)
//        {
//            List<IServerMessage> replies = new List<IServerMessage>();

//            try
//            {
//                if (inputData.StartsWith("!") || inputData.StartsWith("@"))
//                {
//                    if (user.rank > 1 && inputData.StartsWith("@"))
//                    {
//                        inputData = inputData.Substring(1);
//                        #region Command parsing for rank > 1
//                        string[] parameters = inputData.Split(' ');

//                        switch (parameters[0])
//                        {
//                            case "shutdown":
//                                {

//                                    Logging.LogMessage("Server exiting via IRC at " + DateTime.Now);
//                                    Logging.DisablePrimaryWriting(true);
//                                    replies.Add(new PrivateMessage("The server is saving users furniture, rooms, etc. WAIT FOR THE SERVER TO CLOSE, DO NOT EXIT THE PROCESS IN TASK MANAGER!!"));

//                                    ButterflyEnvironment.PreformShutDown(true);
//                                    break;
//                                }

//                            case "flush":
//                                {
//                                    if (parameters.Length < 2)
//                                        replies.Add(new PrivateMessage("You need to specify a parameter within your command. Type help for more information"));
//                                    else
//                                    {
//                                        switch (parameters[1])
//                                        {
//                                            case "settings":
//                                                {
//                                                    if (parameters.Length < 3)
//                                                        replies.Add(new PrivateMessage("You need to specify a parameter within your command. Type help for more information"));
//                                                    else
//                                                    {
//                                                        switch (parameters[2])
//                                                        {
//                                                            case "catalog":
//                                                                {
//                                                                    replies.Add(new PrivateMessage("Flushing catalog settings"));

//                                                                    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
//                                                                    {
//                                                                        getGame().GetCatalog().Initialize(dbClient);
//                                                                    }
//                                                                    getGame().GetCatalog().InitCache();
//                                                                    getGame().GetClientManager().QueueBroadcaseMessage(new ServerMessage(441));

//                                                                    replies.Add(new PrivateMessage("Catalog flushed"));

//                                                                    break;
//                                                                }

//                                                            //case "config":
//                                                            //    {
//                                                            //        Console.WriteLine("Flushing configuration");


//                                                            //        break;
//                                                            //    }

//                                                            case "modeldata":
//                                                                {
//                                                                    replies.Add(new PrivateMessage("Flushing modeldata"));
//                                                                    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
//                                                                    {
//                                                                        getGame().GetRoomManager().LoadModels(dbClient);
//                                                                        getGame().GetRoomManager().InitRoomLinks(dbClient);
//                                                                    }
//                                                                    replies.Add(new PrivateMessage("Models flushed"));

//                                                                    break;
//                                                                }

//                                                            case "bans":
//                                                                {
//                                                                    replies.Add(new PrivateMessage("Flushing bans"));
//                                                                    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
//                                                                    {
//                                                                        getGame().GetBanManager().LoadBans(dbClient);
//                                                                    }
//                                                                    replies.Add(new PrivateMessage("Bans flushed"));

//                                                                    break;
//                                                                }

//                                                            case "commands":
//                                                                {
//                                                                    replies.Add(new PrivateMessage("Flushing commands"));
//                                                                    ChatCommandRegister.Init();
//                                                                    PetCommandHandeler.Init();
//                                                                    PetLocale.Init();
//                                                                    replies.Add(new PrivateMessage("Commands flushed"));

//                                                                    break;
//                                                                }

//                                                            case "language":
//                                                                {
//                                                                    replies.Add(new PrivateMessage("Flushing language files"));
//                                                                    LanguageLocale.Init();
//                                                                    replies.Add(new PrivateMessage("Language files flushed"));

//                                                                    break;
//                                                                }
//                                                        }
//                                                    }
//                                                    break;
//                                                }

//                                            //case "users":
//                                            //    {
//                                            //        replies.Add(new PrivateMessage("Flushing users..."));
//                                            //        replies.Add(new PrivateMessage(getGame().GetClientManager().flushUsers() + " users flushed"));
//                                            //        break;
//                                            //    }

//                                            //case "connections":
//                                            //    {
//                                            //        replies.Add(new PrivateMessage("Flushing connections..."));
//                                            //        replies.Add(new PrivateMessage(getGame().GetClientManager().flushConnections() + " connections flushed"));
//                                            //        break;
//                                            //    }

//                                            case "ddosprotection":
//                                                {
//                                                    replies.Add(new PrivateMessage("Flushing anti-ddos..."));
//                                                    TcpAuthorization.Flush();
//                                                    replies.Add(new PrivateMessage("Anti-ddos flushed"));

//                                                    break;
//                                                }

//                                            case "toilet":
//                                                {
//                                                    replies.Add(new PrivateMessage("Flushing toilet..."));
//                                                    replies.Add(new PrivateMessage("*SPLOUSH*"));
//                                                    replies.Add(new PrivateMessage("Toilet flushed"));
//                                                    break;
//                                                }

//                                            default:
//                                                {
//                                                    replies.Add(unknownCommand(inputData));
//                                                    break;
//                                                }
//                                        }
//                                    }

//                                    break;
//                                }

//                            case "view":
//                                {
//                                    if (parameters.Length < 2)
//                                        replies.Add(new PrivateMessage("You need to specify a parameter within your command. Type help for more information"));
//                                    else
//                                    {
//                                        switch (parameters[1])
//                                        {
//                                            case "connections":
//                                            case "users":
//                                                {
//                                                                    replies.Add(new PrivateMessage("Connection count: " + getGame().GetClientManager().connectionCount));
//                                                                    break;
                                              
//                                                }

//                                            case "rooms":
//                                                {
                                     
//                                                                    replies.Add(new PrivateMessage("Loaded room count: " + getGame().GetRoomManager().LoadedRoomsCount));
//                                                                    break;
//                                                }

//                                            case "dbconnections":
//                                                {
//                                                    Console.WriteLine("Database connection: " + ButterflyEnvironment.GetDatabaseManager().getOpenConnectionCount());
//                                                    break;
//                                                }

//                                            default:
//                                                {
//                                                    replies.Add(unknownCommand(inputData));
//                                                    break;
//                                                }
//                                        }

//                                    }
//                                    break;
//                                }

//                            case "alert":
//                                {
//                                    string Notice = inputData.Substring(6);

//                                    ServerMessage HotelAlert = new ServerMessage(810);
//                                    HotelAlert.AppendUInt(1);
//                                    HotelAlert.AppendStringWithBreak(LanguageLocale.GetValue("console.noticefromadmin") +
//                                    Notice);
//                                    getGame().GetClientManager().QueueBroadcaseMessage(HotelAlert);
//                                    replies.Add(new PrivateMessage("[" + Notice + "] sent"));


//                                    if (ButterflyEnvironment.IrcEnabled)
//                                        ButterflyEnvironment.messagingBot.SendMassMessage(new PublicMessage(string.Format("[{0}] => [{1}]", user.ircuser, Notice)), true);

//                                    break;
//                                }

//                            case "ddos":
//                            case "setddosprotection":
//                                {
//                                    if (parameters.Length < 2)
//                                        replies.Add(new PrivateMessage("You need to specify a parameter within your command. Type help for more information"));
//                                    else
//                                    {
//                                        TcpAuthorization.Enabled = (parameters[1] == "true");
//                                        if (TcpAuthorization.Enabled)
//                                            replies.Add(new PrivateMessage("DDOS protection enabled"));
//                                        else
//                                            replies.Add(new PrivateMessage("DDOS protection disabled"));
//                                    }

//                                    break;
//                                }

//                            case "version":
//                                {
//                                    replies.Add(new PrivateMessage(ButterflyEnvironment.PrettyVersion));
//                                    break;
//                                }


//                            case "help":
//                                {
//                                    replies.Add(new PrivateMessage("..::**Øø.     Help Commands List     .øØ**::.."));
//                                    replies.Add(new PrivateMessage("[] = Required                 () = Not Required"));
//                                    replies.Add(new PrivateMessage("[--=Command Syntax=--]         - [==What It Does==]"));
//                                    replies.Add(new PrivateMessage("alert [message]                - Sends HostelAlert To Server"));
//                                    replies.Add(new PrivateMessage("ban [user] [days]              - Bans Users for X# Days from Server"));
//                                    replies.Add(new PrivateMessage("credits [user] [amnt]          - Gives User Credits"));
//                                    replies.Add(new PrivateMessage("ddos [true/false]              - Toggles (soft)DDoS Protection"));
//                                    replies.Add(new PrivateMessage("flush cache                    - Clears Cache"));
//                                    replies.Add(new PrivateMessage("flush connections              - Disconnects Everyone"));
//                                    replies.Add(new PrivateMessage("flush console                  - Clears Console [/unseen in IRC]"));
//                                    replies.Add(new PrivateMessage("flush ddosprotection           - Unban DDoS'ers"));
//                                    replies.Add(new PrivateMessage("flush rooms                    - Unloads ALL Rooms"));
//                                    replies.Add(new PrivateMessage("flush settings catalog         - Reload Catalog"));
//                                    replies.Add(new PrivateMessage("flush settings modeldata       - Reload Modeldata"));
//                                    replies.Add(new PrivateMessage("flush settings bans            - Flushes Bans"));
//                                    replies.Add(new PrivateMessage("flush settings commands        - Flushes Commands"));
//                                    replies.Add(new PrivateMessage("flush settings language        - Flushes Language Files"));
//                                    replies.Add(new PrivateMessage("flush toilet                   - [/unused] Demonstration Purposes Only"));
//                                    replies.Add(new PrivateMessage("flush users                    - Disconnects Unset Users"));
//                                    replies.Add(new PrivateMessage("help                           - Shows Commands $this-]"));
//                                    replies.Add(new PrivateMessage("kick [user] (reason)           - Kicks User from Server"));
//                                    replies.Add(new PrivateMessage("linkalert [link] (message)     - Sends HotelAlert w/ Link"));
//                                    replies.Add(new PrivateMessage("mordi [username]               - Sets User Mordi"));
//                                    replies.Add(new PrivateMessage("pixels [user] [amnt]           - Gives User Pixels"));
//                                    replies.Add(new PrivateMessage("powerlevels                    - Displays PowerLevels"));
//                                    replies.Add(new PrivateMessage("reload [roomid]                - Reloads Room"));
//                                    replies.Add(new PrivateMessage("shutdown                       - Closes Server Safely"));
//                                    replies.Add(new PrivateMessage("stats                          - Shows Users Online, Rooms Loaded, Uptime, & Server Version"));
//                                    replies.Add(new PrivateMessage("staffonline                    - Displays Staff Online"));
//                                    replies.Add(new PrivateMessage("setmax [roomid] [max]          - Sets Max # Users Alloucated Room"));
//                                    replies.Add(new PrivateMessage("roomalert [id] [msg]           - Send Room Alert"));
//                                    replies.Add(new PrivateMessage("roomselect [id]                - Eavesdrop on Room"));
//                                    replies.Add(new PrivateMessage("roomdeselect [id]              - Stop Eavesdropping on Room"));
//                                    replies.Add(new PrivateMessage("useralert [username] [msg]     - Sends Alert To Specific User"));
//                                    replies.Add(new PrivateMessage("unban [user]                   - Unbans User from Server"));
//                                    replies.Add(new PrivateMessage("view users                     - List Connected user's [Name]"));
//                                    replies.Add(new PrivateMessage("view rooms                     - View Loaded Rooms"));
//                                    replies.Add(new PrivateMessage("version                        - Displays Server Version"));
//                                    replies.Add(new PrivateMessage("view connections               - List Connected User's [IP:PORT]"));
//                                    replies.Add(new PrivateMessage("wheres [username]              - Finds Username"));
//                                    replies.Add(new PrivateMessage("talk [username] [message]      - Sends a shout message in-game"));
//                                    replies.Add(new PrivateMessage("join [username] [roomid]       - Opens room for the user entered"));
//                                    break;
//                                }

//                            case "useralert:":
//                                {
//                                    string TargetUser = parameters[1];
//                                    GameClient TargetClient = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(TargetUser);

//                                    if (TargetClient == null)
//                                    {
//                                        replies.Add(new PrivateMessage("User not found or user is offline"));
//                                        break;
//                                    }

//                                    TargetClient.SendNotif(MergeParams(parameters, 2), true);
//                                    break;
//                                }

//                            case "roomselect":
//                                {
//                                    uint roomID = uint.Parse(parameters[1]);
//                                    Room roomLookup = getGame().GetRoomManager().GetRoom(roomID);
//                                    if (roomLookup != null)
//                                    {
//                                        roomLookup.ircUsernames.Add(user.ircuser);
//                                        replies.Add(new PrivateMessage("Selected rooom " + roomID));
//                                    }
//                                    else
//                                    {
//                                        replies.Add(new PrivateMessage("Room is not loaded or does not exist"));
//                                    }

//                                    break;
//                                }

//                            case "roomdeselect":
//                                {
//                                    uint roomID = uint.Parse(parameters[1]);
//                                    Room roomLookup = getGame().GetRoomManager().GetRoom(roomID);
//                                    if (roomLookup != null)
//                                    {
//                                        roomLookup.ircUsernames.Remove(user.ircuser);
//                                        replies.Add(new PrivateMessage("Deselected rooom " + roomID));
//                                    }
//                                    else
//                                    {
//                                        replies.Add(new PrivateMessage("Room is not loaded or does not exist"));
//                                    }

//                                    break;
//                                }
//                            case "roomalert":
//                                {
//                                    uint roomID = uint.Parse(parameters[1]);
//                                    Room roomLookup = getGame().GetRoomManager().GetRoom(roomID);
//                                    string message = MergeParams(parameters, 2);
//                                    if (roomLookup != null)
//                                    {

//                                        ServerMessage nMessage = new ServerMessage();
//                                        nMessage.Init(161);
//                                        nMessage.AppendStringWithBreak(message);

//                                        roomLookup.QueueRoomMessage(nMessage);

//                                        replies.Add(new PrivateMessage("Alert [" + message + "] Sent To Room"));
//                                    }
//                                    else
//                                    {
//                                        replies.Add(new PrivateMessage("Room is not loaded or does not exist"));
//                                    }
//                                    break;
//                                }

//                            case "setmax":
//                                {
//                                    uint roomID = uint.Parse(parameters[1]);
//                                    Room roomLookup = getGame().GetRoomManager().GetRoom(roomID);
//                                    int maxnum = int.Parse(parameters[2]);

//                                    if (roomLookup != null)
//                                    {
//                                        roomLookup.SetMaxUsers(maxnum);
//                                        replies.Add(new PrivateMessage("Maximum allowed users has been set to " + maxnum + " for room " + roomID));
//                                    }
//                                    else
//                                    {
//                                        replies.Add(new PrivateMessage("Room is not loaded or does not exist"));
//                                    }
//                                    break;
//                                }

//                            case "credits":
//                                {
//                                    GameClient TargetClient = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(parameters[1]);
//                                    if (TargetClient != null)
//                                    {
//                                        int creditsToAdd;
//                                        if (int.TryParse(parameters[2], out creditsToAdd))
//                                        {
//                                            TargetClient.GetHabbo().Credits = TargetClient.GetHabbo().Credits + creditsToAdd;
//                                            TargetClient.GetHabbo().UpdateCreditsBalance();
//                                            TargetClient.SendNotif(user.ircuser + "@IRC " + LanguageLocale.GetValue("coins.awardmessage1") + creditsToAdd.ToString() + LanguageLocale.GetValue("coins.awardmessage2"));
//                                            replies.Add(new PrivateMessage("Coins given"));
//                                        }
//                                        else
//                                        {
//                                            replies.Add(new PrivateMessage("Int only!"));
//                                        }
//                                    }
//                                    else
//                                    {
//                                        replies.Add(new PrivateMessage("No user found"));
//                                    }
//                                    break;
//                                }
//                            case "pixels":
//                                {
//                                    GameClient TargetClient = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(parameters[1]);
//                                    if (TargetClient != null)
//                                    {
//                                        int creditsToAdd;
//                                        if (int.TryParse(parameters[2], out creditsToAdd))
//                                        {
//                                            TargetClient.GetHabbo().ActivityPoints = TargetClient.GetHabbo().ActivityPoints + creditsToAdd;
//                                            TargetClient.GetHabbo().UpdateActivityPointsBalance(true);
//                                            TargetClient.SendNotif(user.ircuser + "@IRC" + LanguageLocale.GetValue("pixels.awardmessage1") + creditsToAdd.ToString() + LanguageLocale.GetValue("pixels.awardmessage2"));

//                                            replies.Add(new PrivateMessage("Pixels given"));
//                                        }
//                                        else
//                                        {
//                                            replies.Add(new PrivateMessage("Int only!"));
//                                        }
//                                    }
//                                    else
//                                    {
//                                        replies.Add(new PrivateMessage("No user found"));
//                                    }
//                                    break;
//                                }
//                            case "mordi":
//                                {
//                                    GameClient TargetClient = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(parameters[1]);
//                                    if (TargetClient != null)
//                                    {
//                                        TargetClient.GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(60);
//                                    }
//                                    break;
//                                }
//                            case "wheres":
//                                {
//                                    GameClient TargetClient = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(parameters[1]);
//                                    if (TargetClient != null)
//                                    {
//                                        if (TargetClient.GetHabbo().CurrentRoom != null)
//                                        {
//                                            replies.Add(new PrivateMessage(string.Format("{0} is in [{1}] owned by [{2}] roomID {3}", parameters[1], TargetClient.GetHabbo().CurrentRoom.Name, TargetClient.GetHabbo().CurrentRoom.Owner, TargetClient.GetHabbo().CurrentRoom.RoomId)));
//                                        }
//                                        else
//                                        {
//                                            replies.Add(new PrivateMessage("Hotelview"));
//                                        }
//                                    }
//                                    else
//                                    {
//                                        replies.Add(new PrivateMessage("No user found"));
//                                    }

//                                    break;
//                                }
//                            case "stats":
//                                {
//                                    TimeSpan timeUsed = DateTime.Now - ButterflyEnvironment.ServerStarted;
//                                    replies.Add(new PrivateMessage("Users online: " + ButterflyEnvironment.GetGame().GetClientManager().ClientCount));
//                                    replies.Add(new PrivateMessage("Rooms loaded: " + ButterflyEnvironment.GetGame().GetClientManager().ClientCount));
//                                    replies.Add(new PrivateMessage("Server uptime: " + timeUsed.Days + " day(s), " + timeUsed.Hours + " hour(s) and " + timeUsed.Minutes + " minute(s)"));
//                                    replies.Add(new PrivateMessage("Server version string: " + ButterflyEnvironment.PrettyVersion));
//                                    break;
//                                }
//                            //case "staffonline":
//                            //    {
//                            //        //replies.Add(new PrivateMessage("Staffs online:"));
//                            //        //foreach (GameClient client in getGame().GetClientManager().GetGameClientsByRank(2))
//                            //        //{
//                            //        //    replies.Add(new PrivateMessage(string.Format("[{0}] [{1}]", client.GetHabbo().Username, client.GetHabbo().Rank)));
//                            //        //}
//                            //        break;
//                            //    }

//                            case "ban":
//                                {
//                                    GameClient TargetClient = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(parameters[1]);

//                                    if (TargetClient == null)
//                                    {
//                                        replies.Add(new PrivateMessage("User not found or is not online"));
//                                        break;
//                                    }

//                                    int BanTime = 0;

//                                    try
//                                    {
//                                        BanTime = int.Parse(parameters[2]);
//                                    }
//                                    catch (FormatException)
//                                    {
//                                        replies.Add(new PrivateMessage("Unable to parse parameter day"));
//                                        break;
//                                    }
//                                    ButterflyEnvironment.GetGame().GetBanManager().BanUser(TargetClient, user.ircuser + "@IRC", BanTime * 230400, MergeParams(parameters, 3), false);

//                                    break;
//                                }

//                            case "kick":
//                                {
//                                    string TargetUser = parameters[1];
//                                    GameClient TargetClient = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(TargetUser);

//                                    if (TargetClient == null)
//                                    {
//                                        replies.Add(new PrivateMessage("User not found or is not online"));
//                                        break;
//                                    }

//                                    if (TargetClient.GetHabbo().CurrentRoomId < 1)
//                                    {
//                                        replies.Add(new PrivateMessage("User is not in a room"));
//                                        break;
//                                    }

//                                    Room TargetRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(TargetClient.GetHabbo().CurrentRoomId);

//                                    if (TargetRoom == null)
//                                    {
//                                        replies.Add(new PrivateMessage("User is not in a room"));
//                                        break;
//                                    }

//                                    TargetRoom.GetRoomUserManager().RemoveUserFromRoom(TargetClient, true, false);

//                                    if (parameters.Length > 2)
//                                    {
//                                        TargetClient.SendNotif(LanguageLocale.GetValue("kick.withmessage") + MergeParams(parameters, 2));
//                                    }
//                                    else
//                                    {
//                                        TargetClient.SendNotif(LanguageLocale.GetValue("kick.nomessage"));
//                                    }

//                                    break;
//                                }

//                            case "unban":
//                                {
//                                    string ipOrUsername = parameters[1];
//                                    getGame().GetBanManager().UnbanUser(ipOrUsername);

//                                    break;
//                                }

//                            case "linkalert":
//                                {
//                                    string Link = parameters[1];
//                                    string Message = MergeParams(parameters, 2);

//                                    ServerMessage nMessage = new ServerMessage(161);
//                                    nMessage.AppendStringWithBreak(LanguageLocale.GetValue("hotelallert.notice") + "\r\n" + Message + "\r\n- " + user.ircuser + "@IRC");
//                                    nMessage.AppendStringWithBreak(Link);
//                                    ButterflyEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(nMessage);

//                                    ButterflyEnvironment.messagingBot.SendMassMessage(new PublicMessage(string.Format("[{0}] => [{1}] + [{2}]", user.ircuser, Link, Message)), true);

//                                    break;
//                                }

//                            case "reload":
//                                {
//                                    uint roomID = uint.Parse(parameters[1]);
//                                    Room roomToUnload = getGame().GetRoomManager().GetRoom(roomID);

//                                    if (roomToUnload != null)
//                                    {
//                                        roomToUnload.SendLeaveMessageForBots();
//                                        List<RoomUser> users = roomToUnload.GetRoomUserManager().GetRoomUsers();

//                                        Hashtable usersByUsername = roomToUnload.GetRoomUserManager().usersByUsername.Clone() as Hashtable;
//                                        Hashtable usersByUserID = roomToUnload.GetRoomUserManager().usersByUsername.Clone() as Hashtable;

//                                        int primaryCounter = 0;
//                                        int secondaryCounter = 0;
//                                        roomToUnload.GetRoomUserManager().backupCounters(ref primaryCounter, ref secondaryCounter);

//                                        roomToUnload.FlushSettings();
//                                        roomToUnload.ReloadSettings();
//                                        roomToUnload.GetRoomUserManager().UpdateUserStats(users, usersByUsername, usersByUserID, primaryCounter, secondaryCounter);
//                                        roomToUnload.UpdateFurniture();
//                                        roomToUnload.GetGameMap().GenerateMaps();
//                                    }
//                                    else
//                                    {
//                                        replies.Add(new PrivateMessage("Room is not loaded or does not exist"));
//                                    }

//                                    break;
//                                }

//                            case "powerlevels":
//                                {
//                                    replies.Add(new PrivateMessage("Powerlevel of our server is " + ButterflyEnvironment.GetRandomNumber(9001, 10000) + " (Over FUCKING 9000) GEE-ZUS!"));
//                                    break;
//                                }

//                            case "speak":
//                            case "speech":
//                            case "talk":
//                                {
//                                    string targetUsername = parameters[1];
//                                    string message = MergeParams(parameters, 2);

//                                    GameClient client = getGame().GetClientManager().GetClientByUsername(targetUsername);
//                                    if (client == null)
//                                    {
//                                        replies.Add(new PrivateMessage("User not found, is not online or does not exist"));
//                                        break;
//                                    }

//                                    Room targetRoom = client.GetHabbo().CurrentRoom;
//                                    if (targetRoom == null)
//                                    {
//                                        replies.Add(new PrivateMessage("User spesified is not in a room"));
//                                        break;
//                                    }

//                                    RoomUser roomUser = targetRoom.GetRoomUserByHabbo(targetUsername);
//                                    if (roomUser == null)
//                                        break;

//                                    roomUser.Chat(client, message + " [@IRC]", true);

//                                    break;
//                                }

//                            case "join":
//                                {
//                                    string targetUsername = parameters[1];
//                                    uint roomID = uint.Parse(parameters[2]);
//                                    GameClient client = getGame().GetClientManager().GetClientByUsername(targetUsername);
//                                    if (client == null)
//                                    {
//                                        replies.Add(new PrivateMessage("User not found"));
//                                        break;
//                                    }

//                                    client.GetMessageHandler().PrepareRoomForUser(roomID, "");

//                                    break;
//                                }

//                            default:
//                                {
//                                    replies.Add(unknownCommand(inputData));
//                                    break;
//                                }


//                        }
//                        #endregion
//                    }
//                    else
//                    {
//                        inputData = inputData.Substring(1);
//                        #region Standard user commands
//                        string[] parameters = inputData.Split(' ');
//                        GameClient Session = getGame().GetClientManager().GetClientByUsername(user.habbouser);
//                        if (Session == null)
//                        {
//                            replies.Add(new PrivateMessage("Your user is not logged on in-game"));
//                            return replies;
//                        }

//                        switch (parameters[0])
//                        {
//                            case "wheres":
//                                {
//                                    GameClient TargetClient = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(parameters[1]);
//                                    if (TargetClient != null)
//                                    {
//                                        if (TargetClient.GetHabbo().CurrentRoom != null)
//                                        {
//                                            replies.Add(new PrivateMessage(string.Format("{0} is in [{1}] owned by [{2}] roomID {3}", parameters[1], TargetClient.GetHabbo().CurrentRoom.Name, TargetClient.GetHabbo().CurrentRoom.Owner, TargetClient.GetHabbo().CurrentRoom.RoomId)));
//                                        }
//                                        else
//                                        {
//                                            replies.Add(new PrivateMessage("Hotelview"));
//                                        }
//                                    }
//                                    else
//                                    {
//                                        replies.Add(new PrivateMessage("No user found"));
//                                    }
//                                    break;
//                                }

//                            case "stalk":
//                                {
//                                    GameClient TargetClient = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(parameters[1]);

//                                    if (TargetClient == null || TargetClient.GetHabbo() == null || TargetClient.GetHabbo().CurrentRoom == null)
//                                        break;

//                                    Session.GetMessageHandler().PrepareRoomForUser(TargetClient.GetHabbo().CurrentRoom.RoomId, "");

//                                    replies.Add(new PrivateMessage("Stalking user"));
//                                    break;
//                                }
//                            case "join":
//                                {
//                                    uint roomID = uint.Parse(parameters[1]);
//                                    Session.GetMessageHandler().PrepareRoomForUser(roomID, "");
//                                    replies.Add(new PrivateMessage("Entering room " + roomID));

//                                    break;
//                                }

//                            case "help":
//                            case "commands":
//                                {
//                                    replies.Add(new PrivateMessage("..::**Øø.____VG Help Commands List____.øØ**::.."));
//                                    replies.Add(new PrivateMessage("[] = Required"));
//                                    replies.Add(new PrivateMessage("() = Not Required"));
//                                    replies.Add(new PrivateMessage("[--=Command Syntax=--]__________-____[==What It Does==]"));
//                                    replies.Add(new PrivateMessage("!commands_______________________-_Place your user into a room"));
//                                    replies.Add(new PrivateMessage("!join [roomid]__________________-_Place your user into a room"));
//                                    replies.Add(new PrivateMessage("!listrooms (top)________________-_List rooms (top x) {will get you roomid}"));
//                                    replies.Add(new PrivateMessage("!logout_________________________-_Logs your user out of the hostel"));
//                                    replies.Add(new PrivateMessage("!mordi__________________________-_Gives FREE staff!"));
//                                    replies.Add(new PrivateMessage("!quit___________________________-_Logs your user out of the hostel"));
//                                    replies.Add(new PrivateMessage("!staffonline____________________-_List rooms (top x) {will get you roomid}"));
//                                    replies.Add(new PrivateMessage("!stalk [username]_______________-_Follow User into room they are in"));
//                                    replies.Add(new PrivateMessage("!stats__________________________-_Shows # Users Online, # Rooms, & Uptime"));
//                                    replies.Add(new PrivateMessage("!wheres [username]______________-_Finds Username"));
//                                    replies.Add(new PrivateMessage("[--Default Command--]"));
//                                    replies.Add(new PrivateMessage("Just type your message and you will talk through your character. (make sure you are in a room) :)"));

//                                    break;
//                                }

//                            case "mordi":
//                                {
//                                    Session.GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(60);
//                                    break;
//                                }

//                            case "quit":
//                            case "logout":
//                                {
//                                    Session.Disconnect();
//                                    replies.Add(new PrivateMessage("Logged out!"));
//                                    break;
//                                }

//                            //case "staffonline":
//                            //    {
//                            //        replies.Add(new PrivateMessage("Staffs online:"));
//                            //        foreach (GameClient client in getGame().GetClientManager().GetGameClientsByRank(2))
//                            //        {
//                            //            replies.Add(new PrivateMessage(string.Format("[{0}] [{1}]", client.GetHabbo().Username, client.GetHabbo().Rank)));
//                            //        }
//                            //        break;
//                            //    }

//                            case "stats":
//                                {
//                                    replies.Add(new PrivateMessage(ButterflyEnvironment.PrettyVersion));
//                                    replies.Add(new PrivateMessage("Users online: " + ButterflyEnvironment.GetGame().GetClientManager().ClientCount));
//                                    replies.Add(new PrivateMessage("Rooms loaded: " + ButterflyEnvironment.GetGame().GetRoomManager().LoadedRoomsCount));
//                                    break;
//                                }
//                        }

//                        #endregion
//                    }
//                }
//                else
//                {
//                    GameClient Session = getGame().GetClientManager().GetClientByUsername(user.habbouser);
//                    if (Session == null)
//                    {
//                        replies.Add(new PrivateMessage("Your user is not logged on in-game."));
//                        return replies;
//                    }

//                    if (Session.GetHabbo() == null || Session.GetHabbo().CurrentRoom == null)
//                    {
//                        replies.Add(new PrivateMessage("Your need to be in a room to send messages. Type !help for help"));
//                        return replies;
//                    }

//                    Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Username).Chat(Session, inputData, true);
//                }
//            }
//            catch (Exception e)
//            {
//                replies.Add(new PrivateMessage("Error in command [" + inputData + "]: " + e.ToString()));
//            }

//            return replies;
//        }

//        internal static string MergeParams(string[] Params, int Start)
//        {
//            StringBuilder MergedParams = new StringBuilder();

//            for (int i = 0; i < Params.Length; i++)
//            {
//                if (i < Start)
//                {
//                    continue;
//                }

//                if (i > Start)
//                {
//                    MergedParams.Append(" ");
//                }

//                MergedParams.Append(Params[i]);
//            }

//            return MergedParams.ToString();
//        }

//        private static PrivateMessage unknownCommand(string command)
//        {
//            return new PrivateMessage(command + " is an unknown or unsupported command. Type help for more information");
//        }

//        internal static Game getGame()
//        {
//            return ButterflyEnvironment.GetGame();
//        }
//    }
//}