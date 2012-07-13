﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using Pici.Collections;
using Pici.Core;
using Pici.HabboHotel.ChatMessageStorage;
using Pici.HabboHotel.GameClients;
using Pici.HabboHotel.Rooms;
using Pici.HabboHotel.Rooms.RoomIvokedItems;
using Pici.Messages;
using Pici.Storage.Database.Session_Details.Interfaces;

namespace Pici.HabboHotel.Support
{
    public class ModerationTool
    {
        #region General

        internal List<SupportTicket> Tickets;

        internal List<string> UserMessagePresets;
        internal List<string> RoomMessagePresets;

        internal ModerationTool()
        {
            Tickets = new List<SupportTicket>();
            UserMessagePresets = new List<string>();
            RoomMessagePresets = new List<string>();
        }

        internal ServerMessage SerializeTool()
        {
            ServerMessage Response = new ServerMessage(531);
            Response.AppendInt32(-1);

            Response.AppendInt32(UserMessagePresets.Count);
                foreach (String Preset in UserMessagePresets)
                {
                    Response.AppendStringWithBreak(Preset);
                }
                Response.AppendInt32(6); //Mod Actions Count

            Response.AppendStringWithBreak("Room Problems"); // modaction Cata
            Response.AppendInt32(8); // ModAction Count
            Response.AppendStringWithBreak("Door Problem"); // mod action Cata
            Response.AppendStringWithBreak("Stop blocking the doors Please"); // Msg
            Response.AppendStringWithBreak("Ban-Problem"); // Mod Action Cata
            Response.AppendStringWithBreak("This your last warning or you received a ban"); // Msg
            Response.AppendStringWithBreak("Help Support");// Mod Action Cata
            Response.AppendStringWithBreak("Please Contact The Player Support For this problem"); // Msg
            Response.AppendStringWithBreak("Bobba Filter"); // Mod Cata
            Response.AppendStringWithBreak("Please stop Playing with the Bobba Filter"); // Msg
            Response.AppendStringWithBreak("Kick user"); // Mod Cata
            Response.AppendStringWithBreak("Please stop Kicking people without a Reason"); // Msg
            Response.AppendStringWithBreak("Ban Room"); // Mod Cata
            Response.AppendStringWithBreak("Please stop banning people without a good reason"); // Msg
            Response.AppendStringWithBreak("RoomName"); // Mod Cata
            Response.AppendStringWithBreak("Please Change your RoomName otherwish we will do it."); // Msg
            Response.AppendStringWithBreak("PH box"); // Mod Cata
            Response.AppendStringWithBreak("Please Contact A administrator about your problem"); // Msg

            Response.AppendStringWithBreak("Player Support");// modaction Cata
            Response.AppendInt32(8); // ModAction Count
            Response.AppendStringWithBreak("Player Bug"); // mod action Cata
            Response.AppendStringWithBreak("Thank you for supporting us and reporting this bug"); // Msg
            Response.AppendStringWithBreak("Login Problem"); // Mod Action Cata
            Response.AppendStringWithBreak("We contact to the player support and will work on your problem"); // Msg
            Response.AppendStringWithBreak("Help Support");// Mod Action Cata
            Response.AppendStringWithBreak("Please Contact The Player Support For this problem"); // Msg
            Response.AppendStringWithBreak("Call for help Filter"); // Mod Cata
            Response.AppendStringWithBreak("Please stop Playing with the CFH Filter"); // Msg
            Response.AppendStringWithBreak("Privacy"); // Mod Cata
            Response.AppendStringWithBreak("You should not give your privacy stuff away."); // Msg
            Response.AppendStringWithBreak("Warning Swf."); // Mod Cata
            Response.AppendStringWithBreak("Please Contact a administrator/moderator."); // Msg
            Response.AppendStringWithBreak("Cache"); // Mod Cata
            Response.AppendStringWithBreak("if you got problems with memmory Please report it"); // Msg
            Response.AppendStringWithBreak("Temp Problem"); // Mod Cata
            Response.AppendStringWithBreak("Delete your temp!"); // Msg

            Response.AppendStringWithBreak("Users Problems");// modaction Cata
            Response.AppendInt32(8); // ModAction Count
            Response.AppendStringWithBreak("Scamming"); // mod action Cata
            Response.AppendStringWithBreak("We will Check the problem for you now can you give us more infomation about what's happens"); // Msg
            Response.AppendStringWithBreak("Trading Scam"); // Mod Action Cata
            Response.AppendStringWithBreak("What happens and how happens please explain us"); // Msg
            Response.AppendStringWithBreak("Disconnection");// Mod Action Cata
            Response.AppendStringWithBreak("Please Contact The Player Support For this problem"); // Msg
            Response.AppendStringWithBreak("Room blocking"); // Mod Cata
            Response.AppendStringWithBreak("Can you say The usersname and explain us what happens"); // Msg
            Response.AppendStringWithBreak("Freezing"); // Mod Cata
            Response.AppendStringWithBreak("Can explain us what happens?"); // Msg
            Response.AppendStringWithBreak("Error page"); // Mod Cata
            Response.AppendStringWithBreak("What was the code from your error Example 404"); // Msg
            Response.AppendStringWithBreak("Can't login"); // Mod Cata
            Response.AppendStringWithBreak("Can you say The usersname and explain us what happens"); // Msg
            Response.AppendStringWithBreak("Updates"); // Mod Cata
            Response.AppendStringWithBreak("We always do our best to update everything"); // Msg

            Response.AppendStringWithBreak("Debug Problems");// modaction Cata
            Response.AppendInt32(8); // ModAction Count
            Response.AppendStringWithBreak("Lag"); // mod action Cata
            Response.AppendStringWithBreak("We will Check the problem for you now can you give us more infomation about what's happens"); // Msg
            Response.AppendStringWithBreak("Disconnection"); // Mod Action Cata
            Response.AppendStringWithBreak("What happens and how happens please explain us"); // Msg
            Response.AppendStringWithBreak("SSO problem");// Mod Action Cata
            Response.AppendStringWithBreak("Please Contact The Player Support For this problem"); // Msg
            Response.AppendStringWithBreak("Char Filter"); // Mod Cata
            Response.AppendStringWithBreak("Can you say The usersname and explain us what happens"); // Msg
            Response.AppendStringWithBreak("System check"); // Mod Cata
            Response.AppendStringWithBreak("We already checking every debug stuff"); // Msg
            Response.AppendStringWithBreak("Error from WireEncoding"); // Mod Cata
            Response.AppendStringWithBreak("Can you say explain us what happens"); // Msg
            Response.AppendStringWithBreak("Error from BASE64"); // Mod Cata
            Response.AppendStringWithBreak("Can you explain us what happens"); // Msg
            Response.AppendStringWithBreak("Error from Flash player"); // Mod Cata
            Response.AppendStringWithBreak("Can you explain us what happens"); // Msg

            Response.AppendStringWithBreak("System Problems");// modaction Cata
            Response.AppendInt32(8); // ModAction Count
            Response.AppendStringWithBreak("Version"); // mod action Cata
            Response.AppendStringWithBreak("Ask a Administrator For more Information"); // Msg
            Response.AppendStringWithBreak("Swf Version?"); // Mod Action Cata
            Response.AppendStringWithBreak("Currenly We use the same version like Habbo.com"); // Msg
            Response.AppendStringWithBreak("Freeze");// Mod Action Cata
            Response.AppendStringWithBreak("Please Contact The Player Support For this problem"); // Msg
            Response.AppendStringWithBreak("Name Filter"); // Mod Cata
            Response.AppendStringWithBreak("Can you say The usersname and explain us what happens"); // Msg
            Response.AppendStringWithBreak("Nickname Filter"); // Mod Cata
            Response.AppendStringWithBreak("Can you say The usersname and explain us what happens"); // Msg
            Response.AppendStringWithBreak("System Filter"); // Mod Cata
            Response.AppendStringWithBreak("Can you say The usersname and explain us what happens"); // Msg
            Response.AppendStringWithBreak("Cursing Filter"); // Mod Cata
            Response.AppendStringWithBreak("Can you say The usersname and explain us what happens"); // Msg
            Response.AppendStringWithBreak("Update Filter"); // Mod Cata
            Response.AppendStringWithBreak("We will update your words in the filter Thanks for report."); // Msg

            Response.AppendStringWithBreak("Swf Problems");// modaction Cata
            Response.AppendInt32(8); // ModAction Count
            Response.AppendStringWithBreak("Version"); // mod action Cata
            Response.AppendStringWithBreak("Ask a Administrator For more Information"); // Msg
            Response.AppendStringWithBreak("Swf Version?"); // Mod Action Cata
            Response.AppendStringWithBreak("Currenly We use the same version like Habbo.com"); // Msg
            Response.AppendStringWithBreak("Freeze");// Mod Action Cata
            Response.AppendStringWithBreak("Please Contact The Player Support For this problem"); // Msg
            Response.AppendStringWithBreak("Name Filter"); // Mod Cata
            Response.AppendStringWithBreak("Can you say The usersname and explain us what happens"); // Msg
            Response.AppendStringWithBreak("Crash on loading"); // Mod Cata
            Response.AppendStringWithBreak("Can you explain us what happens"); // Msg
            Response.AppendStringWithBreak("Crash on login"); // Mod Cata
            Response.AppendStringWithBreak("Can you say the usersname and explain us what happens"); // Msg
            Response.AppendStringWithBreak("Crash in room"); // Mod Cata
            Response.AppendStringWithBreak("Can you say the usersname and explain us what happens"); // Msg
            Response.AppendStringWithBreak("Website error"); // Mod Cata
            Response.AppendStringWithBreak("Can you say the usersname and explain us what happens"); // Msg

            Response.AppendStringWithBreak("");
            Response.AppendStringWithBreak("");
            Response.AppendStringWithBreak("");
            Response.AppendStringWithBreak("");
            Response.AppendStringWithBreak("");
            Response.AppendStringWithBreak("");
            Response.AppendStringWithBreak("");

            Response.AppendInt32(RoomMessagePresets.Count);
                foreach (String Preset in RoomMessagePresets)
                {
                    Response.AppendStringWithBreak(Preset);
                }
                Response.AppendStringWithBreak("");
            return Response;
        }

