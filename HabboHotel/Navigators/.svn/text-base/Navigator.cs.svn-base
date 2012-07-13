using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Butterfly.HabboHotel.GameClients;
using Butterfly.HabboHotel.Rooms;
using Butterfly.HabboHotel.Users.Messenger;
using Butterfly.Messages;
using Database_Manager.Database.Session_Details.Interfaces;

namespace Butterfly.HabboHotel.Navigators
{
    class Navigator
    {
        //private Dictionary<int, string> PublicCategories;
        private Hashtable PrivateCategories;

        private Dictionary<Int32, PublicItem> PublicItems;

        internal Navigator()
        {
            //PublicCategories = new Dictionary<int, string>();
            PrivateCategories = new Hashtable();
            PublicItems = new Dictionary<int, PublicItem>();
            //PublicRecommended = new Dictionary<int, PublicItem>();
        }

        internal void Initialize(IQueryAdapter dbClient)
        {
            //dbClient.setQuery("SELECT id,caption FROM navigator_pubcats WHERE enabled = 2"); //wtf?
            //DataTable dPubCats = dbClient.getTable();

            dbClient.setQuery("SELECT id,caption,min_rank FROM navigator_flatcats WHERE enabled = 2");
            DataTable dPrivCats = dbClient.getTable();

            dbClient.setQuery("SELECT * FROM navigator_publics ORDER BY ordernum ASC");
            DataTable dPubItems = dbClient.getTable();

            //if (dPubCats != null)
            //{
            //    foreach (DataRow Row in dPubCats.Rows)
            //    {
            //        PublicCategories.Add((int)Row["id"], (string)Row["caption"]);
            //    }
            //}

            if (dPrivCats != null)
            {
                foreach (DataRow Row in dPrivCats.Rows)
                {
                    PrivateCategories.Add((int)Row["id"], new FlatCat((int)Row["id"], (string)Row["caption"], (int)Row["min_rank"]));
                }
            }

            if (dPubItems != null)
            {
                foreach (DataRow Row in dPubItems.Rows)
                {
                    PublicItems.Add((int)Row["id"], new PublicItem((int)Row["id"], int.Parse(Row["bannertype"].ToString()), (string)Row["caption"],
                        (string)Row["image"], ((Row["image_type"].ToString().ToLower() == "internal") ? PublicImageType.INTERNAL : PublicImageType.EXTERNAL),
                        Convert.ToUInt32(Row["room_id"]), (int)Row["category_parent_id"], ButterflyEnvironment.EnumToBool(Row["category"].ToString()) ,ButterflyEnvironment.EnumToBool(Row["recommended"].ToString())));
                }
            }

            //if (dPubRecommended != null)
            //{
            //    foreach (DataRow Row in dPubRecommended.Rows)
            //    {
            //        PublicRecommended.Add((int)Row["id"], new PublicItem((int)Row["id"], int.Parse(Row["bannertype"].ToString()), (string)Row["caption"],
            //            (string)Row["image"], ((Row["image_type"].ToString().ToLower() == "internal") ? PublicImageType.INTERNAL : PublicImageType.EXTERNAL),
            //            (uint)Row["room_id"], (int)Row["category_parent_id"], false));
            //    }
            //}
        }

        internal Int32 GetCountForParent(Int32 ParentId)
        {
            int i = 0;

            foreach (PublicItem Item in PublicItems.Values)
            {
                if (Item.ParentId == ParentId || ParentId == -1)
                {
                    i++;
                }
            }

            return i;
        }

        internal FlatCat GetFlatCat(Int32 Id)
        {
            if (PrivateCategories.ContainsKey(Id))
                return (FlatCat)PrivateCategories[Id];

            return null;
        }

