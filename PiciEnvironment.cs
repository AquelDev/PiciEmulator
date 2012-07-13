using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime;
using Pici.Core;
using Pici.HabboHotel;
using Pici.HabboHotel.Pets;
using Pici.Messages;
using Pici.Storage.Database;
using Pici.Storage.Database.Session_Details.Interfaces;
using Pici.Messages.StaticMessageHandlers;
using Pici.Messages.ClientMessages;
using Pici.Net;
using System.Globalization;
using System.Net;

namespace Pici
{
    static class PiciEnvironment
    {
        private static ConfigurationData Configuration;
        private static Encoding DefaultEncoding;
        private static ConnectionHandeling ConnectionManager;
        private static Game Game;
        internal static DateTime ServerStarted;
        private static DatabaseManager manager;
        //internal static IRCBot messagingBot;
        internal static bool IrcEnabled;
        internal static bool groupsEnabled;
        internal static bool SystemMute;
        internal static bool useSSO;
        internal static bool isLive;
        internal static bool diagPackets = true;
        internal static int timeout = 500;
        internal static DatabaseType dbType;
        internal static MusSocket MusSystem;
        internal static CultureInfo cultureInfo;


        internal static string Title
        {
            get
            {
                return "Pici";
            }
        }

        internal static string Version
        {
            get
            {
                return "1.0";
            }
        }

        internal static int Build
        {
            get
            {
                return 150;
            }
        }

        internal static bool bool_0_12 = false;

        internal static string LicenseHolder;

        internal static string LicenseName;

        internal static string LicensePass;

        internal static void Initialize()
        {
            Console.Clear();
            DateTime Start = DateTime.Now;
            SystemMute = false;

            IrcEnabled = false;
            ServerStarted = DateTime.Now;
            Console.Title = PiciEnvironment.Title + " " + PiciEnvironment.Version;
            Console.WindowHeight = 30;
            DefaultEncoding = Encoding.Default;

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("");
            Console.WriteLine("          ______ _       _    _______             ");
            Console.WriteLine("         (_____ (_)     (_)  (_______)            ");
            Console.WriteLine("          _____) )  ____ _    _____   ____  _   _ ");
            Console.WriteLine(@"         |  ____/ |/ ___) |  |  ___) |    \| | | |");
            Console.WriteLine(@"         | |    | ( (___| |  | |_____| | | | |_| |");
            Console.WriteLine(@"         |_|    |_|\____)_|  |_______)_|_|_|____/ ");
                                         

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("                              "+PiciEnvironment.Title+" " + PiciEnvironment.Version + " (Build " + PiciEnvironment.Build + ")");


            Console.WriteLine();

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine();

            cultureInfo = CultureInfo.CreateSpecificCulture("en-GB");
            LanguageLocale.Init();

            try
            {
                ChatCommandRegister.Init();
                PetCommandHandeler.Init();
                PetLocale.Init();
                Configuration = new ConfigurationData(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath,@"config.conf"));

                DateTime Starts = DateTime.Now;

                dbType = DatabaseType.MySQL;

                manager = new DatabaseManager(uint.Parse(PiciEnvironment.GetConfig().data["db.pool.maxsize"]), uint.Parse(PiciEnvironment.GetConfig().data["db.pool.minsize"]), int.Parse(PiciEnvironment.GetConfig().data["db.pool.minsize"]), dbType);
                manager.setServerDetails(
                    PiciEnvironment.GetConfig().data["db.hostname"],
                    uint.Parse(PiciEnvironment.GetConfig().data["db.port"]),
                    PiciEnvironment.GetConfig().data["db.username"],
                    PiciEnvironment.GetConfig().data["db.password"],
                    PiciEnvironment.GetConfig().data["db.name"]);
                manager.init();

                TimeSpan TimeUsed2 = DateTime.Now - Starts;

                LanguageLocale.InitSwearWord();

                Game = new Game(int.Parse(PiciEnvironment.GetConfig().data["game.tcp.conlimit"]));
                Game.ContinueLoading();

                ConnectionManager = new ConnectionHandeling(int.Parse(PiciEnvironment.GetConfig().data["game.tcp.port"]),
                    int.Parse(PiciEnvironment.GetConfig().data["game.tcp.conlimit"]),
                    int.Parse(PiciEnvironment.GetConfig().data["game.tcp.conperip"]),
                    PiciEnvironment.GetConfig().data["game.tcp.enablenagles"].ToLower() == "true");
                ConnectionManager.init();
                    
                ConnectionManager.Start();

                StaticClientMessageHandler.Initialize();
                ClientMessageFactory.Init();

                string[] arrayshit = PiciEnvironment.GetConfig().data["mus.tcp.allowedaddr"].Split(Convert.ToChar(","));
                
                MusSystem = new MusSocket(PiciEnvironment.GetConfig().data["mus.tcp.bindip"], int.Parse(PiciEnvironment.GetConfig().data["mus.tcp.port"]), arrayshit, 0);
                
                //InitIRC(); 

                groupsEnabled = true;

                useSSO = true;

                TimeSpan TimeUsed = DateTime.Now - Start;

                Logging.WriteLine("Server -> Started! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");
                isLive = true;

                Console.Beep();


                if (bool_0_12)
                {
                    Console.WriteLine("Coffee team, I appreciate you testing. ;-*");
                    System.Threading.Thread.Sleep(2500);
                    PreformShutDown(true);
                    return;
                }

            }
            catch (KeyNotFoundException e)
            {
                Logging.WriteLine("Please check your configuration file - some values appear to be missing.");
                Logging.WriteLine("Press any key to shut down ...");
                Logging.WriteLine(e.ToString());
                Console.ReadKey(true);
                PiciEnvironment.Destroy();

                return;
            }
            catch (InvalidOperationException e)
            {
                Logging.WriteLine("Failed to initialize PiciEmulator: " + e.Message);
                Logging.WriteLine("Press any key to shut down ...");

                Console.ReadKey(true);
                PiciEnvironment.Destroy();

                return;
            }