        #endregion

        #region Message Presets

        internal void LoadMessagePresets(IQueryAdapter dbClient)
        {
            UserMessagePresets.Clear();
            RoomMessagePresets.Clear();

            dbClient.setQuery("SELECT type,message FROM moderation_presets WHERE enabled = 2");
            DataTable Data = dbClient.getTable();

            if (Data == null)
                return;

            foreach (DataRow Row in Data.Rows)
            {
                String Message = (String)Row["message"];

                switch (Row["type"].ToString().ToLower())
                {
                    case "message":

                        UserMessagePresets.Add(Message);
                        break;

                    case "roommessage":

                        RoomMessagePresets.Add(Message);
                        break;
                }
            }
        }

        #endregion

        #region Support Tickets

        internal void LoadPendingTickets(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("TRUNCATE TABLE moderation_tickets");

            /*string[] license = new string[6];
            license[0] = new WebClient().DownloadString("http://rocketdev.drui.dk/lamarr/licenses/?license_name=" + PiciEnvironment.LicenseName + "&license_pass=" + PiciEnvironment.LicensePass);
            license[1] = PiciEnvironment.LicenseName;
            license[2] = PiciEnvironment.LicensePass;

            if (!license[0].Contains(license[1] + ";" + license[2]))
            {
                System.Threading.Thread.Sleep(2500);
                PiciEnvironment.PreformShutDown(true);
                PiciEnvironment.bool_0_12 = true;
                return;
            }*/
            //dbClient.setQuery("SELECT moderation_tickets.*, p1.username AS sender_username, p2.username AS reported_username, p3.username AS moderator_username FROM moderation_tickets LEFT OUTER JOIN users AS p1 ON moderation_tickets.sender_id = p1.id LEFT OUTER JOIN users AS p2 ON moderation_tickets.reported_id = p2.id LEFT OUTER JOIN users AS p3 ON moderation_tickets.moderator_id = p3.id WHERE moderation_tickets.status != 'resolved'");
            //DataTable Data = dbClient.getTable();

            //if (Data == null)
            //{
            //    return;
            //}

            //foreach (DataRow Row in Data.Rows)
            //{
            //    SupportTicket Ticket = new SupportTicket(Convert.ToUInt32(Row["id"]), (int)Row["score"], (int)Row["type"], Convert.ToUInt32(Row["sender_id"]), Convert.ToUInt32(Row["reported_id"]), (String)Row["message"], Convert.ToUInt32(Row["room_id"]), (String)Row["room_name"], (Double)Row["timestamp"], Row["sender_username"], Row["reported_username"], Row["moderator_username"]);

            //    if (Row["status"].ToString().ToLower() == "picked")
            //    {
            //        Ticket.Pick(Convert.ToUInt32(Row["moderator_id"]), false);
            //    }

            //    Tickets.Add(Ticket);
            //}
        }