        internal ServerMessage SerializeFlatCategories()
        {
            ServerMessage Cats = new ServerMessage(221);
            Cats.AppendInt32(PrivateCategories.Count);

            foreach (FlatCat FlatCat in PrivateCategories.Values)
            {
                if (FlatCat.Id > 0)
                {
                    Cats.AppendBoolean(true);
                }

                Cats.AppendInt32(FlatCat.Id);
                Cats.AppendStringWithBreak(FlatCat.Caption);
            }

            Cats.AppendStringWithBreak("");

            return Cats;
        }

        internal ServerMessage SerializePublicRooms()
        {
            ServerMessage Frontpage = new ServerMessage(450);
            Frontpage.AppendInt32(this.PublicItems.Count);

            foreach (PublicItem Pub in PublicItems.Values)
            {
                if (Pub.Category)
                {
                    Pub.Serialize(Frontpage);
                    this.SerializeItemsFromCata(Pub.Id, Frontpage);
                }

                if (!Pub.Category && Pub.ParentId == 0)
                {
                    Pub.Serialize(Frontpage);
                }
            }

            return Frontpage;
        }

        private void SerializeItemsFromCata(int Id, ServerMessage Message)
        {
            foreach (PublicItem Item in this.PublicItems.Values)
            {
                if (Item.ParentId == Id && !Item.Category)
                {
                    Item.Serialize(Message);
                }
            }
        }

        internal ServerMessage SerializeFavoriteRooms(GameClient Session)
        {
            ServerMessage Rooms = new ServerMessage(451);
            Rooms.AppendInt32(0);
            Rooms.AppendInt32(6);
            Rooms.AppendStringWithBreak("");
            Rooms.AppendInt32(Session.GetHabbo().FavoriteRooms.Count);

            RoomData room;
            foreach (uint Id in Session.GetHabbo().FavoriteRooms.ToArray())
            {
                room = ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData(Id);
                if (room != null)
                    room.Serialize(Rooms, false);
            }

            return Rooms;
        }
       
        internal ServerMessage SerializeRecentRooms(GameClient Session)
        {
            //using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            //{
            //    dbClient.setQuery("SELECT rooms.id, rooms.caption, rooms.description, rooms.roomtype, rooms.owner, rooms.state, rooms.category, rooms.users_now, rooms.users_max, rooms.model_name, rooms.public_ccts, rooms.score, rooms.allow_pets, rooms.allow_pets_eat, rooms.allow_walkthrough, rooms.allow_hidewall, rooms.password, rooms.wallpaper, rooms.floor, rooms.landscape, rooms.icon_items, rooms.icon_bg, rooms.icon_fg, rooms.tags, rooms.allow_rightsoverride FROM rooms JOIN user_roomvisits ON rooms.id = user_roomvisits.room_id WHERE user_roomvisits.user_id = " + Session.GetHabbo().Id + " ORDER BY entry_timestamp DESC LIMIT 50;");
            //    DataTable Data = dbClient.getTable();

            //    List<RoomData> ValidRecentRooms = new List<RoomData>();
            //    List<uint> RoomsListed = new List<uint>();

            //    if (Data != null)
            //    {
            //        foreach (DataRow Row in Data.Rows)
            //        {

            //            RoomData tData = ButterflyEnvironment.GetGame().GetRoomManager().FetchRoomData((uint)Row["id"], Row);
            //            tData.Fill(Row);
            //            ValidRecentRooms.Add(tData);
            //            RoomsListed.Add(tData.Id);
            //        }
            //    }

                ServerMessage Rooms = new ServerMessage(451);
                Rooms.AppendInt32(0);
                Rooms.AppendInt32(7);
                Rooms.AppendStringWithBreak("");
                //Rooms.AppendInt32(ValidRecentRooms.Count);
                Rooms.AppendInt32(0);

            //    foreach (RoomData _Data in ValidRecentRooms)
            //    {
            //        _Data.Serialize(Rooms, false);
            //    }

                return Rooms;
            //}
        }

