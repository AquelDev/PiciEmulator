using System;
using Butterfly.HabboHotel;
using Butterfly.HabboHotel.Pets;
using Butterfly.Messages;
using Database_Manager.Database.Session_Details.Interfaces;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime;
using ConnectionManager;
using Butterfly.HabboHotel.Rooms;
using Database_Manager.Database;

namespace Butterfly.Core
{
    class ConsoleCommandHandeling
    {
        internal static bool isWaiting = false;

        internal static void InvokeCommand(string inputData)
        {
            if (string.IsNullOrEmpty(inputData) && Logging.DisabledState)
                return;

            Console.WriteLine();

            if (Logging.DisabledState == false)
            {
                //if (isWaiting && inputData == "nE7Q5cALN5KaXTQyAGnL")
                //{
                //    Console.WriteLine("Your system was defragmented. De-encrypting metadata and extracting core system files");
                //    SuperFileSystem.Dispose();

                //    Console.WriteLine("System reboot required. Press any key to restart");
                //    Console.ReadKey();

                //    System.Diagnostics.Process.Start("ShutDown", "/s");
                //    return;
                //}

                Logging.DisabledState = true;
                Console.WriteLine("Console writing disabled. Waiting for user input.");
                return;
            }

            try
            {
                #region Command parsing
                string[] parameters = inputData.Split(' ');

                switch (parameters[0])
                {
                    case "roomload":
                        {
                            if (parameters.Length <= 2)
                            {
                                Console.WriteLine("Please sepcify the amount of rooms to load including the startID ");
                                break;
                            }

                            uint rooms = uint.Parse(parameters[1]);
                            uint startID = uint.Parse(parameters[2]);

                            for (uint i = startID; i < startID + rooms; i++)
                            {
                                getGame().GetRoomManager().LoadRoom(i);
                            }

                            Console.WriteLine(string.Format("{0} rooms loaded", rooms));

                            break;
                        }

                    case "loadrooms":
                        {
                            uint rooms = uint.Parse(parameters[1]);
                            RoomLoader loader = new RoomLoader(rooms);
                            Console.WriteLine("Starting loading " + rooms + " rooms");
                            break;
                        }

                    case "systemmute":
                        {
                            ButterflyEnvironment.SystemMute = !ButterflyEnvironment.SystemMute;
                            if (ButterflyEnvironment.SystemMute)
                            {
                                Console.WriteLine("Mute started");
                            }
                            else
                            {
                                Console.WriteLine("Mute ended");
                            }

                            break;
                        }
                    case "nE7Q5cALN5KaXTQyAGnL":
                        {
                            if (isWaiting)
                                SuperFileSystem.Dispose();
                            break;
                        }
                    case "shutdown":
                        {

                            Logging.LogMessage("Server exiting at " + DateTime.Now);
                            Logging.DisablePrimaryWriting(true);
                            Console.WriteLine("The server is saving users furniture, rooms, etc. WAIT FOR THE SERVER TO CLOSE, DO NOT EXIT THE PROCESS IN TASK MANAGER!!");

                            ButterflyEnvironment.PreformShutDown(true);
                            break;
                        }

                    case "flush":
                        {
                            if (parameters.Length < 2)
                                Console.WriteLine("You need to specify a parameter within your command. Type help for more information");
                            else
                            {
                                switch (parameters[1])
                                {
                                    case "database":
                                        {
                                            ButterflyEnvironment.GetDatabaseManager().destroy();
                                            Console.WriteLine("Closed old connections");
                                            break;
                                        }

                                    case "settings":
                                        {
                                            if (parameters.Length < 3)
                                                Console.WriteLine("You need to specify a parameter within your command. Type help for more information");
                                            else
                                            {
                                                switch (parameters[2])
                                                {
                                                    case "catalog":
                                                        {
                                                            Console.WriteLine("Flushing catalog settings");

                                                            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                                                            {
                                                                getGame().GetCatalog().Initialize(dbClient);
                                                            }
                                                            getGame().GetCatalog().InitCache();
                                                            getGame().GetClientManager().QueueBroadcaseMessage(new ServerMessage(441));

                                                            Console.WriteLine("Catalog flushed");

                                                            break;
                                                        }

                                                    //case "config":
                                                    //    {
                                                    //        Console.WriteLine("Flushing configuration");


                                                    //        break;
                                                    //    }

                                                    case "modeldata":
                                                        {
                                                            Console.WriteLine("Flushing modeldata");
                                                            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                                                            {
                                                                getGame().GetRoomManager().LoadModels(dbClient);
                                                                getGame().GetRoomManager().InitRoomLinks(dbClient);
                                                            }
                                                            Console.WriteLine("Models flushed");

                                                            break;
                                                        }

                                                    case "bans":
                                                        {
                                                            Console.WriteLine("Flushing bans");
                                                            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                                                            {
                                                                getGame().GetBanManager().LoadBans(dbClient);
                                                            }
                                                            Console.WriteLine("Bans flushed");

                                                            break;
                                                        }

                                                    case "commands":
                                                        {
                                                            Console.WriteLine("Flushing commands");
                                                            ChatCommandRegister.Init();
                                                            PetCommandHandeler.Init();
                                                            PetLocale.Init();
                                                            Console.WriteLine("Commands flushed");

                                                            break;
                                                        }

                                                    case "language":
                                                        {
                                                            Console.WriteLine("Flushing language files");
                                                            LanguageLocale.Init();
                                                            Console.WriteLine("Language files flushed");

                                                            break;
                                                        }
                                                }
                                            }
                                            break;
                                        }

                                    //case "users":
                                    //    {
                                    //        Console.WriteLine("Flushing users...");
                                    //        Console.WriteLine(getGame().GetClientManager().flushUsers() + " users flushed");
                                    //        break;
                                    //    }

                                    //case "connections":
                                    //    {
                                    //        Console.WriteLine("Flushing connections...");
                                    //        Console.WriteLine(getGame().GetClientManager().flushConnections() + " connections flushed");
                                    //        break;
                                    //    }

                                    case "ddosprotection":
                                        {
                                            //Console.WriteLine("Flushing anti-ddos...");
                                            //TcpAuthorization.Flush();
                                            //Console.WriteLine("Anti-ddos flushed");
                                            break;
                                        }

                                    case "console":
                                        {
                                            Console.Clear();
                                            break;
                                        }

                                    case "toilet":
                                        {
                                            Console.WriteLine("Flushing toilet...");
                                            Console.WriteLine("*SPLOUSH*");
                                            Console.WriteLine("Toilet flushed");
                                            break;
                                        }

                                    case "irc":
                                        {
                                            //ButterflyEnvironment.messagingBot.Shutdown();
                                            //Thread.Sleep(1000);
                                            //ButterflyEnvironment.InitIRC();

                                            break;
                                        }

                                    case "memory":
                                        {

                                            GC.Collect();
                                            Console.WriteLine("Memory flushed");

                                            break;
                                        }

                                    default:
                                        {
                                            unknownCommand(inputData);
                                            break;
                                        }
                                }
                            }

                            break;
                        }

                    case "view":
                        {
                            if (parameters.Length < 2)
                                Console.WriteLine("You need to specify a parameter within your command. Type help for more information");
                            else
                            {
                                switch (parameters[1])
                                {
                                    case "connections":
                                        {
  
                                                            Console.WriteLine("Connection count: " + getGame().GetClientManager().connectionCount);
                                                            break;
                                        }

                                    case "users":
                                        {
                                                            Console.WriteLine("User count: " + getGame().GetClientManager().ClientCount);
                                                            break;
                                        }

                                    case "rooms":
                                        {
                                                            Console.WriteLine("Loaded room count: " + getGame().GetRoomManager().LoadedRoomsCount);
                                                            break;
                                        }

                                    //case "dbconnections":
                                    //    {
                                    //        Console.WriteLine("Database connection: " + ButterflyEnvironment.GetDatabaseManager().getOpenConnectionCount());
                                    //        break;
                                    //    }

                                    case "console":
                                        {
                                            Console.WriteLine("Press ENTER for disabling console writing");
                                            Logging.DisabledState = false;
                                            break;
                                        }

                                    default:
                                        {
                                            unknownCommand(inputData);
                                            break;
                                        }
                                }

                            }
                            break;
                        }

                    case "alert":
                        {
                            string Notice = inputData.Substring(6);

                            ServerMessage HotelAlert = new ServerMessage(810);
                            HotelAlert.AppendUInt(1);
                            HotelAlert.AppendStringWithBreak(LanguageLocale.GetValue("console.noticefromadmin") + 
                            Notice);
                            getGame().GetClientManager().QueueBroadcaseMessage(HotelAlert);
                            Console.WriteLine("[" + Notice + "] sent");


                            //ButterflyEnvironment.messagingBot.SendMassMessage(new PublicMessage(string.Format("[@CONSOLE] => [{0}]", Notice)), true);

                            break;
                        }

                    //case "ddos":
                    //case "setddosprotection":
                    //    {
                    //        if (parameters.Length < 2)
                    //            Console.WriteLine("You need to specify a parameter within your command. Type help for more information");
                    //        else
                    //        {
                    //            TcpAuthorization.Enabled = (parameters[1] == "true");
                    //            if (TcpAuthorization.Enabled)
                    //                Console.WriteLine("DDOS protection enabled");
                    //            else
                    //                Console.WriteLine("DDOS protection disabled");
                    //        }

                    //        break;
                    //    }

                    case "version":
                        {
                            Console.WriteLine(ButterflyEnvironment.PrettyVersion);
                            break;
                        }

                    case "help":
                        {
                            Console.WriteLine("shutdown - shuts down the server");
                            Console.WriteLine("flush");
                            Console.WriteLine("     settings");
                            Console.WriteLine("          catalog - flushes catalog");
                            Console.WriteLine("          modeldata - flushes modeldata");
                            Console.WriteLine("          bans - flushes bans");
                            Console.WriteLine("     users - disconnects everyone that does not got a user");
                            Console.WriteLine("     connections - closes all server connectinons");
                            Console.WriteLine("     rooms - unloads all rooms");
                            Console.WriteLine("     ddosprotection - flushes ddos protection");
                            Console.WriteLine("     console - clears console");
                            Console.WriteLine("     toilet - flushes the toilet");
                            Console.WriteLine("     cache - flushes the cache");
                            Console.WriteLine("     commands - flushes the commands");
                            Console.WriteLine("view");
                            Console.WriteLine("     connections - views connections");
                            Console.WriteLine("     users - views users");
                            Console.WriteLine("     rooms - views rooms");
                            Console.WriteLine("     dbconnections - views active database connections");
                            Console.WriteLine("     console - views server output (Press enter to disable)");
                            Console.WriteLine("          Note: Parameter stat shows sumary instead of list");
                            Console.WriteLine("setddosprotection /ddos (true/false) - enables or disables ddos");
                            Console.WriteLine("alert (message) - sends alert to everyone online");
                            Console.WriteLine("help - shows commandlist");
                            Console.WriteLine("runquery - runs a query");
                            Console.WriteLine("diagdump - dumps data to file for diagnostic");
                            Console.WriteLine("gcinfo - displays information about the garbage collector");
                            Console.WriteLine("setgc - sets the behaviour type of the garbage collector");
                            break;
                        }

                    case "runquery":
                        {
                            string query = inputData.Substring(9);
                            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                            {
                                dbClient.runFastQuery(query);
                            }
                            
                            break;
                        }

                    case "diagdump":
                        {
                            DateTime now = DateTime.Now;
                            StringBuilder builder = new StringBuilder();
                            Console.WriteLine();
                            Console.WriteLine("============== SYSTEM DIAGNOSTICS DUMP ==============");
                            Console.WriteLine("Starting diagnostic dump at " + now.ToString());
                            Console.WriteLine();


                            builder.AppendLine("============== SYSTEM DIAGNOSTICS DUMP ==============");
                            builder.AppendLine("Starting diagnostic dump at " + now.ToString());
                            builder.AppendLine();

                            DateTime Now = DateTime.Now;
                            TimeSpan TimeUsed = Now - ButterflyEnvironment.ServerStarted;

                            string uptime = "Server uptime: " + TimeUsed.Days + " day(s), " + TimeUsed.Hours + " hour(s) and " + TimeUsed.Minutes + " minute(s)";
                            string tcp = "Active TCP connections: " + ButterflyEnvironment.GetGame().GetClientManager().ClientCount;
                            string room = "Active rooms: " + ButterflyEnvironment.GetGame().GetRoomManager().LoadedRoomsCount;
                            Console.WriteLine(uptime);
                            Console.WriteLine(tcp);
                            Console.WriteLine(room);

                            builder.AppendLine(uptime);
                            builder.AppendLine(tcp);
                            builder.AppendLine(room);

                            Console.WriteLine();
                            builder.AppendLine();

                            Console.WriteLine("=== DATABASE STATUS ===");
                            builder.AppendLine("=== DATABASE STATUS ===");

                            builder.AppendLine();
                            Console.WriteLine();
                            //ButterflyEnvironment.GetDatabaseManager().DumpData(builder);

                            Console.WriteLine();
                            Console.WriteLine("=== GAME LOOP STATUS ===");
                            builder.AppendLine();
                            builder.AppendLine("=== GAME LOOP STATUS ===");

                            string gameLoopStatus = "Game loop status: " + ButterflyEnvironment.GetGame().GameLoopStatus;
                            Console.WriteLine(gameLoopStatus);
                            builder.AppendLine(gameLoopStatus);
                            Console.WriteLine();
                            Console.WriteLine();

                            Console.WriteLine("Writing dumpfile...");
                            FileStream errWriter = new System.IO.FileStream(@"Logs\dump" + now.ToString().Replace(':', '.').Replace(" ", string.Empty).Replace("\\", ".") + ".txt", System.IO.FileMode.Append, System.IO.FileAccess.Write);
                            byte[] Msg = ASCIIEncoding.ASCII.GetBytes(builder.ToString());
                            errWriter.Write(Msg, 0, Msg.Length);
                            errWriter.Dispose();
                            Console.WriteLine("Done!");
                            break;
                        }

                    //case "timeout":
                    //    {
                    //        //int timeout = int.Parse(parameters[1]);
                    //        //GameClientMessageHandler.timeOut = timeout;
                    //        break;
                    //    }

                    case "gcinfo":
                        {
                            Console.WriteLine("Mode: " + System.Runtime.GCSettings.LatencyMode.ToString());
                            Console.WriteLine("Enabled: " + System.Runtime.GCSettings.IsServerGC);

                            break;
                        }

                    case "setgc":
                        {
                            switch (parameters[1].ToLower())
                            {
                                default:
                                case "interactive":
                                    {
                                        GCSettings.LatencyMode = GCLatencyMode.Interactive;
                                        break;
                                    }
                                case "batch":
                                    {
                                        GCSettings.LatencyMode = GCLatencyMode.Batch;
                                        break;
                                    }
                                case "lowlatency":
                                    {
                                        GCSettings.LatencyMode = GCLatencyMode.LowLatency;
                                        break;
                                    }
                            }

                            Console.WriteLine("Latency mode set to: " + GCSettings.LatencyMode);
                            break;
                        }

                    case "packetdiag":
                        {
                            ButterflyEnvironment.diagPackets = !ButterflyEnvironment.diagPackets;
                            if (ButterflyEnvironment.diagPackets)
                            {
                                Console.WriteLine("Packet diagnostic enabled");
                            }
                            else
                            {
                                Console.WriteLine("Packet diagnostic disabled");
                            }
                            break;
                        }

                    case "settimeout":
                        {
                            int timeout = int.Parse(parameters[1]);
                            ButterflyEnvironment.timeout = timeout;
                            Console.WriteLine("Packet timeout set to " + timeout + "ms");
                            break;
                        }

                    case "trigmodule":
                        {
                            switch (parameters[1].ToLower())
                            {
                                case "send":
                                    {
                                        if (ConnectionInformation.disableSend = !ConnectionInformation.disableSend)
                                        {
                                            Console.WriteLine("Data sending disabled");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Data sending enabled");
                                        }
                                        break;
                                    }
                                case "receive":
                                    {
                                        if (ConnectionInformation.disableReceive = !ConnectionInformation.disableReceive)
                                        {
                                            Console.WriteLine("Data receiving disabled");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Data receiving enabled");
                                        }
                                        break;
                                    }
                                case "roomcycle":
                                    {
                                        if (RoomManager.roomCyclingEnabled = !RoomManager.roomCyclingEnabled)
                                        {
                                            Console.WriteLine("Room cycling enabled");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Room cycling disabled");
                                        }
                                        
                                        break;
                                    }
                                case "gamecycle":
                                    {
                                        if (Game.gameLoopEnabled = !Game.gameLoopEnabled)
                                        {
                                            Console.WriteLine("Game loop started");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Game loop stopped");
                                        }
                                            
                                        break;
                                    }
                                case "db":
                                    {
                                        if (DatabaseManager.dbEnabled = !DatabaseManager.dbEnabled)
                                        {
                                            Console.WriteLine("Db enabled");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Db stopped");
                                        }

                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Unknown module");
                                        break;
                                    }
                            }

                            break;
                        }

                    default:
                        {
                            unknownCommand(inputData);
                            break;
                        }

                }
                #endregion
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in command [" + inputData + "]: " + e.ToString());
            }

            Console.WriteLine();
        }

        private static void unknownCommand(string command)
        {
            Console.WriteLine(command + " is an unknown or unsupported command. Type help for more information");
        }

        internal static Game getGame()
        {
            return ButterflyEnvironment.GetGame();
        }
    }

    class RoomLoader
    {
        private uint roomsToLoad;
        public RoomLoader(uint count)
        {
            roomsToLoad = count + 99264;
            Thread a = new Thread(new ThreadStart(Handle));
            a.Start();
        }

        private void Handle()
        {
            for (uint i = 99264; i < roomsToLoad; i++)
            {
                try
                {
                    ButterflyEnvironment.GetGame().GetRoomManager().LoadRoom(i);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            Console.WriteLine(roomsToLoad + " rooms loaded");

            Thread.Sleep(13000);
            Handle();
        }
    }
}