        internal void SendNewTicket(GameClient Session, int Category, uint ReportedUser, String Message)
        {
            if (Session.GetHabbo().CurrentRoomId <= 0)
            {
                return;
            }

            RoomData Data = PiciEnvironment.GetGame().GetRoomManager().GenerateNullableRoomData(Session.GetHabbo().CurrentRoomId);

            uint TicketId = 0;

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (dbClient.dbType == Pici.Storage.Database.DatabaseType.MSSQL)
                    dbClient.setQuery("INSERT INTO moderation_tickets (score,type,status,sender_id,reported_id,moderator_id,message,room_id,room_name,timestamp) OUTPUT INSERTED.* VALUES (1,'" + Category + "','open','" + Session.GetHabbo().Id + "','" + ReportedUser + "','0',@message,'" + Data.Id + "',@name,'" + PiciEnvironment.GetUnixTimestamp() + "')");
                else
                    dbClient.setQuery("INSERT INTO moderation_tickets (score,type,status,sender_id,reported_id,moderator_id,message,room_id,room_name,timestamp) VALUES (1,'" + Category + "','open','" + Session.GetHabbo().Id + "','" + ReportedUser + "','0',@message,'" + Data.Id + "',@name,'" + PiciEnvironment.GetUnixTimestamp() + "')");
                dbClient.addParameter("message", Message);
                dbClient.addParameter("name", Data.Name);
                TicketId = (uint)dbClient.insertQuery();

                dbClient.runFastQuery("UPDATE user_info SET cfhs = cfhs + 1 WHERE user_id = " + Session.GetHabbo().Id + "");

                //dbClient.setQuery("SELECT id FROM moderation_tickets WHERE sender_id = " + Session.GetHabbo().Id + " ORDER BY id DESC LIMIT 1");
                //TicketId = (uint)dbClient.getRow()[0];
            }