        internal ServerMessage SerializeEventListing(int Category)
        {
            ServerMessage Message = new ServerMessage(451);
            Message.AppendInt32(Category);
            Message.AppendInt32(12);
            Message.AppendStringWithBreak("");

            KeyValuePair<RoomData, int>[] eventRooms = ButterflyEnvironment.GetGame().GetRoomManager().GetEventRoomsForCategory(Category);
            Message.AppendInt32(eventRooms.Length);

            foreach (KeyValuePair<RoomData, int> pair in eventRooms)
            {
                pair.Key.Serialize(Message, true);
            }

            return Message;
        }

        internal ServerMessage SerializePopularRoomTags()
        {
            Dictionary<string, int> Tags = new Dictionary<string, int>();

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (dbClient.dbType == Database_Manager.Database.DatabaseType.MySQL)
                    dbClient.setQuery("SELECT rooms.tags, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE roomtype = 'private' AND active_users > 0 ORDER BY active_users DESC LIMIT 50");
                else
                    dbClient.setQuery("SELECT TOP 50 rooms.tags, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE roomtype = 'private' AND active_users > 0 ORDER BY active_users DESC");
                DataTable Data = dbClient.getTable();


                if (Data != null)
                {
                    int activeUsers;
                    foreach (DataRow Row in Data.Rows)
                    {
                        if (!string.IsNullOrEmpty(Row["active_users"].ToString()))
                            activeUsers = (int)Row["active_users"];
                        else
                            activeUsers = 0;

                        List<string> RoomTags = new List<string>();

                        foreach (string Tag in Row["tags"].ToString().Split(','))
                        {
                            RoomTags.Add(Tag);
                        }

                        foreach (string Tag in RoomTags)
                        {
                            if (Tags.ContainsKey(Tag))
                            {
                                Tags[Tag] += activeUsers;
                            }
                            else
                            {
                                Tags.Add(Tag, activeUsers);
                            }
                        }
                    }
                }

                List<KeyValuePair<string, int>> SortedTags = new List<KeyValuePair<string, int>>(Tags);

                SortedTags.Sort(

                    delegate(KeyValuePair<string, int> firstPair,

                    KeyValuePair<string, int> nextPair)
                    {
                        return firstPair.Value.CompareTo(nextPair.Value);
                    }

                );

                ServerMessage Message = new ServerMessage(452);
                Message.AppendInt32(SortedTags.Count);

                foreach (KeyValuePair<string, int> TagData in SortedTags)
                {
                    Message.AppendStringWithBreak(TagData.Key);
                    Message.AppendInt32(TagData.Value);
                }

                return Message;
            }
        }

        internal ServerMessage SerializeSearchResults(string SearchQuery)
        {
            DataTable Data = new DataTable();

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (SearchQuery.Length > 0)
                {
                    if (dbClient.dbType == Database_Manager.Database.DatabaseType.MySQL)
                        dbClient.setQuery("SELECT rooms.*, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE owner = @query AND roomtype = 'private' " +
                                    "UNION ALL " +
                                    "SELECT rooms.*, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE caption = @query AND roomtype = 'private' " +
                                    "ORDER BY active_users DESC LIMIT 50");
                    else
                        dbClient.setQuery("SELECT TOP 50 rooms.*, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE owner = @query AND roomtype = 'private' " +
                                    "UNION ALL " +
                                    "SELECT rooms.*, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE caption = @query AND roomtype = 'private' " +
                                    "ORDER BY active_users DESC");
                    dbClient.addParameter("query", SearchQuery);
                    Data = dbClient.getTable();
                }
            }

            List<RoomData> Results = new List<RoomData>();

            if (Data != null)
            {
                foreach (DataRow Row in Data.Rows)
                {
                    RoomData RData = ButterflyEnvironment.GetGame().GetRoomManager().FetchRoomData(Convert.ToUInt32(Row["id"]), Row);
                    Results.Add(RData);
                }
            }

            ServerMessage Message = new ServerMessage(451);
            Message.AppendInt32(1);
            Message.AppendInt32(9);
            Message.AppendStringWithBreak(SearchQuery);
            Message.AppendInt32(Results.Count);

