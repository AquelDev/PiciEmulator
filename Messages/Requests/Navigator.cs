using Pici.HabboHotel.Navigators;
using Pici.HabboHotel.Rooms;
using Pici.Core;
using Pici.HabboHotel.Pathfinding;
using System;
using System.Threading;
using Pici.Storage.Database.Session_Details.Interfaces;
using Pici.Messages.Headers;

namespace Pici.Messages
{
    partial class GameClientMessageHandler
    {
        internal void AddFavorite()
        {
            uint Id = Request.PopWiredUInt();

            RoomData Data = PiciEnvironment.GetGame().GetRoomManager().GenerateRoomData(Id);

            if (Data == null || Session.GetHabbo().FavoriteRooms.Count >= 30 || Session.GetHabbo().FavoriteRooms.Contains(Id) || Data.Type == "public")
            {
                GetResponse().Init(33);
                GetResponse().AppendInt32(-9001);
                SendResponse();

                return;
            }

            GetResponse().Init(459);
            GetResponse().AppendUInt(Id);
            GetResponse().AppendBoolean(true);
            SendResponse();

            Session.GetHabbo().FavoriteRooms.Add(Id);

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.runFastQuery("INSERT INTO user_favorites (user_id,room_id) VALUES (" + Session.GetHabbo().Id + "," + Id + ")");
            }
        }

