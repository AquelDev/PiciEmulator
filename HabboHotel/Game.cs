using System;
using System.Threading.Tasks;
using Pici.Core;
using Pici.HabboHotel.Achievements;
using Pici.HabboHotel.Advertisements;
using Pici.HabboHotel.Catalogs;
using Pici.HabboHotel.GameClients;
using Pici.HabboHotel.Items;
using Pici.HabboHotel.Misc;
using Pici.HabboHotel.Navigators;
using Pici.HabboHotel.Roles;
using Pici.HabboHotel.RoomBots;
using Pici.HabboHotel.Rooms;
using Pici.HabboHotel.Support;
using Pici.ServerManager;
using Pici.HabboHotel.Users.Inventory;
using Pici.Storage.Database.Session_Details.Interfaces;
using System.Threading;
using Pici.HabboHotel.Quests;
using Pici.HabboHotel.SoundMachine;


namespace Pici.HabboHotel
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

            //if (PiciEnvironment.GetConfig().data["client.ping.enabled"] == "1")
            //{
            //    ClientManager.StartConnectionChecker();
            //}

            
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                
                //PiciEnvironment.GameInstance = this;
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
            }
        }

        internal void ContinueLoading()
        {
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                BanManager.LoadBans(dbClient);
                Logging.WriteLine("Ban manager -> READY!");

                //RoleManager.LoadRoles(dbClient);
                RoleManager.LoadRights(dbClient);
                Logging.WriteLine("Role manager -> READY!");

                HelpTool.LoadCategories(dbClient);
                HelpTool.LoadTopics(dbClient);
                Logging.WriteLine("Help tool -> READY!");

                Catalog.Initialize(dbClient);
                Logging.WriteLine("Catacache -> READY!");

                Navigator.Initialize(dbClient);
                Logging.WriteLine("Navigator -> READY!");

                ItemManager.LoadItems(dbClient);
                globalInventory = new InventoryGlobal();
                Logging.WriteLine("Item manager -> READY!");

                RoomManager.LoadModels(dbClient);
                RoomManager.InitRoomLinks(dbClient);
                RoomManager.InitVotedRooms(dbClient);
                Logging.WriteLine("Room manager -> READY!");

                AdvertisementManager.LoadRoomAdvertisements(dbClient);
                Logging.WriteLine("Advertisement manager -> READY!");

                AchievementManager = new AchievementManager(dbClient);
                questManager.Initialize(dbClient);
                Logging.WriteLine("Achievement manager -> READY!");

                ModerationTool.LoadMessagePresets(dbClient);
                ModerationTool.LoadPendingTickets(dbClient);
                Logging.WriteLine("Moderation tool -> READY!");

                BotManager.LoadBots(dbClient);
                Logging.WriteLine("Bot manager manager -> READY!");

                Catalog.InitCache();
                Logging.WriteLine("Catalogue manager -> READY!");

                SongManager.Initialize();
                Logging.WriteLine("Sound manager -> READY!");

                DatabaseCleanup(dbClient);
                LowPriorityWorker.Init(dbClient);
                Logging.WriteLine("Database -> Cleanup performed!");
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
            dbClient.runFastQuery("UPDATE server_status SET status = 1, users_online = 0, rooms_loaded = 0, server_ver = '" + PiciEnvironment.Build + "', stamp = '" + PiciEnvironment.GetUnixTimestamp() + "' ");
        }

        internal void Destroy()
        {

            //Stop game thread

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
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
