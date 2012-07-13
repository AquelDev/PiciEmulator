using System;
using Pici.HabboHotel.Pathfinding;
using Pici.HabboHotel.RoomBots;
using Pici.HabboHotel.Rooms;
using Pici.HabboHotel.Support;
using Pici.Core;

namespace Pici.Messages
{
    partial class GameClientMessageHandler
    {
        internal void InitHelpTool()
        {
            Session.SendMessage(PiciEnvironment.GetGame().GetHelpTool().SerializeFrontpage());
        }

        internal void GetHelpCategories()
        {
            Session.SendMessage(PiciEnvironment.GetGame().GetHelpTool().SerializeIndex());
        }

        internal void ViewHelpTopic()
        {
            uint TopicId = Request.PopWiredUInt();

            HelpTopic Topic = PiciEnvironment.GetGame().GetHelpTool().GetTopic(TopicId);

            if (Topic == null)
            {
                return;
            }

            Session.SendMessage(HelpTool.SerializeTopic(Topic));
        }

        internal void SearchHelpTopics()
        {
            string SearchQuery = Request.PopFixedString();

            if (SearchQuery.Length < 3)
            {
                return;
            }

            Session.SendMessage(HelpTool.SerializeSearchResults(SearchQuery));
        }

        internal void GetTopicsInCategory()
        {
            uint Id = Request.PopWiredUInt();

            HelpCategory Category = PiciEnvironment.GetGame().GetHelpTool().GetCategory(Id);

            if (Category == null)
            {
                return;
            }

            Session.SendMessage(PiciEnvironment.GetGame().GetHelpTool().SerializeCategory(Category));
        }

        internal void SubmitHelpTicket()
        {
            Boolean errorOccured = false;

            if (PiciEnvironment.GetGame().GetModerationTool().UsersHasPendingTicket(Session.GetHabbo().Id))
            {
                errorOccured = true;
            }

            if (!errorOccured)
            {
                String Message = PiciEnvironment.FilterInjectionChars(Request.PopFixedString());

                int Junk = Request.PopWiredInt32();
                int Type = Request.PopWiredInt32();
                uint ReportedUser = Request.PopWiredUInt();

                PiciEnvironment.GetGame().GetModerationTool().SendNewTicket(Session, Type, ReportedUser, Message);
            }

            GetResponse().Init(321);
            GetResponse().AppendBoolean(errorOccured);
            SendResponse();
        }

        internal void DeletePendingCFH()
        {
            if (!PiciEnvironment.GetGame().GetModerationTool().UsersHasPendingTicket(Session.GetHabbo().Id))
            {
                return;
            }

            PiciEnvironment.GetGame().GetModerationTool().DeletePendingTicketForUser(Session.GetHabbo().Id);

            GetResponse().Init(320);
            SendResponse();
        }

        internal void ModGetUserInfo()
        {
            if (!Session.GetHabbo().HasRight("acc_supporttool"))
            {
                return;
            }

            uint UserId = Request.PopWiredUInt();

            if (PiciEnvironment.GetGame().GetClientManager().GetNameById(UserId) != "Unknown User")
            {
                Session.SendMessage(ModerationTool.SerializeUserInfo(UserId));
            }
            else
            {
                Session.SendNotif(LanguageLocale.GetValue("user.loadusererror"));
            }
        }

        internal void ModGetUserChatlog()
        {
            if (!Session.GetHabbo().HasRight("acc_chatlogs"))
            {
                return;
            }

            Session.SendMessage(ModerationTool.SerializeUserChatlog(Request.PopWiredUInt()));
        }

        internal void ModGetRoomChatlog()
        {
            if (!Session.GetHabbo().HasRight("acc_chatlogs"))
            {
                return;
            }

            int Junk = Request.PopWiredInt32();
            uint RoomId = Request.PopWiredUInt();

            if (PiciEnvironment.GetGame().GetRoomManager().GetRoom(RoomId) != null)
            {
                Session.SendMessage(ModerationTool.SerializeRoomChatlog(RoomId));
            }
        }

        internal void ModGetRoomTool()
        {
            if (!Session.GetHabbo().HasRight("acc_supporttool"))
            {
                return;
            }

            uint RoomId = Request.PopWiredUInt();
            RoomData Data = PiciEnvironment.GetGame().GetRoomManager().GenerateNullableRoomData(RoomId);

            Session.SendMessage(ModerationTool.SerializeRoomTool(Data));
        }