        internal void RemoveFavorite()
        {
            uint Id = Request.PopWiredUInt();

            Session.GetHabbo().FavoriteRooms.Remove(Id);

            GetResponse().Init(459);
            GetResponse().AppendUInt(Id);
            GetResponse().AppendBoolean(false);
            SendResponse();

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.runFastQuery("DELETE FROM user_favorites WHERE user_id = " + Session.GetHabbo().Id + " AND room_id = " + Id + "");
            }
        }

        internal void GoToHotelView()
        {
            if (Session.GetHabbo().InRoom)
            {
                Room currentRoom = PiciEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                if (currentRoom != null)
                    currentRoom.GetRoomUserManager().RemoveUserFromRoom(Session, true, false);
                Session.CurrentRoomUserID = -1;
            }
        }

        internal void GetFlatCats()
        {
            Session.SendMessage(PiciEnvironment.GetGame().GetNavigator().SerializeFlatCategories());
        }

        internal void EnterInquiredRoom()
        {
            // ???????????????????????????????
            // I like cake :D
        }

        internal void GetPubs()
        {
            Session.SendMessage(PiciEnvironment.GetGame().GetNavigator().SerializePublicRooms());
        }

        internal void GetRoomInfo()
        {
            try
            {

                uint RoomId = Request.PopWiredUInt();
                bool LoadingState = Request.PopWiredBoolean();
                bool Following = Request.PopWiredBoolean();

                RoomData Data = PiciEnvironment.GetGame().GetRoomManager().GenerateRoomData(RoomId);

                if (Data == null)
                {
                    return;
                }


                /*if (Session.GetHabbo().NewbieStatus == 1)
                {
                    ServerMessage VoteMessage = new ServerMessage(MessageComposerIds.VoteQuestionMessageComposer);
                    VoteMessage.Append("Do you like the hotel?");
                    VoteMessage.Append(4);
                    VoteMessage.Append(1);
                    VoteMessage.Append("fuck no");
                    VoteMessage.Append(2);
                    VoteMessage.Append("FUCKING NO");
                    VoteMessage.Append(3);
                    VoteMessage.Append("FKS SAKE");
                    VoteMessage.Append(4);
                    VoteMessage.Append("Never!");
                    PiciEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(VoteMessage);


                    using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        dbClient.runFastQuery("UPDATE users SET newbie_status = 0 WHERE id = " + Session.GetHabbo().Id);

                    }
                }*/

                GetResponse().Init(454);
                GetResponse().AppendBoolean(LoadingState);
                Data.Serialize(GetResponse(), false);
                GetResponse().AppendBoolean(Following);
                GetResponse().AppendBoolean(LoadingState);
                SendResponse();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        internal void GetPopularRooms()
        {
            Session.SendMessage(PiciEnvironment.GetGame().GetNavigator().SerializeNavigator(Session, Request.PopFixedInt32()));
        }

        internal void GetHighRatedRooms()
        {
            Session.SendMessage(PiciEnvironment.GetGame().GetNavigator().SerializeNavigator(Session, -2));
        }

        internal void GetFriendsRooms()
        {
            Session.SendMessage(PiciEnvironment.GetGame().GetNavigator().SerializeNavigator(Session, -4));
        }

        internal void GetRoomsWithFriends()
        {
            Session.SendMessage(PiciEnvironment.GetGame().GetNavigator().SerializeNavigator(Session, -5));
        }

        internal void GetOwnRooms()
        {
            Session.SendMessage(PiciEnvironment.GetGame().GetNavigator().SerializeNavigator(Session, -3));
        }

        internal void GetFavoriteRooms()
        {
            Session.SendMessage(PiciEnvironment.GetGame().GetNavigator().SerializeFavoriteRooms(Session));
        }

        internal void GetRecentRooms()
        {
            Session.SendMessage(PiciEnvironment.GetGame().GetNavigator().SerializeRecentRooms(Session));
        }

        internal void GetEvents()
        {
            int Category = int.Parse(Request.PopFixedString());

            Session.SendMessage(PiciEnvironment.GetGame().GetNavigator().SerializeEventListing(Category));
        }

        internal void GetPopularTags()
        {
            Session.SendMessage(PiciEnvironment.GetGame().GetNavigator().SerializePopularRoomTags());
        }

        internal void PerformSearch()
        {
            Session.SendMessage(PiciEnvironment.GetGame().GetNavigator().SerializeSearchResults(Request.PopFixedString()));
        }

        internal void PerformSearch2()
        {
            int junk = Request.PopWiredInt32();
            Session.SendMessage(PiciEnvironment.GetGame().GetNavigator().SerializeSearchResults(Request.PopFixedString()));
        }

        internal void OpenFlat()
        {
            uint Id = Request.PopWiredUInt();
            string Password = Request.PopFixedString();
            int Junk = Request.PopWiredInt32();

            Logging.WriteLine("Loading room [" + Id + "]");

            RoomData Data = PiciEnvironment.GetGame().GetRoomManager().GenerateRoomData(Id);

            if (Data == null || Data.Type != "private")
                return;

            PrepareRoomForUser(Id, Password);
        }

        //internal void RegisterNavigator()
        //{
        //    RequestHandlers.Add(391, new RequestHandler(OpenFlat));
        //    RequestHandlers.Add(19, new RequestHandler(AddFavorite));
        //    RequestHandlers.Add(20, new RequestHandler(RemoveFavorite));
        //    RequestHandlers.Add(53, new RequestHandler(GoToHotelView));
        //    RequestHandlers.Add(151, new RequestHandler(GetFlatCats));
        //    RequestHandlers.Add(233, new RequestHandler(EnterInquiredRoom));
        //    RequestHandlers.Add(380, new RequestHandler(GetPubs));
        //    RequestHandlers.Add(385, new RequestHandler(GetRoomInfo));
        //    RequestHandlers.Add(430, new RequestHandler(GetPopularRooms));
        //    RequestHandlers.Add(431, new RequestHandler(GetHighRatedRooms));
        //    RequestHandlers.Add(432, new RequestHandler(GetFriendsRooms));
        //    RequestHandlers.Add(433, new RequestHandler(GetRoomsWithFriends));
        //    RequestHandlers.Add(434, new RequestHandler(GetOwnRooms));
        //    RequestHandlers.Add(435, new RequestHandler(GetFavoriteRooms));
        //    RequestHandlers.Add(436, new RequestHandler(GetRecentRooms));
        //    RequestHandlers.Add(439, new RequestHandler(GetEvents));
        //    RequestHandlers.Add(382, new RequestHandler(GetPopularTags));
        //    RequestHandlers.Add(437, new RequestHandler(PerformSearch));
        //    RequestHandlers.Add(438, new RequestHandler(PerformSearch2));
        //}

        //internal void UnregisterNavigator()
        //{
        //    RequestHandlers.Remove(391);
        //    RequestHandlers.Remove(19);
        //    RequestHandlers.Remove(20);
        //    RequestHandlers.Remove(53);
        //    RequestHandlers.Remove(151);
        //    RequestHandlers.Remove(233);
        //    RequestHandlers.Remove(380);
        //    RequestHandlers.Remove(385);
        //    RequestHandlers.Remove(430);
        //    RequestHandlers.Remove(431);
        //    RequestHandlers.Remove(432);
        //    RequestHandlers.Remove(433);
        //    RequestHandlers.Remove(434);
        //    RequestHandlers.Remove(435);
        //    RequestHandlers.Remove(436);
        //    RequestHandlers.Remove(439);
        //    RequestHandlers.Remove(382);
        //    RequestHandlers.Remove(437);
        //    RequestHandlers.Remove(438);
        //}
    }
}