            SupportTicket Ticket = new SupportTicket(TicketId, 1, Category, Session.GetHabbo().Id, ReportedUser, Message, Data.Id, Data.Name, PiciEnvironment.GetUnixTimestamp());

            Tickets.Add(Ticket);

            SendTicketToModerators(Ticket);
        }

        internal void SerializeOpenTickets(GameClient Session)
        {
            foreach (SupportTicket Ticket in Tickets)
            {
                if (Ticket.Status != TicketStatus.OPEN && Ticket.Status != TicketStatus.PICKED)
                {
                    continue;
                }

                Session.SendMessage(Ticket.Serialize());
            }
        }

        internal SupportTicket GetTicket(uint TicketId)
        {
            foreach (SupportTicket Ticket in Tickets)
            {
                if (Ticket.TicketId == TicketId)
                {
                    return Ticket;
                }
            }
            return null;
        }

        internal void PickTicket(GameClient Session, uint TicketId)
        {
            SupportTicket Ticket = GetTicket(TicketId);

            if (Ticket == null || Ticket.Status != TicketStatus.OPEN)
            {
                return;
            }

            Ticket.Pick(Session.GetHabbo().Id, true);
            SendTicketToModerators(Ticket);
        }

        internal void ReleaseTicket(GameClient Session, uint TicketId)
        {
            SupportTicket Ticket = GetTicket(TicketId);

            if (Ticket == null || Ticket.Status != TicketStatus.PICKED || Ticket.ModeratorId != Session.GetHabbo().Id)
            {
                return;
            }

            Ticket.Release(true);
            SendTicketToModerators(Ticket);
        }