        internal void ModPickTicket()
        {
            if (!Session.GetHabbo().HasRight("acc_supporttool"))
            {
                return;
            }

            int Junk = Request.PopWiredInt32();
            uint TicketId = Request.PopWiredUInt();
            PiciEnvironment.GetGame().GetModerationTool().PickTicket(Session, TicketId);
        }

        internal void ModReleaseTicket()
        {
            if (!Session.GetHabbo().HasRight("acc_supporttool"))
            {
                return;
            }

            int amount = Request.PopWiredInt32();

            for (int i = 0; i < amount; i++)
            {
                uint TicketId = Request.PopWiredUInt();

                PiciEnvironment.GetGame().GetModerationTool().ReleaseTicket(Session, TicketId);
            }
        }

        internal void ModCloseTicket()
        {
            if (!Session.GetHabbo().HasRight("acc_supporttool"))
            {
                return;
            }

            int Result = Request.PopWiredInt32(); // result, 1 = useless, 2 = abusive, 3 = resolved
            int Junk = Request.PopWiredInt32(); // ? 
            uint TicketId = Request.PopWiredUInt(); // id

            PiciEnvironment.GetGame().GetModerationTool().CloseTicket(Session, TicketId, Result);
        }

        internal void ModGetTicketChatlog()
        {

            if (!Session.GetHabbo().HasRight("acc_supporttool"))
            {
                return;
            }

            SupportTicket Ticket = PiciEnvironment.GetGame().GetModerationTool().GetTicket(Request.PopWiredUInt());

            if (Ticket == null)
            {
                return;
            }

            RoomData Data = PiciEnvironment.GetGame().GetRoomManager().GenerateNullableRoomData(Ticket.RoomId);

            if (Data == null)
            {
                return;
            }

            Session.SendMessage(ModerationTool.SerializeTicketChatlog(Ticket, Data, Ticket.Timestamp));
        }

        internal void ModGetRoomVisits()
        {
            if (!Session.GetHabbo().HasRight("acc_supporttool"))
            {
                return;
            }

            uint UserId = Request.PopWiredUInt();

            Session.SendMessage(ModerationTool.SerializeRoomVisits(UserId));
        }

        internal void ModSendRoomAlert()
        {
            if (!Session.GetHabbo().HasRight("acc_supporttool"))
            {
                return;
            }

            int One = Request.PopWiredInt32();
            int Two = Request.PopWiredInt32();
            String Message = Request.PopFixedString();

            ModerationTool.RoomAlert(Session.GetHabbo().CurrentRoomId, !Two.Equals(3), Message);
        }

        internal void ModPerformRoomAction()
        {
            if (!Session.GetHabbo().HasRight("acc_supporttool"))
            {
                return;
            }

            uint RoomId = Request.PopWiredUInt();
            Boolean ActOne = Request.PopWiredBoolean(); // set room lock to doorbell
            Boolean ActTwo = Request.PopWiredBoolean(); // set room to inappropiate
            Boolean ActThree = Request.PopWiredBoolean(); // kick all users

            ModerationTool.PerformRoomAction(Session, RoomId, ActThree, ActOne, ActTwo);
        }

        internal void ModSendUserCaution()
        {
            if (!Session.GetHabbo().HasRight("acc_supporttool"))
            {
                return;
            }

            uint UserId = Request.PopWiredUInt();
            String Message = Request.PopFixedString();

            ModerationTool.AlertUser(Session, UserId, Message, true);
        }

        internal void ModSendUserMessage()
        {
            if (!Session.GetHabbo().HasRight("acc_supporttool"))
            {
                return;
            }

            uint UserId = Request.PopWiredUInt();
            String Message = Request.PopFixedString();

            ModerationTool.AlertUser(Session, UserId, Message, false);
        }

        internal void ModKickUser()
        {
            if (!Session.GetHabbo().HasRight("acc_supporttool"))
            {
                return;
            }

            uint UserId = Request.PopWiredUInt();
            String Message = Request.PopFixedString();

            ModerationTool.KickUser(Session, UserId, Message, false);
        }

        internal void ModBanUser()
        {
            if (!Session.GetHabbo().HasRight("acc_supporttool"))
            {
                return;
            }

            uint UserId = Request.PopWiredUInt();
            String Message = Request.PopFixedString();
            int Length = Request.PopWiredInt32() * 3600;

            ModerationTool.BanUser(Session, UserId, Length, Message);
        }