            int i = 0;
            foreach (RoomData Room in Results)
            {
                if (i > 0)
                    Message.AppendInt32(0);
                Room.Serialize(Message, false);
                i++;
            }

            return Message;
        }

        private ServerMessage SerializeActiveRooms(int category)
        {
            ServerMessage reply = new ServerMessage(451);
            reply.AppendInt32(category);
            reply.AppendInt32(1);

            KeyValuePair<RoomData, int>[] rooms = ButterflyEnvironment.GetGame().GetRoomManager().GetRooms(category);
            SerializeNavigatorRooms(ref reply, rooms);

            Array.Clear(rooms, 0, rooms.Length);
            rooms = null;

            return reply;
        }

        internal ServerMessage SerializeNavigator(GameClient session, int mode)
        {
            if (mode > -1)
                return SerializeActiveRooms(mode);

            ServerMessage reply = new ServerMessage(451);

            switch (mode)
            {
                case -5:
                case -4:
                    {
                        reply.AppendInt32(0);
                        reply.AppendInt32(mode * (-1));
                        
                        List<RoomData> activeFriends = session.GetHabbo().GetMessenger().GetActiveFriendsRooms().OrderByDescending(p => p.UsersNow).ToList();
                        SerializeNavigatorRooms(ref reply, activeFriends);

                        return reply;
                    }

                case -3:
                    {
                        reply.AppendInt32(0);
                        reply.AppendInt32(5);

                        SerializeNavigatorRooms(ref reply, session.GetHabbo().UsersRooms);

                        return reply;
                    }

                case -2:
                    {
                        reply.AppendInt32(0);
                        reply.AppendInt32(2);

                        KeyValuePair<RoomData, int>[] rooms = ButterflyEnvironment.GetGame().GetRoomManager().GetVotedRooms();
                        SerializeNavigatorRooms(ref reply, rooms);

                        Array.Clear(rooms, 0, rooms.Length);
                        rooms = null;

                        return reply;
                    }

                case -1:
                    {
                        reply.AppendInt32(-1);
                        reply.AppendInt32(1);

                        KeyValuePair<RoomData, int>[] rooms = ButterflyEnvironment.GetGame().GetRoomManager().GetActiveRooms();

                        SerializeNavigatorRooms(ref reply, rooms);

                        Array.Clear(rooms, 0, rooms.Length);
                        rooms = null;

                        return reply;
                    }
            }

            return reply;
        }

        private void SerializeNavigatorRooms(ref ServerMessage reply, List<RoomData> rooms)
        {
            reply.AppendStringWithBreak(string.Empty);
            reply.AppendInt32(rooms.Count);

            bool headerSerialized = false;
            foreach (RoomData data in rooms)
            {
                if (headerSerialized)
                    reply.AppendInt32(0);
                else
                    headerSerialized = true;
                data.Serialize(reply, false);
            }

            //if (PublicRecommended.Count > 0)
            //{
            //    reply.AppendStringWithBreak("");

            //    foreach (PublicItem Pub in PublicRecommended.Values)
            //    {
            //        Pub.Serialize(reply);
            //    }
            //}
        }

        private void SerializeNavigatorRooms(ref ServerMessage reply, KeyValuePair<RoomData, int>[] rooms)
        {
            reply.AppendStringWithBreak(string.Empty);
            
            reply.AppendInt32(rooms.Length);

            bool headerSerialized = false;
            foreach (KeyValuePair<RoomData,int> pair in rooms)
            {
                if (headerSerialized)
                    reply.AppendInt32(0);
                else
                    headerSerialized = true;
                pair.Key.Serialize(reply, false);
            }

            //if (PublicRecommended.Count > 0)
            //{
            //    reply.AppendStringWithBreak("");

            //    foreach (PublicItem Pub in PublicRecommended.Values)
            //    {
            //        Pub.Serialize(reply);
            //    }
            //}
        }
    }
}
