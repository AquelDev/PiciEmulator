using System;
using System.Data;
using Butterfly.HabboHotel.Misc;
using Butterfly.HabboHotel.Rooms;
using Butterfly.HabboHotel.Users.Badges;
using Database_Manager.Database.Session_Details.Interfaces;

namespace Butterfly.Messages
{
    partial class GameClientMessageHandler
    {
        internal void GetUserInfo()
        {
            GetResponse().Init(5);
            GetResponse().AppendStringWithBreak(Session.GetHabbo().Id.ToString());
            GetResponse().AppendStringWithBreak(Session.GetHabbo().Username);
            GetResponse().AppendStringWithBreak(Session.GetHabbo().Look);
            GetResponse().AppendStringWithBreak(Session.GetHabbo().Gender.ToUpper());
            GetResponse().AppendStringWithBreak(Session.GetHabbo().Motto);
            GetResponse().AppendStringWithBreak(Session.GetHabbo().RealName);
            GetResponse().AppendInt32(0);
            GetResponse().AppendStringWithBreak("ch=s01/250,56,49"); // hmm. << SWIMMING @E
            GetResponse().AppendInt32(0);
            GetResponse().AppendInt32(0);
            GetResponse().AppendInt32(Session.GetHabbo().Respect);
            GetResponse().AppendInt32(Session.GetHabbo().DailyRespectPoints); // respect to give away
            GetResponse().AppendInt32(Session.GetHabbo().DailyPetRespectPoints);
            SendResponse();
        }

        internal void GetBalance()
        {
            Session.GetHabbo().UpdateCreditsBalance();
            Session.GetHabbo().UpdateActivityPointsBalance(false);
        }

        internal void GetSubscriptionData()
        {
            string SubscriptionId = Request.PopFixedString();

            GetResponse().Init(7);
            GetResponse().AppendStringWithBreak(SubscriptionId.ToLower());

            if (Session.GetHabbo().GetSubscriptionManager().HasSubscription(SubscriptionId))
            {
                Double Expire = Session.GetHabbo().GetSubscriptionManager().GetSubscription(SubscriptionId).ExpireTime;
                Double TimeLeft = Expire - ButterflyEnvironment.GetUnixTimestamp();
                int TotalDaysLeft = (int)Math.Ceiling(TimeLeft / 86400);
                int MonthsLeft = TotalDaysLeft / 31;

                if (MonthsLeft >= 1) MonthsLeft--;

                GetResponse().AppendInt32(TotalDaysLeft - (MonthsLeft * 31));
                GetResponse().AppendBoolean(true);
                GetResponse().AppendInt32(MonthsLeft);

                if (Session.GetHabbo().Rank >= 2)
                {
                    GetResponse().AppendInt32(1);
                    GetResponse().AppendInt32(1);
                    GetResponse().AppendInt32(2);
                }
                else
                {
                    GetResponse().AppendInt32(1);
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    GetResponse().AppendInt32(0);
                }
            }

            SendResponse();
        }

        internal void GetBadges()
        {
            Session.SendMessage(Session.GetHabbo().GetBadgeComponent().Serialize());
        }

        internal void UpdateBadges()
        {
            Session.GetHabbo().GetBadgeComponent().ResetSlots();

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.runFastQuery("UPDATE user_badges SET badge_slot = 0 WHERE user_id = " + Session.GetHabbo().Id);
            }

            if (Request.RemainingLength > 0)
            {
                while (Request.RemainingLength > 0)
                {
                    int Slot = Request.PopWiredInt32();
                    string Badge = Request.PopFixedString();

                    if (Badge.Length == 0)
                    {
                        continue;
                    }

                    if (!Session.GetHabbo().GetBadgeComponent().HasBadge(Badge) || Slot < 1 || Slot > 5)
                    {
                        return;
                    }

                    Session.GetHabbo().GetBadgeComponent().GetBadge(Badge).Slot = Slot;

                    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        dbClient.setQuery("UPDATE user_badges SET badge_slot = " + Slot + " WHERE badge_id = @badge AND user_id = " + Session.GetHabbo().Id + "");
                        dbClient.addParameter("badge", Badge);
                        dbClient.runQuery();
                    }
                }

                ButterflyEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, HabboHotel.Quests.QuestType.PROFILE_BADGE);
            }

            ServerMessage Message = new ServerMessage(228);
            Message.AppendUInt(Session.GetHabbo().Id);
            Message.AppendInt32(Session.GetHabbo().GetBadgeComponent().EquippedCount);

            foreach (Badge Badge in Session.GetHabbo().GetBadgeComponent().BadgeList.Values)
            {
                if (Badge.Slot <= 0)
                {
                    continue;
                }

                Message.AppendInt32(Badge.Slot);
                Message.AppendStringWithBreak(Badge.Code);
            }

            if (Session.GetHabbo().InRoom && ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId) != null)
            {
                ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId).SendMessage(Message);
            }
            else
            {
                Session.SendMessage(Message);
            }
        }

        internal void GetAchievements()
        {
            ButterflyEnvironment.GetGame().GetAchievementManager().GetList(Session, Request);
        }

        internal void ChangeLook()
        {
            if (Session.GetHabbo().MutantPenalty)
            {
                Session.SendNotif("Because of a penalty or restriction on your account, you are not allowed to change your look.");
                return;
            }

            string Gender = Request.PopFixedString().ToUpper();
            string Look = ButterflyEnvironment.FilterInjectionChars(Request.PopFixedString());

            if (!AntiMutant.ValidateLook(Look, Gender))
            {
                return;
            }

            ButterflyEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, HabboHotel.Quests.QuestType.PROFILE_CHANGE_LOOK);

            Session.GetHabbo().Look = ButterflyEnvironment.FilterFigure(Look);
            Session.GetHabbo().Gender = Gender.ToLower();

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("UPDATE users SET look = @look, gender = @gender WHERE id = " + Session.GetHabbo().Id);
                dbClient.addParameter("look", Look);
                dbClient.addParameter("gender", Gender);
                dbClient.runQuery();
            }

            ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(Session, "ACH_AvatarLooks", 1);

            Session.GetMessageHandler().GetResponse().Init(266);
            Session.GetMessageHandler().GetResponse().AppendInt32(-1);
            Session.GetMessageHandler().GetResponse().AppendStringWithBreak(Session.GetHabbo().Look);
            Session.GetMessageHandler().GetResponse().AppendStringWithBreak(Session.GetHabbo().Gender.ToLower());
            Session.GetMessageHandler().GetResponse().AppendStringWithBreak(Session.GetHabbo().Motto);
            Session.GetMessageHandler().SendResponse();

            if (Session.GetHabbo().InRoom)
            {
                Room Room = Session.GetHabbo().CurrentRoom;

                if (Room == null)
                {
                    return;
                }

                RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);

                if (User == null)
                {
                    return;
                }

                ServerMessage RoomUpdate = new ServerMessage(266);
                RoomUpdate.AppendInt32(User.VirtualId);
                RoomUpdate.AppendStringWithBreak(Session.GetHabbo().Look);
                RoomUpdate.AppendStringWithBreak(Session.GetHabbo().Gender.ToLower());
                RoomUpdate.AppendStringWithBreak(Session.GetHabbo().Motto);
                Room.SendMessage(RoomUpdate);
            }
        }

        internal void ChangeMotto()
        {
            string Motto = ButterflyEnvironment.FilterInjectionChars(Request.PopFixedString());

            if (Motto == Session.GetHabbo().Motto) // Prevents spam?
            {
                return;
            }

            //if (Motto.Length < 0)
            //{
            //    return; // trying to fk the client :D
            //} Congratulations. The string length can not hold calue < 0. Stupid -_-"

            Session.GetHabbo().Motto = Motto;


            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("UPDATE users SET motto = @motto WHERE id = '" + Session.GetHabbo().Id + "'");
                dbClient.addParameter("motto", Motto);
                dbClient.runQuery();
            }

            ButterflyEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, HabboHotel.Quests.QuestType.PROFILE_CHANGE_MOTTO);

            if (Session.GetHabbo().InRoom)
            {
                Room Room = Session.GetHabbo().CurrentRoom;

                if (Room == null)
                {
                    return;
                }

                RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);

                if (User == null)
                {
                    return;
                }

                ServerMessage RoomUpdate = new ServerMessage(266);
                RoomUpdate.AppendInt32(User.VirtualId);
                RoomUpdate.AppendStringWithBreak(Session.GetHabbo().Look);
                RoomUpdate.AppendStringWithBreak(Session.GetHabbo().Gender.ToLower());
                RoomUpdate.AppendStringWithBreak(Session.GetHabbo().Motto);
                RoomUpdate.AppendInt32(Session.GetHabbo().AchievementPoints);
                Room.SendMessage(RoomUpdate);
            }

            ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(Session, "ACH_Motto", 1);
        }

        internal void GetWardrobe()
        {
            GetResponse().Init(267);
            GetResponse().AppendBoolean(Session.GetHabbo().HasFuse("fuse_use_wardrobe"));

            if (Session.GetHabbo().HasFuse("fuse_use_wardrobe"))
            {
                using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    //dbClient.addParameter("userid", Session.GetHabbo().Id);
                    dbClient.setQuery("SELECT slot_id, look, gender FROM user_wardrobe WHERE user_id = " + Session.GetHabbo().Id);
                    DataTable WardrobeData = dbClient.getTable();

                    if (WardrobeData == null)
                    {
                        GetResponse().AppendInt32(0);
                    }
                    else
                    {
                        GetResponse().AppendInt32(WardrobeData.Rows.Count);

                        foreach (DataRow Row in WardrobeData.Rows)
                        {
                            GetResponse().AppendUInt(Convert.ToUInt32(Row["slot_id"]));
                            GetResponse().AppendStringWithBreak((string)Row["look"]);
                            GetResponse().AppendStringWithBreak((string)Row["gender"]);
                        }
                    }
                }

                SendResponse();
            }
        }

        internal void SaveWardrobe()
        {
            uint SlotId = Request.PopWiredUInt();

            string Look = Request.PopFixedString();
            string Gender = Request.PopFixedString();

            if (!AntiMutant.ValidateLook(Look, Gender))
            {
                return;
            }

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT null FROM user_wardrobe WHERE user_id = " + Session.GetHabbo().Id + " AND slot_id = " + SlotId + "");
                dbClient.addParameter("look", Look);
                dbClient.addParameter("gender", Gender.ToUpper());

                if (dbClient.getRow() != null)
                {
                    dbClient.setQuery("UPDATE user_wardrobe SET look = @look, gender = @gender WHERE user_id = " + Session.GetHabbo().Id + " AND slot_id = " + SlotId + ";");
                    dbClient.addParameter("look", Look);
                    dbClient.addParameter("gender", Gender.ToUpper());
                    dbClient.runQuery();
                }
                else
                {
                    dbClient.setQuery("INSERT INTO user_wardrobe (user_id,slot_id,look,gender) VALUES (" + Session.GetHabbo().Id + "," + SlotId + ",@look,@gender)");
                    dbClient.addParameter("look", Look);
                    dbClient.addParameter("gender", Gender.ToUpper());
                    dbClient.runQuery();
                }
            }
        }

        internal void GetPetsInventory()
        {
            if (Session.GetHabbo().GetInventoryComponent() == null)
            {
                return;
            }

            Session.SendMessage(Session.GetHabbo().GetInventoryComponent().SerializePetInventory());
        }


        //internal void RegisterUsers()
        //{
        //    RequestHandlers.Add(7, new RequestHandler(GetUserInfo));
        //    RequestHandlers.Add(8, new RequestHandler(GetBalance));
        //    RequestHandlers.Add(26, new RequestHandler(GetSubscriptionData));

        //    RequestHandlers.Add(157, new RequestHandler(GetBadges));
        //    RequestHandlers.Add(158, new RequestHandler(UpdateBadges));
        //    RequestHandlers.Add(370, new RequestHandler(GetAchievements));

        //    RequestHandlers.Add(44, new RequestHandler(ChangeLook));
        //    RequestHandlers.Add(484, new RequestHandler(ChangeMotto));
        //    RequestHandlers.Add(375, new RequestHandler(GetWardrobe));
        //    RequestHandlers.Add(376, new RequestHandler(SaveWardrobe));

        //    RequestHandlers.Add(404, new RequestHandler(GetInventory));
        //    RequestHandlers.Add(3000, new RequestHandler(GetPetsInventory));

        //}

        //internal void UnregisterUser()
        //{
        //    RequestHandlers.Remove(7);
        //    RequestHandlers.Remove(8);
        //    RequestHandlers.Remove(26);
        //    RequestHandlers.Remove(157);
        //    RequestHandlers.Remove(158);
        //    RequestHandlers.Remove(370);
        //    RequestHandlers.Remove(44);
        //    RequestHandlers.Remove(375);
        //    RequestHandlers.Remove(376);
        //    RequestHandlers.Remove(404);
        //    RequestHandlers.Remove(3000);
        //}
    }
}