        internal void CallGuideBot()
        {
            Room Room = PiciEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

            if (Room == null || !Room.CheckRights(Session, true))
            {
                return;
            }

            if (Room.guideBotIsCalled)
            {
                Session.GetMessageHandler().GetResponse().Init(33);
                Session.GetMessageHandler().GetResponse().AppendInt32(4009);
                Session.GetMessageHandler().SendResponse();

                return;
            }

            if (Session.GetHabbo().CalledGuideBot)
            {
                Session.GetMessageHandler().GetResponse().Init(33);
                Session.GetMessageHandler().GetResponse().AppendInt32(4010);
                Session.GetMessageHandler().SendResponse();

                return;
            }

            RoomUser NewUser = Room.DeployBot(PiciEnvironment.GetGame().GetBotManager().GetBot(55));
            NewUser.SetPos(Room.GetGameMap().Model.DoorX, Room.GetGameMap().Model.DoorY, Room.GetGameMap().Model.DoorZ);
            NewUser.UpdateNeeded = true;

            RoomUser RoomOwner = Room.GetRoomUserManager().GetRoomUserByHabbo(Room.Owner);

            if (RoomOwner != null)
            {
                NewUser.MoveTo(RoomOwner.Coordinate);
                NewUser.SetRot(Rotation.Calculate(NewUser.X, NewUser.Y, RoomOwner.X, RoomOwner.Y), false);
            }

            
            Session.GetHabbo().CalledGuideBot = true;
        }

        //internal void RegisterHelp()
        //{
        //    RequestHandlers.Add(416, new RequestHandler(InitHelpTool));
        //    RequestHandlers.Add(417, new RequestHandler(GetHelpCategories));
        //    RequestHandlers.Add(418, new RequestHandler(ViewHelpTopic));
        //    RequestHandlers.Add(419, new RequestHandler(SearchHelpTopics));
        //    RequestHandlers.Add(420, new RequestHandler(GetTopicsInCategory));
        //    RequestHandlers.Add(453, new RequestHandler(SubmitHelpTicket));
        //    RequestHandlers.Add(238, new RequestHandler(DeletePendingCFH));
        //    RequestHandlers.Add(440, new RequestHandler(CallGuideBot));
        //    RequestHandlers.Add(200, new RequestHandler(ModSendRoomAlert));
        //    RequestHandlers.Add(450, new RequestHandler(ModPickTicket));
        //    RequestHandlers.Add(451, new RequestHandler(ModReleaseTicket));
        //    RequestHandlers.Add(452, new RequestHandler(ModCloseTicket));
        //    RequestHandlers.Add(454, new RequestHandler(ModGetUserInfo));
        //    RequestHandlers.Add(455, new RequestHandler(ModGetUserChatlog));
        //    RequestHandlers.Add(456, new RequestHandler(ModGetRoomChatlog));
        //    RequestHandlers.Add(457, new RequestHandler(ModGetTicketChatlog));
        //    RequestHandlers.Add(458, new RequestHandler(ModGetRoomVisits));
        //    RequestHandlers.Add(459, new RequestHandler(ModGetRoomTool));
        //    RequestHandlers.Add(460, new RequestHandler(ModPerformRoomAction));
        //    RequestHandlers.Add(461, new RequestHandler(ModSendUserCaution));
        //    RequestHandlers.Add(462, new RequestHandler(ModSendUserMessage));
        //    RequestHandlers.Add(463, new RequestHandler(ModKickUser));
        //    RequestHandlers.Add(464, new RequestHandler(ModBanUser));
        //}

        //internal void UnregisterHelp()
        //{
        //    RequestHandlers.Remove(416);
        //    RequestHandlers.Remove(417);
        //    RequestHandlers.Remove(418);
        //    RequestHandlers.Remove(419);
        //    RequestHandlers.Remove(420);
        //    RequestHandlers.Remove(453);
        //    RequestHandlers.Remove(238);
        //    RequestHandlers.Remove(440);
        //    RequestHandlers.Remove(200);
        //    RequestHandlers.Remove(450);
        //    RequestHandlers.Remove(451);
        //    RequestHandlers.Remove(452);
        //    RequestHandlers.Remove(454);
        //    RequestHandlers.Remove(455);
        //    RequestHandlers.Remove(456);
        //    RequestHandlers.Remove(457);
        //    RequestHandlers.Remove(458);
        //    RequestHandlers.Remove(459);
        //    RequestHandlers.Remove(460);
        //    RequestHandlers.Remove(461);
        //    RequestHandlers.Remove(462);
        //    RequestHandlers.Remove(463);
        //    RequestHandlers.Remove(464);
        //}
    }
}