            catch (Exception e)
            {
                Console.WriteLine("Fatal error during startup: " + e.ToString());
                Console.WriteLine("Press a key to exit");

                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        internal static void InitIRC()
        {
            //if (PiciEnvironment.GetConfig().data["irc.enabled"] == "true")
            //{
            //    UserFactory.Init();

            //    messagingBot = new IRCBot(
            //        PiciEnvironment.GetConfig().data["irc.server"],
            //        int.Parse(PiciEnvironment.GetConfig().data["irc.port"]),
            //        PiciEnvironment.GetConfig().data["irc.user"],
            //        PiciEnvironment.GetConfig().data["irc.nick"],
            //        PiciEnvironment.GetConfig().data["irc.channel"],
            //        PiciEnvironment.GetConfig().data["irc.password"]);

            //    messagingBot.Start();
            //    IrcEnabled = true;
            //}
        }

        //private static string encodeVL64(int i)
        //{
        //    byte[] wf = new byte[6];
        //    int pos = 0;
        //    int startPos = pos;
        //    int bytes = 1;
        //    int negativeMask = i >= 0 ? 0 : 4;
        //    i = Math.Abs(i);
        //    wf[pos++] = (byte)(64 + (i & 3));
        //    for (i >>= 2; i != 0; i >>= 6)
        //    {
        //        bytes++;
        //        wf[pos++] = (byte)(64 + (i & 0x3f));
        //    }

        //    wf[startPos] = (byte)(wf[startPos] | bytes << 3 | negativeMask);

        //    System.Text.ASCIIEncoding encoder = new ASCIIEncoding();
        //    string tmp = encoder.GetString(wf);
        //    return tmp.Replace("\0", "");
        //}

        internal static bool EnumToBool(string Enum)
        {
            return (Enum == "1");
        }

        

        internal static string BoolToEnum(bool Bool)
        {
            if (Bool)
            {
                return "1";
            }

            return "0";
        }

        internal static int GetRandomNumber(int Min, int Max)
        {
            RandomBase Quick = new Quick();
            return Quick.Next(Min, Max);
        }

        internal static int GetUnixTimestamp()
        {
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            double unixTime = ts.TotalSeconds;
            return (int)unixTime;
        }

        internal static string FilterInjectionChars(string Input)
        {
            return FilterInjectionChars(Input, false);
        }

        private static readonly List<char> allowedchars = new List<char>(new char[]{ 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 
                                                'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 
                                                'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '.' });

        internal static string FilterFigure(string figure)
        {
            foreach (char character in figure)
            {
                if (!isValid(character))
                    return "lg-3023-1335.hr-828-45.sh-295-1332.hd-180-4.ea-3168-89.ca-1813-62.ch-235-1332";
            }

            return figure;
        }

        private static bool isValid(char character)
        {
            return allowedchars.Contains(character);
        }

        internal static string FilterInjectionChars(string Input, bool AllowLinebreaks)
        {
            Input = Input.Replace(Convert.ToChar(1), ' ');
            Input = Input.Replace(Convert.ToChar(2), ' ');
            //Input = Input.Replace(Convert.ToChar(3), ' ');
            Input = Input.Replace(Convert.ToChar(9), ' ');

            if (!AllowLinebreaks)
            {
                Input = Input.Replace(Convert.ToChar(13), ' ');
            }

            return Input;
        }

        internal static bool IsValidAlphaNumeric(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }

            for (int i = 0; i < inputStr.Length; i++)
            {
                if (!(char.IsLetter(inputStr[i])) && (!(char.IsNumber(inputStr[i]))))
                {
                    return false;
                }
            }

            return true;
        }

        internal static ConfigurationData GetConfig()
        {
            return Configuration;
        }

        //internal static DatabaseManagerOld GetDatabase()
        //{
        //    return DatabaseManager;
        //}

        internal static Encoding GetDefaultEncoding()
        {
            return DefaultEncoding;
        }

        internal static ConnectionHandeling GetConnectionManager()
        {
            return ConnectionManager;
        }

        internal static Game GetGame()
        {
            return Game;
        }

        //internal static Game GameInstance
        //{
        //    get
        //    {
        //        return Game;
        //    }
        //    set
        //    {
        //        Game = value;
        //    }
        //}

        internal static void Destroy()
        {
            isLive = false;
            Logging.WriteLine("Destroying Pici environment...");

            if (GetGame() != null)
            {
                GetGame().Destroy();
                Game = null;
            }

            if (GetConnectionManager() != null)
            {
                Logging.WriteLine("Destroying connection manager.");
                GetConnectionManager().Destroy();
                //ConnectionManager = null;
            }

            if (manager != null)
            {
                try
                {
                    Logging.WriteLine("Destroying database manager.");
                    //GetDatabase().StopClientMonitor();
                    manager.destroy();
                    //GetDatabase().DestroyDatabaseManager();
                    //DatabaseManager = null;
                }
                catch { }
            }

            Logging.WriteLine("Uninitialized successfully. Closing.");

            //Environment.Exit(0); Cba :P
        }

        private static bool ShutdownInitiated = false;

        internal static bool ShutdownStarted
        {
            get
            {
                return ShutdownInitiated;
            }
        }

        internal static void SendMassMessage(string Message)
        {
            try
            {
                ServerMessage HotelAlert = new ServerMessage(808);
                HotelAlert.Append("Important Notice from Hotel Management");
                HotelAlert.Append(Message);
                PiciEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(HotelAlert);
            }
            catch (Exception e) { Logging.HandleException(e, "PiciEnvironment.SendMassMessage"); }
        }

        internal static DatabaseManager GetDatabaseManager()
        {
            return manager;
        }

        internal static void PreformShutDown()
        {
            PreformShutDown(true);
        }

        internal static void PreformShutDown(bool ExitWhenDone)
        {
            if (ShutdownInitiated || !isLive)
                return;

            StringBuilder builder = new StringBuilder();

            DateTime ShutdownStart = DateTime.Now;

            DateTime MessaMessage = DateTime.Now;
            ShutdownInitiated = true;

            SendMassMessage(LanguageLocale.GetValue("shutdown.alert"));
            AppendTimeStampWithComment(ref builder, MessaMessage, "Hotel pre-warning");

            Game.StopGameLoop();
            Console.Write("Game loop stopped");

            DateTime ConnectionClose = DateTime.Now;
            Console.WriteLine("Server shutting down...");
            Console.Title = "<<- SERVER SHUTDOWN ->>";
            
            GetConnectionManager().Destroy();
            AppendTimeStampWithComment(ref builder, ConnectionClose, "Socket close");

            DateTime sConnectionClose = DateTime.Now;
            GetGame().GetClientManager().CloseAll();
            AppendTimeStampWithComment(ref builder, sConnectionClose, "Furni pre-save and connection close");

            DateTime RoomRemove = DateTime.Now;
            Console.WriteLine("<<- SERVER SHUTDOWN ->> ROOM SAVE");
            Game.GetRoomManager().RemoveAllRooms();
            AppendTimeStampWithComment(ref builder, RoomRemove, "Room destructor");

            DateTime DbSave = DateTime.Now;

            using (IQueryAdapter dbClient = manager.getQueryreactor())
            {
               // dbClient.runFastQuery("TRUNCATE TABLE user_tickets");
                dbClient.runFastQuery("TRUNCATE TABLE user_online");
                dbClient.runFastQuery("TRUNCATE TABLE room_active");
                //dbClient.runFastQuery("UPDATE users SET online = 0");
                //dbClient.runFastQuery("UPDATE rooms SET users_now = 0");
            }
            AppendTimeStampWithComment(ref builder, DbSave, "Database pre-save");

            DateTime connectionShutdown = DateTime.Now;
            ConnectionManager.Destroy();
            AppendTimeStampWithComment(ref builder, connectionShutdown, "Connection shutdown");

            DateTime gameDestroy = DateTime.Now;
            Game.Destroy();
            AppendTimeStampWithComment(ref builder, gameDestroy, "Game destroy");

            DateTime databaseDeconstructor = DateTime.Now;
            try
            {
                Console.WriteLine("Destroying database manager.");

                manager.destroy();
            }
            catch { }
            AppendTimeStampWithComment(ref builder, databaseDeconstructor, "Database shutdown");

            TimeSpan timeUsedOnShutdown = DateTime.Now - ShutdownStart;
            builder.AppendLine("Total time on shutdown " + TimeSpanToString(timeUsedOnShutdown));
            builder.AppendLine("You have reached ==> [END OF SESSION]");
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine();

            Logging.LogShutdown(builder);

            Console.WriteLine("System disposed, goodbye!");
            if (ExitWhenDone)
                Environment.Exit(Environment.ExitCode);
        }

        internal static string TimeSpanToString(TimeSpan span)
        {
            return span.Seconds + " s, " + span.Milliseconds + " ms";
        }

        internal static void AppendTimeStampWithComment(ref StringBuilder builder, DateTime time, string text)
        {
            builder.AppendLine(text + " =>[" + TimeSpanToString(DateTime.Now - time) + "]");
        }

        
    }
}