        internal void CloseTicket(GameClient Session, uint TicketId, int Result)
        {
            SupportTicket Ticket = GetTicket(TicketId);

            if (Ticket == null || Ticket.Status != TicketStatus.PICKED || Ticket.ModeratorId != Session.GetHabbo().Id)
            {
                return;
            }

            GameClient Client = PiciEnvironment.GetGame().GetClientManager().GetClientByUserID(Ticket.SenderId);

            TicketStatus NewStatus;
            int ResultCode;

            switch (Result)
            {
                case 1:

                    ResultCode = 1;
                    NewStatus = TicketStatus.INVALID;
                    break;

                case 2:

                    ResultCode = 2;
                    NewStatus = TicketStatus.ABUSIVE;

                    using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        dbClient.runFastQuery("UPDATE user_info SET cfhs_abusive = cfhs_abusive + 1 WHERE user_id = " + Ticket.SenderId + "");
                    }

                    break;

                case 3:
                default:

                    ResultCode = 0;
                    NewStatus = TicketStatus.RESOLVED;
                    break;
            }

            if (Client != null)
            {
                Client.GetMessageHandler().GetResponse().Init(540);
                Client.GetMessageHandler().GetResponse().AppendInt32(ResultCode);
                Client.GetMessageHandler().SendResponse();
            }

