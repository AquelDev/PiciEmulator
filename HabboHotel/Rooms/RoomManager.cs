﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using Pici.Core;
using Pici.HabboHotel.Events;
using Pici.HabboHotel.GameClients;
using Pici.HabboHotel.Users;
using Pici.Messages;
using Pici.Storage.Database;
using Pici.Storage.Database.Session_Details.Interfaces;


namespace Pici.HabboHotel.Rooms
{
    class RoomManager
    {
        #region Fields
        internal const int MAX_PETS_PER_ROOM = 15;

        private Dictionary<uint, Room> loadedRooms;

        private Queue roomsToAddQueue;
        private Queue roomsToRemoveQueue;
        private Queue roomDataToAddQueue;

        private Queue votedRoomsAddQueue;
        private Queue votedRoomsRemoveQueue;

        private Queue activeRoomsUpdateQueue;
        private Queue activeRoomsAddQueue;
        private Queue activeRoomsRemoveQueue;

        private Hashtable roomModels;
        private Hashtable loadedRoomData;
        private Dictionary<uint, List<RoomLinkInformation>> roomLinks;
        private Dictionary<int, RoomCategory> roomCategories;

        private Dictionary<RoomData, int> votedRooms;
        private IEnumerable<KeyValuePair<RoomData, int>> orderedVotedRooms;

        private Dictionary<RoomData, int> activeRooms;
        private IEnumerable<KeyValuePair<RoomData, int>> orderedActiveRooms;

        private List<Room> mToRemove;

        private EventManager eventManager;
        #endregion

        #region Return values

        internal int LoadedRoomsCount
        {
            get
            {
                return this.loadedRooms.Count;
            }
        }

        internal EventManager GetEventManager()
        {
            return eventManager;
        }

        internal KeyValuePair<RoomData, int>[] GetRooms(int categoryID)
        {
            return roomCategories[categoryID].GetOrderedRooms();
        }

        internal KeyValuePair<RoomData, int>[] GetActiveRooms()
        {
            return orderedActiveRooms.ToArray();
        }

        internal KeyValuePair<RoomData, int>[] GetVotedRooms()
        {
            return orderedVotedRooms.ToArray();
        }

        internal List<RoomLinkInformation> getLinkedRoomData(uint roomID)
        {
            if (roomLinks.ContainsKey(roomID))
                return roomLinks[roomID];
            else
                return new List<RoomLinkInformation>();
        }


        private static RoomModel GetCustomData(UInt32 roomID)
        {
            DataRow RoomData;
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT doorx,doory,height,modeldata,poolmap FROM room_models_customs WHERE roomid = " + roomID);
                RoomData = dbClient.getRow();
            }

            if (RoomData == null)
                throw new Exception("The custom room model for room " + roomID + " was not found");

            string Modeldata = (string)RoomData["modeldata"];
            return new RoomModel((int)RoomData["doorx"], (int)RoomData["doory"], (Double)RoomData["height"], 2, Modeldata, "", 0, string.Empty, new List<PublicRoomSquare>());
        }

        internal RoomModel GetModel(string Model, UInt32 RoomID)
        {
            if (Model == "custom")
                return GetCustomData(RoomID);

            if (roomModels.ContainsKey(Model))
                return (RoomModel)roomModels[Model];

            return null;
        }

        internal RoomData GenerateNullableRoomData(UInt32 RoomId)
        {
            if (GenerateRoomData(RoomId) != null)
            {
                return GenerateRoomData(RoomId);
            }

            RoomData Data = new RoomData();
            Data.FillNull(RoomId);
            return Data;
        }

        internal RoomData GenerateRoomData(UInt32 RoomId)
        {
            
            if (loadedRoomData.ContainsKey(RoomId))
                return (RoomData)loadedRoomData[RoomId];

            RoomData Data = new RoomData();
            if (IsRoomLoaded(RoomId))
            {
                return GetRoom(RoomId).RoomData;
            }
            else
            {
                DataRow Row = null;

                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.setQuery("SELECT rooms.*, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE id = " + RoomId);
                    Row = dbClient.getRow();
                }

                if (Row == null)
                {
                    return null;
                }

                Data.Fill(Row);
                loadedRoomData.Add(RoomId, Data);
            }

