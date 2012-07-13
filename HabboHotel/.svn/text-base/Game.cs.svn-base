using System;
using System.Threading.Tasks;
using Butterfly.Core;
using Butterfly.HabboHotel.Achievements;
using Butterfly.HabboHotel.Advertisements;
using Butterfly.HabboHotel.Catalogs;
using Butterfly.HabboHotel.GameClients;
using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Misc;
using Butterfly.HabboHotel.Navigators;
using Butterfly.HabboHotel.Roles;
using Butterfly.HabboHotel.RoomBots;
using Butterfly.HabboHotel.Rooms;
using Butterfly.HabboHotel.Support;
using Butterfly.ServerManager;
using Butterfly.HabboHotel.Users.Inventory;
using Database_Manager.Database.Session_Details.Interfaces;
using System.Threading;
using Butterfly.HabboHotel.Quests;
using Butterfly.HabboHotel.SoundMachine;


namespace Butterfly.HabboHotel
{
    class Game
    {
        #region Fields
        private GameClientManager ClientManager;
        private ModerationBanManager BanManager;
        private RoleManager RoleManager;
        private HelpTool HelpTool;
        private Catalog Catalog;
        private Navigator Navigator;
        private ItemManager ItemManager;
        private RoomManager RoomManager;
        private AdvertisementManager AdvertisementManager;
        private PixelManager PixelManager;
        private AchievementManager AchievementManager;
        private ModerationTool ModerationTool;
        private BotManager BotManager;
        //private Task StatisticsThread;
        //private Task ConsoleTitleTask;
        private InventoryGlobal globalInventory;
        private QuestManager questManager;
        //private SoundMachineManager soundMachineManager;

        private Task gameLoop;
        private bool gameLoopActive;
        private bool gameLoopEnded;
        private const int gameLoopSleepTime = 25;

        #endregion

        #region Return values

        internal GameClientManager GetClientManager()
        {
            return ClientManager;
        }

        internal ModerationBanManager GetBanManager()
        {
            return BanManager;
        }

        internal RoleManager GetRoleManager()
        {
            return RoleManager;
        }

        internal HelpTool GetHelpTool()
        {
            return HelpTool;
        }

        internal Catalog GetCatalog()
        {
            return Catalog;
        }

        internal Navigator GetNavigator()
        {
            return Navigator;
        }

        internal ItemManager GetItemManager()
        {
            return ItemManager;
        }

        internal RoomManager GetRoomManager()
        {
            return RoomManager;
        }

        internal AdvertisementManager GetAdvertisementManager()
        {
            return AdvertisementManager;
        }

        internal PixelManager GetPixelManager()
        {
            return PixelManager;
        }

        internal AchievementManager GetAchievementManager()
        {
            return AchievementManager;
        }

        internal ModerationTool GetModerationTool()
        {
            return ModerationTool;
        }

        internal BotManager GetBotManager()
        {
            return BotManager;
        }

        internal InventoryGlobal GetInventory()
        {
            return globalInventory;
        }

        internal QuestManager GetQuestManager()
        {
            return questManager;
        }
        #endregion

        #region Boot
        internal Game(int conns)
        {
            ClientManager = new GameClientManager();

            //if (ButterflyEnvironment.GetConfig().data["client.ping.enabled"] == "1")
            //{
            //    ClientManager.StartConnectionChecker();
            //}

            
            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                
                //ButterflyEnvironment.GameInstance = this;
                DateTime start = DateTime.Now;

                BanManager = new ModerationBanManager();
                RoleManager = new RoleManager();
                HelpTool = new HelpTool();
                Catalog = new Catalog();
                Navigator = new Navigator();
                ItemManager = new ItemManager();
                RoomManager = new RoomManager();
                AdvertisementManager = new AdvertisementManager();
                PixelManager = new PixelManager();
                
                ModerationTool = new ModerationTool();
                BotManager = new BotManager();
                questManager = new QuestManager();
                //soundMachineManager = new SoundMachineManager();

                TimeSpan spent = DateTime.Now - start;

                Logging.WriteLine("Class initialization -> READY! (" + spent.Seconds + " s, " + spent.Milliseconds + " ms)");
            }
        }