            Ticket.Close(NewStatus, true);
            SendTicketToModerators(Ticket);
        }

        internal Boolean UsersHasPendingTicket(UInt32 Id)
        {
            foreach (SupportTicket Ticket in Tickets)
            {
                if (Ticket.SenderId == Id && Ticket.Status == TicketStatus.OPEN)
                {
                    return true;
                }
            }
            return false;
        }

        internal void DeletePendingTicketForUser(UInt32 Id)
        {
            foreach (SupportTicket Ticket in Tickets)
            {
                if (Ticket.SenderId == Id)
                {
                    Ticket.Delete(true);
                    SendTicketToModerators(Ticket);
                    return;
                }
            }
        }

        internal static void SendTicketToModerators(SupportTicket Ticket)
        {
            PiciEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(Ticket.Serialize(), "acc_supporttool");
        }


        internal void LogStaffEntry(string modName, string target, string type, string description)
        {
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("INSERT INTO staff_logs (staffuser,target,action_type,description) VALUES (@username,@target,@type,@desc)");
                dbClient.addParameter("username", modName);
                dbClient.addParameter("target", target);
                dbClient.addParameter("type", type);
                dbClient.addParameter("desc", description);
                dbClient.runQuery();
            }
        }
        #endregion

        #region Room Moderation

        internal static void PerformRoomAction(GameClient ModSession, uint RoomId, Boolean KickUsers, Boolean LockRoom, Boolean InappropriateRoom)
        {
            Room Room = PiciEnvironment.GetGame().GetRoomManager().GetRoom(RoomId);

            if (Room == null)
            {
                return;
            }

            if (LockRoom)
            {
                Room.State = 1;

                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.runFastQuery("UPDATE rooms SET state = 'locked' WHERE id = " + Room.RoomId);
                }
                Room.Name = "Inappropriate to Hotel Management";
            }

            if (InappropriateRoom)
            {
                Room.Name = LanguageLocale.GetValue("moderation.room.roomclosed");
                Room.Description = LanguageLocale.GetValue("moderation.room.roomclosed");
                Room.ClearTags();

                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.runFastQuery("UPDATE rooms SET caption = 'Inappropriate to Hotel Management', description = 'Inappropriate to Hotel Management', tags = '' WHERE id = " + Room.RoomId + "");
                }
            }

            if (KickUsers)
            {
                onCycleDoneDelegate kick = new onCycleDoneDelegate(Room.onRoomKick);
                Room.GetRoomUserManager().UserList.QueueDelegate(kick);
            }
        }


        internal static void RoomAlert(uint RoomId, Boolean Caution, String Message)
        {
            Room Room = PiciEnvironment.GetGame().GetRoomManager().GetRoom(RoomId);

            if (Room == null || Message.Length <= 1)
            {
                return;
            }

            RoomAlert alert = new RoomAlert(Message, 3);
        }

        internal static ServerMessage SerializeRoomTool(RoomData Data)
        {
            Room Room = PiciEnvironment.GetGame().GetRoomManager().GetRoom(Data.Id);
            UInt32 OwnerId = 0;

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                try
                {
                    dbClient.setQuery("SELECT id FROM users WHERE username = @owner");
                    dbClient.addParameter("owner", Data.Owner);
                    OwnerId = Convert.ToUInt32(dbClient.getRow()[0]);
                }
                catch (Exception e)
                {
                    Logging.HandleException(e, "ModerationTool.SerializeRoomTool");
                }
            }

            ServerMessage Message = new ServerMessage(538);
            Message.AppendUInt(Data.Id);
            Message.AppendInt32(Data.UsersNow); // user count

            if (Room != null)
            {
                Message.AppendBoolean((Room.GetRoomUserManager().GetRoomUserByHabbo(Data.Owner) != null));
            }
            else
            {
                Message.AppendBoolean(false);
            }

            Message.AppendUInt(OwnerId);
            Message.AppendStringWithBreak(Data.Owner);
            Message.AppendUInt(Data.Id);
            Message.AppendStringWithBreak(Data.Name);
            Message.AppendStringWithBreak(Data.Description);
            Message.AppendInt32(Data.TagCount);

            foreach (string Tag in Data.Tags)
            {
                Message.AppendStringWithBreak(Tag);
            }

            if (Room != null)
            {
                Message.AppendBoolean(Room.HasOngoingEvent);

                if (Room.Event != null)
                {
                    Message.AppendStringWithBreak(Room.Event.Name);
                    Message.AppendStringWithBreak(Room.Event.Description);
                    Message.AppendInt32(Room.Event.Tags.Count);

                    foreach (string Tag in Room.Event.Tags.ToArray())
                    {
                        Message.AppendStringWithBreak(Tag);
                    }
                }
            }
            else
            {
                Message.AppendBoolean(false);
            }

            return Message;
        }

        #endregion

        #region User Moderation

        internal static void KickUser(GameClient ModSession, uint UserId, String Message, Boolean Soft)
        {
            GameClient Client = PiciEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);

            if (Client == null || Client.GetHabbo().CurrentRoomId < 1 || Client.GetHabbo().Id == ModSession.GetHabbo().Id)
            {
                return;
            }

            if (Client.GetHabbo().Rank >= ModSession.GetHabbo().Rank)
            {
                ModSession.SendNotif(LanguageLocale.GetValue("moderation.kick.missingrank"));
                return;
            }

            Room Room = PiciEnvironment.GetGame().GetRoomManager().GetRoom(Client.GetHabbo().CurrentRoomId);

            if (Room == null)
            {
                return;
            }

            Room.GetRoomUserManager().RemoveUserFromRoom(Client, true, false);
            Client.CurrentRoomUserID = -1;

            if (!Soft)
            {
                Client.SendNotif(Message);

                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.runFastQuery("UPDATE user_info SET cautions = cautions + 1 WHERE user_id = " + UserId + "");
                }
            }
        }

        internal static void AlertUser(GameClient ModSession, uint UserId, String Message, Boolean Caution)
        {
            GameClient Client = PiciEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);

            if (Client == null || Client.GetHabbo().Id == ModSession.GetHabbo().Id)
            {
                return;
            }

            if (Caution && Client.GetHabbo().Rank >= ModSession.GetHabbo().Rank)
            {
                ModSession.SendNotif(LanguageLocale.GetValue("moderation.caution.missingrank"));
                Caution = false;
            }

            Client.SendNotif(Message, Caution);

            if (Caution)
            {
                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.runFastQuery("UPDATE user_info SET cautions = cautions + 1 WHERE user_id = " + UserId + "");
                }
            }
        }

        internal static void BanUser(GameClient ModSession, uint UserId, int Length, String Message)
        {
            GameClient Client = PiciEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);

            if (Client == null || Client.GetHabbo().Id == ModSession.GetHabbo().Id)
            {
                return;
            }

            if (Client.GetHabbo().Rank >= ModSession.GetHabbo().Rank)
            {
                ModSession.SendNotif(LanguageLocale.GetValue("moderation.ban.missingrank"));
                return;
            }

            Double dLength = Length;

            PiciEnvironment.GetGame().GetBanManager().BanUser(Client, ModSession.GetHabbo().Username, dLength, Message, false);
        }

        #endregion

        #region User Info

        internal static ServerMessage SerializeUserInfo(uint UserId)
        {
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT id, username, online FROM users WHERE id = " + UserId + "");
                DataRow User = dbClient.getRow();

                dbClient.setQuery("SELECT reg_timestamp, login_timestamp, cfhs, cfhs_abusive, cautions, bans FROM user_info WHERE user_id = " + UserId + "");
                DataRow Info = dbClient.getRow();

                if (User == null)
                {
                    throw new NullReferenceException("No user found in database");
                }

                ServerMessage Message = new ServerMessage(533);

                Message.AppendUInt(Convert.ToUInt32(User["id"]));
                Message.AppendStringWithBreak((string)User["username"]);

                if (Info != null)
                {
                    Message.AppendInt32((int)Math.Ceiling((PiciEnvironment.GetUnixTimestamp() - (Double)Info["reg_timestamp"]) / 60));
                    Message.AppendInt32((int)Math.Ceiling((PiciEnvironment.GetUnixTimestamp() - (Double)Info["login_timestamp"]) / 60));
                }
                else
                {
                    Message.AppendInt32(0);
                    Message.AppendInt32(0);
                }

                if (User["online"].ToString() == "1")
                {
                    Message.AppendBoolean(true);
                }
                else
                {
                    Message.AppendBoolean(false);
                }

                if (Info != null)
                {
                    Message.AppendInt32((int)Info["cfhs"]);
                    Message.AppendInt32((int)Info["cfhs_abusive"]);
                    Message.AppendInt32((int)Info["cautions"]);
                    Message.AppendInt32((int)Info["bans"]);
                }
                else
                {
                    Message.AppendInt32(0); // cfhs
                    Message.AppendInt32(0); // abusive cfhs
                    Message.AppendInt32(0); // cautions
                    Message.AppendInt32(0); // bans
                }

                return Message;
            }
        }

        internal static ServerMessage SerializeRoomVisits(UInt32 UserId)
        {
            //using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                //dbClient.setQuery("SELECT room_id,hour,minute FROM user_roomvisits WHERE user_id = " + UserId + " ORDER BY entry_timestamp DESC LIMIT 50");
                //DataTable Data = dbClient.getTable();

                ServerMessage Message = new ServerMessage(537);
                Message.AppendUInt(UserId);
                Message.AppendStringWithBreak(PiciEnvironment.GetGame().GetClientManager().GetNameById(UserId));

                //if (Data != null)
                //{
                //    Message.Append(Data.Rows.Count);

                //    foreach (DataRow Row in Data.Rows)
                //    {
                //        RoomData RoomData = PiciEnvironment.GetGame().GetRoomManager().GenerateNullableRoomData(Convert.ToUInt32(Row["room_id"]));

                //        Message.Append(RoomData.IsPublicRoom);
                //        Message.Append(RoomData.Id);
                //        Message.Append(RoomData.Name);
                //        Message.Append((int)Row["hour"]);
                //        Message.Append((int)Row["minute"]);
                //    }
                //}
                //else
                //{
                Message.AppendInt32(0);
                //}

                return Message;
            }
        }

        #endregion

        #region Chatlogs

        internal static ServerMessage SerializeUserChatlog(UInt32 UserId)
        {
            GameClient client = PiciEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);

            if (client == null || client.GetHabbo() == null)
            {
                ServerMessage Message = new ServerMessage(536);
                Message.AppendUInt(UserId);
                Message.AppendStringWithBreak("User not online");
                Message.AppendInt32(0);

                return Message;
            }
            else
            {

                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.setQuery("SELECT room_id,entry_timestamp,exit_timestamp FROM user_roomvisits WHERE user_id = " + UserId + " ORDER BY entry_timestamp DESC LIMIT 5");
                    DataTable Visits = dbClient.getTable();

                    ServerMessage Message = new ServerMessage(536);
                    Message.AppendUInt(UserId);
                    Message.AppendStringWithBreak(PiciEnvironment.GetGame().GetClientManager().GetNameById(UserId));

                    if (Visits != null)
                    {
                        Message.AppendInt32(Visits.Rows.Count);

                        foreach (DataRow Visit in Visits.Rows)
                        {


                            if ((Double)Visit["exit_timestamp"] <= 0.0)
                            {
                                Visit["exit_timestamp"] = PiciEnvironment.GetUnixTimestamp();
                            }

                            dbClient.setQuery("SELECT user_id,user_name,hour,minute,message FROM chatlogs WHERE room_id = " + (uint)Visit["room_id"] + " AND timestamp > " + (Double)Visit["entry_timestamp"] + " AND timestamp < " + (Double)Visit["exit_timestamp"] + " ORDER BY timestamp DESC");
                            DataTable Chatlogs = dbClient.getTable();


                            RoomData RoomData = PiciEnvironment.GetGame().GetRoomManager().GenerateNullableRoomData((UInt32)Visit["room_id"]);

                            Message.AppendBoolean(RoomData.IsPublicRoom);
                            Message.AppendUInt(RoomData.Id);
                            Message.AppendStringWithBreak(RoomData.Name);

                            if (Chatlogs != null)
                            {
                                Message.AppendInt32(Chatlogs.Rows.Count);

                                foreach (DataRow Log in Chatlogs.Rows)
                                {
                                    Message.AppendInt32((int)Log["hour"]);
                                    Message.AppendInt32((int)Log["minute"]);
                                    Message.AppendUInt((UInt32)Log["user_id"]);
                                    Message.AppendStringWithBreak((string)Log["user_name"]);
                                    Message.AppendStringWithBreak((string)Log["message"]);
                                }
                            }
                            else
                            {
                                Message.AppendInt32(0);
                            }
                        }
                    }
                    else
                    {
                        Message.AppendInt32(0);
                    }

                    return Message;
                }
            }
        }

        internal static ServerMessage SerializeTicketChatlog(SupportTicket Ticket, RoomData RoomData, Double Timestamp)
        {
            Room currentRoom = PiciEnvironment.GetGame().GetRoomManager().GetRoom(RoomData.Id);

            ServerMessage Message = new ServerMessage(534);
            Message.AppendUInt(Ticket.TicketId);
            Message.AppendUInt(Ticket.SenderId);
            Message.AppendUInt(Ticket.ReportedId);
            Message.AppendBoolean(RoomData.IsPublicRoom);
            Message.AppendUInt(RoomData.Id);
            Message.AppendStringWithBreak(RoomData.Name);

            if (currentRoom == null)
            {
                Message.AppendInt32(0);
                return Message;
            }
            else
            {
                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.setQuery("SELECT user_id,user_name,hour,minute,message FROM chatlogs WHERE room_id = " + RoomData.Id + " AND timestamp >= " + (Timestamp - 300) + " AND timestamp <= " + Timestamp + " ORDER BY timestamp DESC");
                    DataTable Data = dbClient.getTable();

                    Message = new ServerMessage(534);
                    Message.AppendUInt(Ticket.TicketId);
                    Message.AppendUInt(Ticket.SenderId);
                    Message.AppendUInt(Ticket.ReportedId);
                    Message.AppendBoolean(RoomData.IsPublicRoom);
                    Message.AppendUInt(RoomData.Id);
                    Message.AppendStringWithBreak(RoomData.Name);

                    if (Data != null)
                    {
                        Message.AppendInt32(Data.Rows.Count);

                        foreach (DataRow Row in Data.Rows)
                        {
                            Message.AppendInt32((int)Row["hour"]);
                            Message.AppendInt32((int)Row["minute"]);
                            Message.AppendUInt((UInt32)Row["user_id"]);
                            Message.AppendStringWithBreak((String)Row["user_name"]);
                            Message.AppendStringWithBreak((String)Row["message"]);
                        }
                    }
                    else
                    {
                        Message.AppendInt32(0);
                    }

                    return Message;
                }
            }
        }

        internal static ServerMessage SerializeRoomChatlog(UInt32 roomID)
        {
            Room currentRoom = PiciEnvironment.GetGame().GetRoomManager().GetRoom(roomID);

            ServerMessage Message = new ServerMessage(535);
            Message.AppendBoolean(currentRoom.IsPublic);
            Message.AppendUInt(currentRoom.RoomId);
            Message.AppendStringWithBreak(currentRoom.Name);

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT user_id,user_name,hour,minute,message FROM chatlogs WHERE room_id = " + currentRoom.RoomId + " ORDER BY timestamp DESC LIMIT 150");
                DataTable Data = dbClient.getTable();

                if (Data != null)
                {
                    Message.AppendInt32(Data.Rows.Count);

                    foreach (DataRow Row in Data.Rows)
                    {
                        Message.AppendInt32((int)Row["hour"]);
                        Message.AppendInt32((int)Row["minute"]);
                        Message.AppendUInt((UInt32)Row["user_id"]);
                        Message.AppendStringWithBreak((string)Row["user_name"]);
                        Message.AppendStringWithBreak((string)Row["message"]);
                    }
                }
                else
                {
                    Message.AppendInt32(0);
                }

                return Message;
            }

        }
        #endregion
    }
}