            return Data;
        }

        internal KeyValuePair<RoomData, int>[] GetEventRoomsForCategory(int categoryID)
        {
            return eventManager.GetRoomsForCategory(categoryID);
        }

        internal Boolean IsRoomLoaded(UInt32 RoomId)
        {
            return loadedRooms.ContainsKey(RoomId);
        }

        internal Room LoadRoom(UInt32 Id)
        {
            if (IsRoomLoaded(Id))
            {
                return GetRoom(Id);
            }

            RoomData Data = GenerateRoomData(Id);

            if (Data == null)
                return null;

            Room Room = new Room(Data);

            //Room Room = new Room(Data.Id, Data.Name, Data.Description, Data.Type, Data.Owner, Data.Category, Data.State,
            //    Data.UsersMax, Data.ModelName, Data.CCTs, Data.Score, Data.Tags, Data.AllowPets, Data.AllowPetsEating,
            //    Data.AllowWalkthrough, Data.Hidewall, Data.Icon, Data.Password, Data.Wallpaper, Data.Floor, Data.Landscape, Data, Data.AllowRightsOverride);

            lock (roomsToAddQueue.SyncRoot)
            {
                roomsToAddQueue.Enqueue(Room);
            }


            Room.InitBots();
            Room.InitPets();

            //Logging.WriteLine("[RoomMgr] Loaded room: \"" + Room.Name + "\" (ID: " + Id + ")");

            return Room;
        }

        internal RoomData FetchRoomData(UInt32 RoomId, DataRow dRow)
        {
            if (loadedRoomData.ContainsKey(RoomId))
                return (RoomData)loadedRoomData[RoomId];
            else
            {
                RoomData data = new RoomData();
                if (IsRoomLoaded(RoomId))
                    data.Fill(GetRoom(RoomId));
                else
                    data.Fill(dRow);

                loadedRoomData.Add(RoomId, data);
                return data;
            }
        }

        internal Room GetRoom(uint roomID)
        {
            Room room;
            if (loadedRooms.TryGetValue(roomID, out room))
                return room;

            return null;
            //thRow new ObjectNotFoundException("No room for roomID " + RoomId + " found, script aborted.");
        }

        internal RoomData CreateRoom(GameClient Session, string Name, string Model)
        {
            Name = PiciEnvironment.FilterInjectionChars(Name);

            if (!roomModels.ContainsKey(Model))
            {
                Session.SendNotif(LanguageLocale.GetValue("room.modelmissing"));
                return null;
            }

            Console.WriteLine("ClubOnly: " + ((RoomModel)roomModels[Model]).ClubOnly + "");

            if (((RoomModel)roomModels[Model]).ClubOnly == 1 && !Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_vip"))
            {
                Session.SendNotif(LanguageLocale.GetValue("room.missingclub"));
                return null;
            }

            if (Name.Length < 3)
            {
                Session.SendNotif(LanguageLocale.GetValue("room.namelengthshort"));
                return null;
            }

            UInt32 RoomId = 0;

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (dbClient.dbType == DatabaseType.MSSQL)//description,public_ccts,tags,password
                    dbClient.setQuery("INSERT INTO rooms (roomtype,caption,owner,model_name,description,public_ccts,tags,password) OUTPUT INSERTED.* VALUES ('private',@caption,@username,@model,'','','','')");
                else
                    dbClient.setQuery("INSERT INTO rooms (roomtype,caption,owner,model_name) VALUES ('private',@caption,@username,@model)");
                dbClient.addParameter("caption", Name);
                dbClient.addParameter("model", Model);
                dbClient.addParameter("username", Session.GetHabbo().Username);

                RoomId = (UInt32)dbClient.insertQuery();


            }

            RoomData newRoomData = GenerateRoomData(RoomId);
            Session.GetHabbo().UsersRooms.Add(newRoomData);

            return newRoomData;
        }

        //internal List<RoomData> GetActiveRooms(int count)
        //{
        //    return (from p in loadedRooms.Values
        //            where p.UsersNow > 0
        //            orderby p.UsersNow
        //            descending
        //            select p.GetRoomData).Take(count).ToList();
        //}

        //internal List<RoomData> GetActiveRooms(int count, int category)
        //{
        //     List<RoomData> rooms = (from p in loadedRooms.Values
        //                 where p.UsersNow > 0 && p.Category == category
        //                 orderby p.UsersNow
        //                 descending
        //                 select p.GetRoomData).Take(count).ToList();

        //    return rooms;
        //}

        //internal List<RoomData> GetVotedRooms(int count)
        //{
        //    List<RoomData> rooms = (from p in loadedRoomData.Values
        //                 orderby p.Score
        //                 descending
        //                 select p).Take(count).ToList();

        //    return rooms;
        //}
        #endregion

        #region Boot

        internal RoomManager()
        {
            this.loadedRooms = new Dictionary<uint, Room>();

            this.roomModels = new Hashtable();
            this.loadedRoomData = new Hashtable();
            this.mToRemove = new List<Room>();
            this.roomCategories = new Dictionary<int, RoomCategory>();

            this.votedRooms = new Dictionary<RoomData, int>();
            this.activeRooms = new Dictionary<RoomData, int>();

            this.roomsToAddQueue = new Queue();
            this.roomsToRemoveQueue = new Queue();
            this.roomDataToAddQueue = new Queue();

            this.votedRoomsRemoveQueue = new Queue();
            this.votedRoomsAddQueue = new Queue();

            this.activeRoomsRemoveQueue = new Queue();
            this.activeRoomsUpdateQueue = new Queue();
            this.activeRoomsAddQueue = new Queue();

            this.eventManager = new EventManager();
        }

        internal void InitRoomLinks(IQueryAdapter dbClient)
        {
            this.roomLinks = new Dictionary<uint, List<RoomLinkInformation>>();

            dbClient.setQuery("SELECT * FROM room_links");
            DataTable roomLinkData = dbClient.getTable();

            foreach (DataRow Row in roomLinkData.Rows)
            {
                RoomLinkInformation info = new RoomLinkInformation(Row);

                if (roomLinks.ContainsKey(info.roomID))
                {
                    roomLinks[info.roomID].Add(info);
                }
                else
                {
                    List<RoomLinkInformation> newList = new List<RoomLinkInformation>();
                    newList.Add(info);
                    roomLinks.Add(info.roomID, newList);
                }
            }


            dbClient.setQuery("SELECT id,caption,min_rank FROM navigator_flatcats WHERE enabled = 2");
            DataTable dPrivCats = dbClient.getTable();

            int categoryID;
            string caption;
            foreach (DataRow dRow in dPrivCats.Rows)
            {
                categoryID = (int)dRow[0];
                caption = (string)dRow[1];

                RoomCategory category = new RoomCategory(categoryID, caption);
                roomCategories.Add(categoryID, category);
            }
        }

        internal void InitVotedRooms(IQueryAdapter dbClient)
        {
            if (dbClient.dbType == Pici.Storage.Database.DatabaseType.MySQL)
                dbClient.setQuery("SELECT rooms.*, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE score > 0 AND roomtype = 'private' ORDER BY score DESC LIMIT 40");
            else
                dbClient.setQuery("SELECT TOP 40 rooms.*, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE score > 0 AND roomtype = 'private' ORDER BY score DESC");
            DataTable dTable = dbClient.getTable();

            foreach (DataRow dRow in dTable.Rows)
            {
                RoomData data = FetchRoomData(Convert.ToUInt32(dRow["id"]), dRow);
                QueueVoteAdd(data);
            }
        }

        internal void LoadModels(IQueryAdapter dbClient)
        {
            roomModels.Clear();

            dbClient.setQuery("SELECT id,door_x,door_y,door_z,door_dir,heightmap,public_items,club_only,poolmap FROM room_models");
            DataTable Data = dbClient.getTable();

            dbClient.setQuery("SELECT model, x, y, rot FROM room_model_static");
            DataTable PublicRoomItems = dbClient.getTable();

            List<PublicRoomSquare> PublicRoomDataSqList = new List<PublicRoomSquare>();

            foreach (DataRow dRow in PublicRoomItems.Rows)
            {
                PublicRoomDataSqList.Add(new PublicRoomSquare(dRow));
            }

            if (Data == null)
                return;

            foreach (DataRow Row in Data.Rows)
            {
                string Modelname = (string)Row["id"];
                string staticFurniture = (string)Row["public_items"];

                List<PublicRoomSquare> publicFurni = new List<PublicRoomSquare>();
                if (!string.IsNullOrEmpty(staticFurniture))
                {
                    publicFurni.AddRange(from p in PublicRoomDataSqList where p.RoomModelName == Modelname select p);
                }

                roomModels.Add(Modelname, new RoomModel((int)Row["door_x"],
                    (int)Row["door_y"], (Double)Row["door_z"], (int)Row["door_dir"], (string)Row["heightmap"],
                    staticFurniture, (int)Row["club_only"], (string)Row["poolmap"], publicFurni));
            }
        }
        #endregion

        #region Threading
        internal void OnCycle()
        {
            WorkRoomDataQueue();
            WorkRoomsToAddQueue();
            WorkRoomsToRemoveQueue();
            
            RoomCycleTask();
            RemoveTask(); // Queries

            bool activeRoomsAdded = WorkActiveRoomsAddQueue();
            bool activeRoomsRemoved = WorkActiveRoomsRemoveQueue();
            bool activeRoomsUpdated = WorkActiveRoomsUpdateQueue();

            if (activeRoomsAdded || activeRoomsRemoved || activeRoomsUpdated)
                SortActiveRooms();

            bool votedRoomsAdded = WorkVotedRoomsAddQueue();
            bool votedRoomsRemoved = WorkVotedRoomsRemoveQueue();

            if (votedRoomsAdded || votedRoomsRemoved)
                SortVotedRooms();

            WorkCategories();
            eventManager.onCycle();
        }


        private void WorkCategories()
        {
            foreach (RoomCategory category in roomCategories.Values)
            {
                category.OnCycle();
            }
        }

        private void SortActiveRooms()
        {
            orderedActiveRooms = activeRooms.OrderByDescending(t => t.Value).Take(40);
        }

        private void SortVotedRooms()
        {
            orderedVotedRooms = votedRooms.Take(40).OrderByDescending(t => t.Value);
        }

        private bool WorkActiveRoomsUpdateQueue()
        {
            if (activeRoomsUpdateQueue.Count > 0)
            {
                lock (activeRoomsUpdateQueue.SyncRoot)
                {
                    while (activeRoomsUpdateQueue.Count > 0)
                    {
                        RoomData data = (RoomData)activeRoomsUpdateQueue.Dequeue();
                        if (!activeRooms.ContainsKey(data))
                            activeRooms.Add(data, data.UsersNow);
                        else
                            activeRooms[data] = data.UsersNow;
                    }
                }
                return true;
            }
            return false;
        }

        private bool WorkActiveRoomsAddQueue()
        {
            if (activeRoomsAddQueue.Count > 0)
            {
                lock (activeRoomsAddQueue.SyncRoot)
                {
                    while (activeRoomsAddQueue.Count > 0)
                    {
                        RoomData data = (RoomData)activeRoomsAddQueue.Dequeue();
                        if (!activeRooms.ContainsKey(data))
                            activeRooms.Add(data, data.UsersNow);
                    }
                }
                return true;
            }
            return false;
        }

        private bool WorkActiveRoomsRemoveQueue()
        {
            if (activeRoomsRemoveQueue.Count > 0)
            {
                lock (activeRoomsRemoveQueue.SyncRoot)
                {
                    while (activeRoomsRemoveQueue.Count > 0)
                    {
                        RoomData data = (RoomData)activeRoomsRemoveQueue.Dequeue();
                        activeRooms.Remove(data);
                    }
                }
                return true;
            }
            return false;
        }

        private bool WorkVotedRoomsAddQueue()
        {
            if (votedRoomsAddQueue.Count > 0)
            {
                lock (votedRoomsAddQueue.SyncRoot)
                {
                    while (votedRoomsAddQueue.Count > 0)
                    {
                        RoomData data = (RoomData)votedRoomsAddQueue.Dequeue();
                        if (!votedRooms.ContainsKey(data))
                            votedRooms.Add(data, data.Score);
                        else
                            votedRooms[data] = data.Score;
                    }
                }
                return true;
            }
            return false;
        }

        private bool WorkVotedRoomsRemoveQueue()
        {
            if (votedRoomsRemoveQueue.Count > 0)
            {
                lock (votedRoomsRemoveQueue.SyncRoot)
                {
                    while (votedRoomsRemoveQueue.Count > 0)
                    {
                        RoomData data = (RoomData)votedRoomsRemoveQueue.Dequeue();
                        votedRooms.Remove(data);
                    }
                }
                return true;
            }
            return false;
        }


        private void WorkRoomsToAddQueue()
        {
            if (roomsToAddQueue.Count > 0)
            {
                lock (roomsToAddQueue.SyncRoot)
                {
                    while (roomsToAddQueue.Count > 0)
                    {
                        Room room = (Room)roomsToAddQueue.Dequeue();
                        if (!loadedRooms.ContainsKey(room.RoomId))
                            loadedRooms.Add(room.RoomId, room);
                    }
                }
            }
        }

        private void WorkRoomsToRemoveQueue()
        {
            if (roomsToRemoveQueue.Count > 0)
            {
                lock (roomsToRemoveQueue.SyncRoot)
                {
                    while (roomsToRemoveQueue.Count > 0)
                    {
                        uint roomID = (uint)roomsToRemoveQueue.Dequeue();
                        loadedRooms.Remove(roomID);
                    }
                }
            }
        }

        private void WorkRoomDataQueue()
        {
            if (roomDataToAddQueue.Count > 0)
            {
                lock (roomDataToAddQueue.SyncRoot)
                {
                    while (roomDataToAddQueue.Count > 0)
                    {
                        RoomData data = (RoomData)roomDataToAddQueue.Dequeue();
                        if (!loadedRooms.ContainsKey(data.Id))
                            loadedRoomData.Add(data.Id, data);
                    }
                }
            }
        }

        internal static bool roomCyclingEnabled = true;
        private DateTime cycleLastExecution;
        internal void RoomCycleTask()
        {
            TimeSpan sinceLastTime = DateTime.Now - cycleLastExecution;

            if (sinceLastTime.TotalMilliseconds >= 500 && roomCyclingEnabled)
            {
                cycleLastExecution = DateTime.Now;

                foreach (Room Room in loadedRooms.Values)
                {
                    if (Room.mIsIdle)
                    {
                        loadedRooms.Remove(Room.RoomId);
                        mToRemove.Add(Room);
                    }
                    else if (!Room.isCycling)
                    {
                        ThreadPool.UnsafeQueueUserWorkItem(Room.ProcessRoom, null);
                    }
                }
            }
        }

        private static DateTime removeLastExecution;
        internal void RemoveTask()
        {
            TimeSpan sinceLastTime = DateTime.Now - removeLastExecution;

            if (sinceLastTime.TotalMilliseconds >= 5000)
            {
                removeLastExecution = DateTime.Now;
                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    foreach (Room room in mToRemove)
                    {
                        Logging.WriteLine("[RoomMgr] Requesting unload of idle room - ID#: " + room.RoomId);

                        room.GetRoomItemHandler().SaveFurniture(dbClient);
                        UnloadRoom(room);
                    }
                }
                mToRemove.Clear();
            }
        }
        #endregion

        #region Methods

        internal void QueueVoteAdd(RoomData data)
        {
            lock (votedRoomsAddQueue.SyncRoot)
            {
                votedRoomsAddQueue.Enqueue(data);
            }
        }

        internal void QueueVoteRemove(RoomData data)
        {
            lock (votedRoomsRemoveQueue.SyncRoot)
            {
                votedRoomsRemoveQueue.Enqueue(data);
            }
        }

        internal void QueueActiveRoomUpdate(RoomData data)
        {
            lock (activeRoomsUpdateQueue.SyncRoot)
            {
                activeRoomsUpdateQueue.Enqueue(data);
            }

            if (!roomCategories.ContainsKey(data.Category))
                roomCategories.Add(data.Category, new RoomCategory(data.Category, "MISSING CATEGORY"));
                roomCategories[data.Category].QueueUpdateActiveRooms(data);
        }

        internal void QueueActiveRoomAdd(RoomData data)
        {
            lock (activeRoomsAddQueue.SyncRoot)
            {
                activeRoomsAddQueue.Enqueue(data);
            }

            if (!roomCategories.ContainsKey(data.Category))
                roomCategories.Add(data.Category, new RoomCategory(data.Category, "MISSING CATEGORY"));
            roomCategories[data.Category].QueueAddActiveRooms(data);
        }

        internal void QueueActiveRoomRemove(RoomData data)
        {
            lock (activeRoomsRemoveQueue.SyncRoot)
            {
                activeRoomsRemoveQueue.Enqueue(data);
            }

            if (!roomCategories.ContainsKey(data.Category))
                roomCategories.Add(data.Category, new RoomCategory(data.Category, "MISSING CATEGORY"));
                roomCategories[data.Category].QueueRemoveActiveRooms(data);
        }

        internal void RemoveAllRooms()
        {
            int length = loadedRooms.Count;
            int i = 0;
            foreach (Room Room in loadedRooms.Values)
            {
                PiciEnvironment.GetGame().GetRoomManager().UnloadRoom(Room);
                Console.Clear();
                Console.WriteLine("<<- SERVER SHUTDOWN ->> ROOM ITEM SAVE: " + String.Format("{0:0.##}", ((double)i / length) * 100) + "%");
                i++;
            }
            Console.WriteLine("Done disposing rooms!");
        }

        internal void UnloadRoom(Room Room)
        {
            if (Room == null)
            {
                return;
            }

            lock (roomsToRemoveQueue.SyncRoot)
            {
                roomsToRemoveQueue.Enqueue(Room.RoomId);
            }

            Room.Destroy();

           Logging.WriteLine("[RoomMgr] Unloaded room: \"" + Room.Name + "\" (ID: " + Room.RoomId + ")");
        }

        #endregion
    }

    class TeleUserData
    {
        private UInt32 RoomId;
        private UInt32 TeleId;

        private GameClientMessageHandler mHandler;
        private Habbo mUserRefference;

        internal TeleUserData(GameClientMessageHandler pHandler, Habbo pUserRefference, UInt32 RoomId, UInt32 TeleId)
        {
            //this.User = User;
            this.mHandler = pHandler;
            this.mUserRefference = pUserRefference;
            this.RoomId = RoomId;
            this.TeleId = TeleId;
        }

        internal void Execute()
        {
            if (mHandler == null || mUserRefference == null)
            {
                return;
            }

            mUserRefference.IsTeleporting = true;
            mUserRefference.TeleporterId = TeleId;
            mHandler.PrepareRoomForUser(RoomId, "");
        }
    }
}