        internal void ContinueLoading()
        {
            DateTime Start;
            TimeSpan TimeUsed;
            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                Start = DateTime.Now;
                BanManager.LoadBans(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine("Ban manager -> READY! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                //RoleManager.LoadRoles(dbClient);
                RoleManager.LoadRights(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine("Role manager -> READY! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                HelpTool.LoadCategories(dbClient);
                HelpTool.LoadTopics(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine("Help tool -> READY! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                Catalog.Initialize(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine("Catacache -> READY! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                Navigator.Initialize(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine("Navigator -> READY! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                ItemManager.LoadItems(dbClient);
                globalInventory = new InventoryGlobal();
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine("Item manager -> READY! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                RoomManager.LoadModels(dbClient);
                RoomManager.InitRoomLinks(dbClient);
                RoomManager.InitVotedRooms(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine("Room manager -> READY! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                AdvertisementManager.LoadRoomAdvertisements(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine("Adviserment manager -> READY! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                AchievementManager = new AchievementManager(dbClient);
                questManager.Initialize(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine("Achievement manager -> READY! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                ModerationTool.LoadMessagePresets(dbClient);
                ModerationTool.LoadPendingTickets(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine("Moderation tool -> READY! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                BotManager.LoadBots(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine("Bot manager manager -> READY! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                Catalog.InitCache();
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine("Catalogue manager -> READY! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                SongManager.Initialize();
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine("Sound manager -> READY! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");


                Start = DateTime.Now;
                DatabaseCleanup(dbClient);
                LowPriorityWorker.Init(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine("Database -> Cleanup performed! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");
            }

            StartGameLoop();

            Logging.WriteLine("Game manager -> READY!");

            DataSocket.SetupListener(42);
            DataSocket.Start();
        }

        #endregion

        #region Game loop
        internal void StartGameLoop()
        {
            gameLoopEnded = false;
            gameLoopActive = true;
            gameLoop = new Task(MainGameLoop);
            gameLoop.Start();
        }

        internal void StopGameLoop()
        {
            gameLoopActive = false;

            while (!gameLoopEnded)
            {
                Thread.Sleep(gameLoopSleepTime);
            }
        }

        internal byte GameLoopStatus = 0;

        internal static bool gameLoopEnabled = true;
        private void MainGameLoop()
        {
            DateTime time;
            TimeSpan spent;
            while (gameLoopActive)
            {
                if (gameLoopEnabled)
                {
                    try
                    {
                        GameLoopStatus = 1;
                        time = DateTime.Now;
                        LowPriorityWorker.Process(); //1 query
                        spent = DateTime.Now - time;
                        if (spent.TotalSeconds > 3)
                            Console.WriteLine("Low priority Process worker took really long time to cycle!");

                        GameLoopStatus = 2;
                        time = DateTime.Now;
                        LowPriorityWorker.ConsoleTitleWorker();
                        spent = DateTime.Now - time;
                        if (spent.TotalSeconds > 3)
                            Console.WriteLine("Low priority ConsoleTitleWorker worker took really long time to cycle!");

                        GameLoopStatus = 5;
                        time = DateTime.Now;
                        RoomManager.OnCycle(); // Queries for furni save
                        spent = DateTime.Now - time;
                        if (spent.TotalSeconds > 3)
                            Console.WriteLine("RoomManager.OnCycle took really long time to cycle!");

                        GameLoopStatus = 6;
                        time = DateTime.Now;
                        ClientManager.OnCycle();
                        spent = DateTime.Now - time;
                        if (spent.TotalSeconds > 3)
                            Console.WriteLine("ClientManager.OnCycle took really long time to cycle!");

                        GameLoopStatus = 7;
                    }
                    catch (Exception e)
                    {
                        Logging.LogCriticalException("INVALID MARIO BUG IN GAME LOOP: " + e.ToString());
                    }
                    GameLoopStatus = 8;
                }
                Thread.Sleep(gameLoopSleepTime);
            }

            gameLoopEnded = true;
        }
        #endregion

        #region Shutdown
        internal static void DatabaseCleanup(IQueryAdapter dbClient)
        {
            //dbClient.runFastQuery("TRUNCATE TABLE user_tickets");
            dbClient.runFastQuery("TRUNCATE TABLE user_online");
            dbClient.runFastQuery("TRUNCATE TABLE room_active");
            dbClient.runFastQuery("UPDATE server_status SET status = 1, users_online = 0, rooms_loaded = 0, server_ver = '" + ButterflyEnvironment.PrettyVersion + "', stamp = '" + ButterflyEnvironment.GetUnixTimestamp() + "' ");
        }

        internal void Destroy()
        {

            //Stop game thread

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                DatabaseCleanup(dbClient);
            }
            if (GetClientManager() != null)
            {
                //GetClientManager().Clear();
                //GetClientManager().StopConnectionChecker();
            }

            Console.WriteLine("Destroyed Habbo Hotel.");
        }
        #endregion

        
    }
}
