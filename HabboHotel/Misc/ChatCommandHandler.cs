using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Pici.Core;
using Pici.HabboHotel.GameClients;
using Pici.HabboHotel.Items;
using Pici.HabboHotel.Rooms;
using Pici.HabboHotel.Rooms.RoomIvokedItems;
using Pici.HabboHotel.Users;
using Pici.IRC;
using Pici.Messages;
using Pici.Storage.Database.Session_Details.Interfaces;
using System.Drawing;
using Pici.Util;

namespace Pici.HabboHotel.Misc
{
class ChatCommandHandler
    {
        private GameClient Session;
        private string[] Params;

        public ChatCommandHandler(string[] input, GameClient session)
        {
            Params = input;
            Session = session;
        }

        internal bool WasExecuted()
        {
            ChatCommand command = ChatCommandRegister.GetCommand(Params[0].Substring(1).ToLower());

            if (command.UserGotAuthorization(Session))
            {
                ChatCommandRegister.InvokeCommand(this, command.commandID);
                Dispose();
                return true;
            }
            else
            {
                Dispose();
                return false;
            }
        }

        internal void Dispose()
        {
            Session = null;
            Array.Clear(this.Params, 0, Params.Length);
        }

        #region Commands
        internal void moonwalk()
        {
            Room room = Session.GetHabbo().CurrentRoom;
            if (room == null)
                return;

            RoomUser roomuser = room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            roomuser.moonwalkEnabled = !roomuser.moonwalkEnabled;

            if (roomuser.moonwalkEnabled)
                Session.SendNotif("Moonwalk enabled");
            else
                Session.SendNotif("Moonwalk disabled");
        }

        internal void push()
        {
            Room room = Session.GetHabbo().CurrentRoom;
            if (room == null)
                return;

            RoomUser roomuser = room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (roomuser == null)
                return;
            
            if (Params.Length == 1)
            {
                
                Point squareInfront = CoordinationUtil.GetPointInFront(roomuser.Coordinate, roomuser.RotBody);
                List<RoomUser> users = room.GetGameMap().GetRoomUsers(squareInfront);

                Point squareInFrontOfUserInFront = CoordinationUtil.GetPointInFront(squareInfront, roomuser.RotBody); //Yo dawg, we heard yo like coordinates, so we put a coordinate inside yo coordinate
                foreach (RoomUser user in users)
                {
                    user.MoveTo(squareInFrontOfUserInFront);
                }
            }
            else
            {
                RoomUser roomuserTarget = room.GetRoomUserManager().GetRoomUserByHabbo(Params[1]);
                if (roomuserTarget == null)
                    return;

                Point furtherstSquare = CoordinationUtil.GetPointBehind(roomuserTarget.Coordinate, roomuserTarget.RotBody);

                Point a = new Point(furtherstSquare.X, furtherstSquare.Y++);
                if (CoordinationUtil.GetDistance(furtherstSquare, a) > CoordinationUtil.GetDistance(furtherstSquare, roomuserTarget.Coordinate))
                    furtherstSquare = a;

                Point b = new Point(furtherstSquare.X, furtherstSquare.Y--);
                if (CoordinationUtil.GetDistance(furtherstSquare, b) > CoordinationUtil.GetDistance(furtherstSquare, roomuserTarget.Coordinate))
                    furtherstSquare = b;

                Point c = new Point(furtherstSquare.X++, furtherstSquare.Y);
                if (CoordinationUtil.GetDistance(furtherstSquare, c) > CoordinationUtil.GetDistance(furtherstSquare, roomuserTarget.Coordinate))
                    furtherstSquare = c;

                Point d = new Point(furtherstSquare.X--, furtherstSquare.Y++);
                if (CoordinationUtil.GetDistance(furtherstSquare, d) > CoordinationUtil.GetDistance(furtherstSquare, roomuserTarget.Coordinate))
                    furtherstSquare = d;

                Point e = new Point(furtherstSquare.X++, furtherstSquare.Y--);
                if (CoordinationUtil.GetDistance(furtherstSquare, e) > CoordinationUtil.GetDistance(furtherstSquare, roomuserTarget.Coordinate))
                    furtherstSquare = e;

                Point f = new Point(furtherstSquare.X--, furtherstSquare.Y);
                if (CoordinationUtil.GetDistance(furtherstSquare, f) > CoordinationUtil.GetDistance(furtherstSquare, roomuserTarget.Coordinate))
                    furtherstSquare = f;

                roomuserTarget.MoveTo(furtherstSquare);
            }
        }

        internal void pull()
        {
            Room room = Session.GetHabbo().CurrentRoom;
            if (room == null)
                return;


            RoomUser roomuser = room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (roomuser == null)
                return;

            if (Params.Length == 1)
            {
                Point squareInFront = CoordinationUtil.GetPointInFront(roomuser.Coordinate, roomuser.RotBody);
                Point squareInFrontInFront = CoordinationUtil.GetPointBehind(squareInFront, CoordinationUtil.RotationIverse(roomuser.RotBody));
                List<RoomUser> users = room.GetGameMap().GetRoomUsers(squareInFrontInFront);

                foreach (RoomUser user in users)
                {
                    user.MoveTo(squareInFront);
                }
            }
            else
            {
                RoomUser roomuserTarget = room.GetRoomUserManager().GetRoomUserByHabbo(Params[1]);
                if (roomuserTarget == null)
                    return;

                Point closestSquare = CoordinationUtil.GetPointBehind(roomuserTarget.Coordinate, roomuserTarget.RotBody);

                Point a = new Point(closestSquare.X, closestSquare.Y++);
                if (CoordinationUtil.GetDistance(closestSquare, a) < CoordinationUtil.GetDistance(closestSquare, roomuserTarget.Coordinate))
                    closestSquare = a;

                Point b = new Point(closestSquare.X, closestSquare.Y--);
                if (CoordinationUtil.GetDistance(closestSquare, b) < CoordinationUtil.GetDistance(closestSquare, roomuserTarget.Coordinate))
                    closestSquare = b;

                Point c = new Point(closestSquare.X++, closestSquare.Y);
                if (CoordinationUtil.GetDistance(closestSquare, c) < CoordinationUtil.GetDistance(closestSquare, roomuserTarget.Coordinate))
                    closestSquare = c;

                Point d = new Point(closestSquare.X--, closestSquare.Y++);
                if (CoordinationUtil.GetDistance(closestSquare, d) < CoordinationUtil.GetDistance(closestSquare, roomuserTarget.Coordinate))
                    closestSquare = d;

                Point e = new Point(closestSquare.X++, closestSquare.Y--);
                if (CoordinationUtil.GetDistance(closestSquare, e) < CoordinationUtil.GetDistance(closestSquare, roomuserTarget.Coordinate))
                    closestSquare = e;

                Point f = new Point(closestSquare.X--, closestSquare.Y);
                if (CoordinationUtil.GetDistance(closestSquare, f) < CoordinationUtil.GetDistance(closestSquare, roomuserTarget.Coordinate))
                    closestSquare = f;

                roomuserTarget.MoveTo(closestSquare);
            }
        }

        internal void copylook()
        {
            string copyTarget = Params[1];
            bool findResult = false;


            string gender = null;
            string figure = null;
            DataRow dRow;
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT gender,look FROM users WHERE username = @username");
                dbClient.addParameter("username", copyTarget);
                dRow = dbClient.getRow();

                if (dRow != null)
                {
                    findResult = true;
                    gender = (string)dRow[0];
                    figure = (string)dRow[1];

                    dbClient.setQuery("UPDATE users SET gender = @gender, look = @look WHERE username = @username");
                    dbClient.addParameter("gender", gender);
                    dbClient.addParameter("look", figure);
                    dbClient.addParameter("username", Session.GetHabbo().Username);
                    dbClient.runQuery();
                }
            }

            if (findResult)
            {
                Session.GetHabbo().Gender = gender;
                Session.GetHabbo().Look = figure;
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
        }


        internal void pickall()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            TargetRoom = Session.GetHabbo().CurrentRoom;

            if (TargetRoom != null && TargetRoom.CheckRights(Session, true))
            {
                List<RoomItem> RemovedItems = TargetRoom.GetRoomItemHandler().RemoveAllFurniture(Session);
                Session.GetHabbo().GetInventoryComponent().AddItemArray(RemovedItems);
                Session.GetHabbo().GetInventoryComponent().UpdateItems(false); //ARGH!
            }
        }

        internal void setspeed()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            TargetRoom = Session.GetHabbo().CurrentRoom;
            if (TargetRoom != null && TargetRoom.CheckRights(Session, true))
            {
                try
                {
                    Session.GetHabbo().CurrentRoom.GetRoomItemHandler().SetSpeed(int.Parse(Params[1]));
                }
                catch { Session.SendNotif(LanguageLocale.GetValue("input.intonly")); }
            }
        }

        internal void unload()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            PiciEnvironment.GetGame().GetRoomManager().UnloadRoom(TargetRoom);
            //TargetRoom.RequestReload();
        }

        internal void disablediagonal()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;

            TargetRoom = Session.GetHabbo().CurrentRoom;

            if (TargetRoom.GetGameMap().DiagonalEnabled)
                TargetRoom.GetGameMap().DiagonalEnabled = false;
            else
                TargetRoom.GetGameMap().DiagonalEnabled = true;
        }

        internal void setmax()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            TargetRoom = Session.GetHabbo().CurrentRoom;
            UInt32 roomid = TargetRoom.RoomId;

            try
            {
                int MaxUsers = int.Parse(Params[1]);

                if ((MaxUsers > 100 && Session.GetHabbo().Rank == 1) || MaxUsers > 200)
                    Session.SendNotif(LanguageLocale.GetValue("setmax.maxusersreached"));
                else
                {
                    TargetRoom.SetMaxUsers(MaxUsers);
                }
            }
            catch
            { }
        }

        internal void overridee()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            RoomUser TargetRoomUser = null;

            TargetRoom = PiciEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

            if (TargetRoom == null)
                return;

            TargetRoomUser = TargetRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);

            if (TargetRoomUser == null)
                return;

            if (TargetRoomUser.AllowOverride)
            {
                TargetRoomUser.AllowOverride = false;
                Session.SendNotif(LanguageLocale.GetValue("override.disabled"));
            }
            else
            {
                TargetRoomUser.AllowOverride = true;
                Session.SendNotif(LanguageLocale.GetValue("override.enabled"));
            }

            TargetRoom.GetGameMap().GenerateMaps();
        }

        internal void warp()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            RoomUser TargetRoomUser = null;

            TargetRoom = PiciEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

            TargetRoomUser = TargetRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);

            if (TargetRoomUser == null)
                return;
            if (!TargetRoom.CheckRights(Session,false))
            {
                return;
            }

            TargetRoomUser.TeleportEnabled = !TargetRoomUser.TeleportEnabled;

            TargetRoom.GetGameMap().GenerateMaps();
        }
        
        internal void teleport()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            RoomUser TargetRoomUser = null;

            TargetRoom = PiciEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

            TargetRoomUser = TargetRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);

            if (TargetRoomUser == null)
                return;

            TargetRoomUser.TeleportEnabled = !TargetRoomUser.TeleportEnabled;

            TargetRoom.GetGameMap().GenerateMaps();
        }

        internal static void catarefresh()
        {
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                PiciEnvironment.GetGame().GetCatalog().Initialize(dbClient);
            }
            PiciEnvironment.GetGame().GetCatalog().InitCache();
            PiciEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(new ServerMessage(441));
        }

        internal void unbanUser()
        {
            if (Params.Length > 1)
            {
                PiciEnvironment.GetGame().GetBanManager().UnbanUser(Params[1]);
            }
        }

        internal void roomalert()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            TargetRoom = PiciEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

            if (TargetRoom == null)
                return;


            string Msg = MergeParams(Params, 1);

            ServerMessage nMessage = new ServerMessage();
            nMessage.Init(161);
            nMessage.Append(Msg);

            PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, string.Empty, "Alert", "Room alert with message [" + Msg + "]");
            TargetRoom.QueueRoomMessage(nMessage);
        }

        internal void coords()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            RoomUser TargetRoomUser = null;
            TargetRoom = PiciEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

            if (TargetRoom == null)
            {
                return;
            }

            TargetRoomUser = TargetRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);

            if (TargetRoomUser == null)
            {
                return;
            }

            Session.SendNotif("X: " + TargetRoomUser.X + " - Y: " + TargetRoomUser.Y + " - Z: " + TargetRoomUser.Z + " - Rot: " + TargetRoomUser.RotBody + ", sqState: " + TargetRoom.GetGameMap().GameMap[TargetRoomUser.X, TargetRoomUser.Y].ToString());
        }

        internal void coins()
        {
            GameClient TargetClient = null;
            Room TargetRoom = Session.GetHabbo().CurrentRoom;

            TargetClient = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient != null)
            {
                int creditsToAdd;
                if (int.TryParse(Params[2], out creditsToAdd))
                {
                    TargetClient.GetHabbo().Credits = TargetClient.GetHabbo().Credits + creditsToAdd;
                    TargetClient.GetHabbo().UpdateCreditsBalance();
                    TargetClient.SendNotif(Session.GetHabbo().Username + LanguageLocale.GetValue("coins.awardmessage1") + creditsToAdd.ToString() + LanguageLocale.GetValue("coins.awardmessage2"));
                    Session.SendNotif(LanguageLocale.GetValue("coins.updateok"));
                    return;
                }
                else
                {
                    Session.SendNotif(LanguageLocale.GetValue("input.intonly"));
                    return;
                }
            }
            else
            {
                Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
                return;
            }
        }

        internal void pixels()
        {
            GameClient TargetClient = null;
            Room TargetRoom = Session.GetHabbo().CurrentRoom;

            TargetClient = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient != null)
            {
                int creditsToAdd;
                if (int.TryParse(Params[2], out creditsToAdd))
                {
                    TargetClient.GetHabbo().ActivityPoints = TargetClient.GetHabbo().ActivityPoints + creditsToAdd;
                    TargetClient.GetHabbo().UpdateActivityPointsBalance(true);
                    TargetClient.SendNotif(Session.GetHabbo().Username + LanguageLocale.GetValue("pixels.awardmessage1") + creditsToAdd.ToString() + LanguageLocale.GetValue("pixels.awardmessage2"));
                    Session.SendNotif(LanguageLocale.GetValue("pixels.updateok"));
                    return;
                }
                else
                {
                    Session.SendNotif(LanguageLocale.GetValue("input.intonly"));
                    return;
                }
            }
            else
            {
                Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
                return;
            }
        }

        internal void handitem()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            RoomUser TargetRoomUser = null;
            TargetRoom = PiciEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

            if (TargetRoom == null)
            {
                return;
            }

            TargetRoomUser = TargetRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);

            if (TargetRoomUser == null)
            {
                return;
            }

            try
            {
                TargetRoomUser.CarryItem(int.Parse(Params[1]));
            }
            catch { }

            return;
        }

        internal void hotelalert()
        {
            string Notice = GetInput(Params).Substring(4);

            ServerMessage HotelAlert = new ServerMessage(808);
            HotelAlert.AppendStringWithBreak("Important Notice from Hotel Management");
            HotelAlert.AppendStringWithBreak(Notice + "\r\n" + "- " + Session.GetHabbo().Username);
            PiciEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(HotelAlert);

            PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, string.Empty, "HotelAlert", "Hotel alert [" + Notice + "]");

            //PiciEnvironment.messagingBot.SendMassMessage(new PublicMessage(string.Format("[{0}] => [{1}]", Session.GetHabbo().Username, Notice)), true);
        }

        internal void freeze()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            RoomUser Target = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Params[1]);
            if (Target != null)
                Target.Freezed = (Target.Freezed != true);
        }

        internal void buyx()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            try
            {
                int userInput = int.Parse(Params[1]);
                if (Session.GetHabbo().Rank > 1)
                {
                    if (userInput <= 0)
                    {
                        Session.SendNotif(LanguageLocale.GetValue("buyx.maxminreached"));
                    }
                    else
                    {
                        Session.GetHabbo().buyItemLoop = userInput;
                    }
                }
                else
                {
                    if (userInput <= 0 || userInput >= 50)
                    {
                        Session.SendNotif(LanguageLocale.GetValue("buyx.maxminreached"));
                    }
                    else
                    {
                        Session.GetHabbo().buyItemLoop = userInput;
                    }

                }
            }
            catch
            {
                Session.SendNotif(LanguageLocale.GetValue("input.intonly"));
            }
        }

        internal void enable()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;

            int EffectID = int.Parse(Params[1]);
            Session.GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(EffectID);
        }

        internal void roommute()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            if (Session.GetHabbo().CurrentRoom.RoomMuted)
                Session.GetHabbo().CurrentRoom.RoomMuted = false;
            else
                Session.GetHabbo().CurrentRoom.RoomMuted = true;

            PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, string.Empty, "Room Mute", "Room muted");

        }

        public void masscredits()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;

            try
            {
                int CreditAmount = int.Parse(Params[1]);
                PiciEnvironment.GetGame().GetClientManager().QueueCreditsUpdate(CreditAmount);
                PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, string.Empty, "Mass Credits", "Send [" + CreditAmount + "] credits to everyone online");

            }
            catch
            {
                Session.SendNotif(LanguageLocale.GetValue("input.intonly"));
            }
        }

        internal void globalcredits()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;

            try
            {
                int CreditAmount = int.Parse(Params[1]);
                PiciEnvironment.GetGame().GetClientManager().QueueCreditsUpdate(CreditAmount);

                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                    dbClient.runFastQuery("UPDATE users SET credits = credits + " + CreditAmount);

                PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, string.Empty, "Mass Credits", "Send [" + CreditAmount + "] credits to everyone in the database");

            }
            catch
            {
                Session.SendNotif(LanguageLocale.GetValue("input.intonly"));
            }
        }

        internal void openroom()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;

            try
            {
                uint roomID = uint.Parse(Params[1]);

                Session.GetMessageHandler().PrepareRoomForUser(roomID, "");
            }
            catch
            {
                Session.SendNotif(LanguageLocale.GetValue("input.intonly"));
            }
        }

        internal void stalk()
        {
            GameClient TargetClient = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);

            if (TargetClient == null || TargetClient.GetHabbo() == null || TargetClient.GetHabbo().CurrentRoom == null)
                return;

            Session.GetMessageHandler().PrepareRoomForUser(TargetClient.GetHabbo().CurrentRoom.RoomId, "");
        }

        internal void roombadge()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            if (Session.GetHabbo().CurrentRoom == null)
                return;
            
            TargetRoom.QueueRoomBadge(Params[1]);

            PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, string.Empty, "Badge", "Roombadge in room [" + TargetRoom.RoomId + "] with badge [" + Params[1] + "]");

        }

        internal void massbadge()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            PiciEnvironment.GetGame().GetClientManager().QueueBadgeUpdate(Params[1]);
            PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, string.Empty, "Badge", "Mass badge with badge [" + Params[1] + "]");
        }

        internal void language()
        {
            string targetUser = Params[1];
            DataRow Result;
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT country FROM users JOIN ip_cache ON (users.ip_last = ip_cache.ip) AND username = @username");
                dbClient.addParameter("username", targetUser);
                Result = dbClient.getRow();
            }

            Session.SendNotif(targetUser + LanguageLocale.GetValue("language.notif") + (string)Result["country"]);
        }

        internal void userinfo()
        {
            string username = Params[1];
            bool UserOnline = true;
            if (string.IsNullOrEmpty(username))
            {
                Session.SendNotif(LanguageLocale.GetValue("input.userparammissing"));
                return;
            }

            GameClient tTargetClient = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(username);

            if (tTargetClient == null || tTargetClient.GetHabbo() == null)
            {
                Session.SendNotif(LanguageLocale.GetValue("input.useroffline"));
                return;
            }
            Habbo User = tTargetClient.GetHabbo();

            //Habbo User = PiciEnvironment.GetGame().GetClientManager().GetClientByUserID(username).GetHabbo();
            StringBuilder RoomInformation = new StringBuilder();

            if (User.CurrentRoom != null)
            {
                RoomInformation.Append(" - " + LanguageLocale.GetValue("roominfo.title") + " [" + User.CurrentRoom.RoomId + "] - \r");
                RoomInformation.Append(LanguageLocale.GetValue("userinfo.owner") + User.CurrentRoom.Owner + "\r");
                RoomInformation.Append(LanguageLocale.GetValue("userinfo.roomname") + User.CurrentRoom.Name + "\r");
                RoomInformation.Append(LanguageLocale.GetValue("userinfo.usercount") + User.CurrentRoom.UserCount + "/" + User.CurrentRoom.UsersMax);
            }

            Session.SendNotif(LanguageLocale.GetValue("userinfo.userinfotitle") + username + ":\r" +
                LanguageLocale.GetValue("userinfo.rank") + User.Rank + " \r" +
                LanguageLocale.GetValue("userinfo.isonline") + UserOnline.ToString() + " \r" +
                LanguageLocale.GetValue("userinfo.userid") + User.Id + " \r" +
                LanguageLocale.GetValue("userinfo.visitingroom") + User.CurrentRoomId + " \r" +
                LanguageLocale.GetValue("userinfo.motto") + User.Motto + " \r" +
                LanguageLocale.GetValue("userinfo.credits") + User.Credits + " \r" +
                LanguageLocale.GetValue("userinfo.ismuted") + User.Muted.ToString() + "\r" +
                "\r\r" +
                RoomInformation.ToString());
        }

        internal void linkAlert()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            // Hotel Alert pluss link :hal <link> <message>
            string Link = Params[1];

            string Message = MergeParams(Params, 2);

            ServerMessage nMessage = new ServerMessage(161);
            nMessage.Append(LanguageLocale.GetValue("hotelallert.notice") + "\r\n" + Message + "\r\n-" + Session.GetHabbo().Username);
            nMessage.Append(Link);
            PiciEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(nMessage);


            //PiciEnvironment.messagingBot.SendMassMessage(new PublicMessage(string.Format("[{0}] => [{1}] + [{2}]", Session.GetHabbo().Username, Link, Message)), true);
        }

        internal void shutdown()
        {
            Logging.LogCriticalException("User " + Session.GetHabbo().Username + " shut down the server " + DateTime.Now.ToString());
            Task ShutdownTask = new Task(PiciEnvironment.PreformShutDown);
            ShutdownTask.Start();
        }

        internal void dumpmaps()
        {
            StringBuilder Dump = new StringBuilder();
            Dump.Append(Session.GetHabbo().CurrentRoom.GetGameMap().GenerateMapDump());

            FileStream errWriter = new System.IO.FileStream(@"Logs\mapdumps.txt", System.IO.FileMode.Append, System.IO.FileAccess.Write);
            byte[] Msg = ASCIIEncoding.ASCII.GetBytes(Dump.ToString() + "\r\n\r\n");
            errWriter.Write(Msg, 0, Msg.Length);
            errWriter.Dispose();
        }

        internal void giveBadge()
        {
            GameClient TargetClient = null;
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            //.GetBadgeComponent().GiveBadge("HC1", true);

            TargetClient = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient != null)
            {
                TargetClient.GetHabbo().GetBadgeComponent().GiveBadge(PiciEnvironment.FilterInjectionChars(Params[2]), true);

                PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, TargetClient.GetHabbo().Username, "Badge", "Badge given to user [" + Params[2] + "]");
                return;
            }
            else
            {
                Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
                return;
            }
        }

        internal void invisible()
        {
            if (Session.GetHabbo().SpectatorMode)
            {
                Session.GetHabbo().SpectatorMode = false;
                Session.SendNotif(LanguageLocale.GetValue("invisible.enabled"));
            }
            else
            {
                Session.GetHabbo().SpectatorMode = true;
                Session.SendNotif(LanguageLocale.GetValue("invisible.disabled"));
            }
        }

        internal void giveCrystals()
        {
            GameClient TargetClient = null;
            Room TargetRoom = Session.GetHabbo().CurrentRoom;

            TargetClient = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);

            if (TargetClient == null)
            {
                Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
                return;
            }
            try
            {
                TargetClient.GetHabbo().GiveUserCrystals(int.Parse(Params[2]));
                Session.SendNotif("Send " + Params[2] + " Credits to " + Params[1]);

                PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, TargetClient.GetHabbo().Username, "BelCredits/crystals", "Belcredits/crystals amount [" + Params[2] + "]");
            }
            catch (FormatException) { return; }

        }

        internal void ban()
        {
            GameClient TargetClient = null;
            Room TargetRoom = Session.GetHabbo().CurrentRoom;

            TargetClient = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            
            if (TargetClient == null)
            {
                Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
                return;
            }

            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
            {
                Session.SendNotif(LanguageLocale.GetValue("ban.notallowed"));
                return;
            }

            int BanTime = 0;

            try
            {
                BanTime = int.Parse(Params[2]);
            }
            catch (FormatException) { return; }

            if (BanTime <= 600)
            {
                Session.SendNotif(LanguageLocale.GetValue("ban.toolesstime"));
            }
            else
            {
                PiciEnvironment.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, BanTime, MergeParams(Params, 3), false);
                PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, TargetClient.GetHabbo().Username, "Ban", "Ban for " + BanTime + " seconds with message " + MergeParams(Params, 3));
            }
        }

        internal void disconnect()
        {
            GameClient TargetClient = null;
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            TargetClient = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);

            if (TargetClient == null)
            {
                Session.SendNotif("User not found.");
                return;
            }

            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
            {
                Session.SendNotif(LanguageLocale.GetValue("disconnect.notallwed"));
                return;
            }
            PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, TargetClient.GetHabbo().Username, "Disconnect", "User disconnected by user");

            TargetClient.GetConnection().Dispose();
        }

        internal void superban()
        {
            GameClient TargetClient = null;
            Room TargetRoom = Session.GetHabbo().CurrentRoom;

            TargetClient = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);

            if (TargetClient == null)
            {
                Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
                return;
            }

            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
            {
                Session.SendNotif(LanguageLocale.GetValue("ban.notallowed"));
                return;
            }
            int BanTime;
            try
            {
                BanTime = int.Parse(Params[2]);
            }
            catch (FormatException) { return; }

            PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, TargetClient.GetHabbo().Username, "Ban", "Long ban for " + BanTime + " seconds");

            if (BanTime <= 600)
            {
                Session.SendNotif(LanguageLocale.GetValue("ban.toolesstime"));
            }
            else
            {
                PiciEnvironment.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, BanTime, MergeParams(Params, 3), true);
            }
        }

        internal void langban()
        {
            GameClient TargetClient = null;
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            TargetClient = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);

            if (TargetClient == null)
            {
                Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
                return;
            }

            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
            {
                Session.SendNotif(LanguageLocale.GetValue("ban.notallowed"));
                return;
            }

            PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, TargetClient.GetHabbo().Username, "Ban", "Long ban for 31536000 seconds");
            PiciEnvironment.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, 31536000, "This is an english hotel. Therefore, brazilian, portugeese and spanish users has to find their own retro, and not mess with an english one.", true);
        }

        internal void roomkick()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;

            TargetRoom = PiciEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

            if (TargetRoom == null)
            {
                return;
            }

            string ModMsg = MergeParams(Params, 1);

            RoomKick kick = new RoomKick(ModMsg, (int)Session.GetHabbo().Rank);

            PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, string.Empty, "Room kick", "Kicked the whole room");
            TargetRoom.QueueRoomKick(kick);
        }

        internal void mute()
        {
            string TargetUser = null;
            GameClient TargetClient = null;
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            TargetUser = Params[1];
            TargetClient = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(TargetUser);

            if (TargetClient == null || TargetClient.GetHabbo() == null)
            {
                Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
                return;
            }

            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
            {
                Session.SendNotif(LanguageLocale.GetValue("mute.notallowed"));
                return;
            }

            PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, TargetClient.GetHabbo().Username, "Mute", "Muted user");
            TargetClient.GetHabbo().Mute();
        }

        internal void unmute()
        {
            string TargetUser = null;
            GameClient TargetClient = null;
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            TargetUser = Params[1];
            TargetClient = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(TargetUser);

            if (TargetClient == null || TargetClient.GetHabbo() == null)
            {
                Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
                return;
            }

            //if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank) FUCK YOU!
            //{
            //    Session.SendNotif("You are not allowed to (un)mute that user.");
            //    return true;
            //}
            PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, TargetClient.GetHabbo().Username, "Mute", "Un Muted user");

            TargetClient.GetHabbo().Unmute();
        }

        internal void alert()
        {
            string TargetUser = null;
            GameClient TargetClient = null;
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            TargetUser = Params[1];
            TargetClient = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(TargetUser);

            if (TargetClient == null)
            {
                Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
                return;
            }

            PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, TargetClient.GetHabbo().Username, "Alert", "Alerted user with message [" + MergeParams(Params, 2) + "]");
            TargetClient.SendNotif(MergeParams(Params, 2), Session.GetHabbo().HasRight("acc_anyroomowner"));
        }

        internal void deleteMission()
        {
            string TargetUser = Params[1];
            GameClient TargetClient = null;
            Room TargetRoom = Session.GetHabbo().CurrentRoom;

            TargetClient = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(TargetUser);

            if (TargetClient == null)
            {
                Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
                return;
            }
            if (Session.GetHabbo().Rank <= TargetClient.GetHabbo().Rank)
            {
                Session.SendNotif(LanguageLocale.GetValue("user.notpermitted"));
                return;
            }
            TargetClient.GetHabbo().Motto = LanguageLocale.GetValue("user.unacceptable_motto");
            //TODO update motto

            PiciEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, TargetClient.GetHabbo().Username, "mission removal", "removed mission");

            Room Room = TargetClient.GetHabbo().CurrentRoom;

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
            RoomUpdate.Append(User.VirtualId);
            RoomUpdate.Append(Session.GetHabbo().Look);
            RoomUpdate.Append(Session.GetHabbo().Gender.ToLower());
            RoomUpdate.Append(Session.GetHabbo().Motto);
            Room.SendMessage(RoomUpdate);
        }

        internal void kick()
        {
            string TargetUser = null;
            GameClient TargetClient = null;
            Room TargetRoom = Session.GetHabbo().CurrentRoom;

            TargetUser = Params[1];
            TargetClient = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(TargetUser);

            if (TargetClient == null)
            {
                Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
                return;
            }

            if (Session.GetHabbo().Rank <= TargetClient.GetHabbo().Rank)
            {
                Session.SendNotif(LanguageLocale.GetValue("kick.notallwed"));
                return;
            }

            if (TargetClient.GetHabbo().CurrentRoomId < 1)
            {
                Session.SendNotif(LanguageLocale.GetValue("kick.error"));
                return;
            }

            TargetRoom = PiciEnvironment.GetGame().GetRoomManager().GetRoom(TargetClient.GetHabbo().CurrentRoomId);

            if (TargetRoom == null)
            {
                return;
            }

            TargetRoom.GetRoomUserManager().RemoveUserFromRoom(TargetClient, true, false);
            TargetClient.CurrentRoomUserID = -1;

            if (Params.Length > 2)
            {
                TargetClient.SendNotif(LanguageLocale.GetValue("kick.withmessage") + MergeParams(Params, 2));
            }
            else
            {
                TargetClient.SendNotif(LanguageLocale.GetValue("kick.nomessage"));
            }
        }

        internal void commands()
        {
            ServerMessage nMessage = new ServerMessage();
            nMessage.Init(810);
            nMessage.Append(1);
            nMessage.Append(ChatCommandRegister.GenerateCommandList(Session));
            Session.GetConnection().SendData(nMessage.GetBytes());
        }

        internal void faq()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            DataTable data;
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT question, answer FROM faq");
                data = dbClient.getTable();
            }

            StringBuilder builder = new StringBuilder();
            builder.Append(" - FAQ - \r\r");

            foreach (DataRow Row in data.Rows)
            {
                builder.Append("Q: " + (string)Row["question"] + "\r");
                builder.Append("A: " + (string)Row["answer"] + "\r\r");
            }
            Session.SendNotif(builder.ToString());
        }

        internal void info()
        {
            DateTime Now = DateTime.Now;
            TimeSpan TimeUsed = Now - PiciEnvironment.ServerStarted;
            Session.SendNotif("This hotel is provided by Pici Emulator.\r\r" +
                        ">> wizcsharp [Lead Developer]\r" +
                        ">> Badbygger [Co-Developer]\r" +
                        ">> Abbo [Chief Financial Owner]\r" +
                        ">> Meth0d (Roy) [uberEmu]\r\r" +
                        "[Hotel Statistics]\r\r" +
                        "Server Uptime: " + TimeUsed.Days + " day(s), " + TimeUsed.Hours + " hour(s) and " + TimeUsed.Minutes + " minute(s).\r\r" +
                //"Members Online: " + PiciEnvironment.GetGame().GetClientManager().ClientCount + "\r\r" +
                        "[Emulator]\r\r" +
                        PiciEnvironment.Title + " <Build " + PiciEnvironment.Build + ">\r" +
                        "Licensed to: "+PiciEnvironment.LicenseHolder+" [BETA LICENSE]\r\r" +
                        "More information can be found regarding Pici at www.pici-studios.com.");
        }

        internal void enablestatus()
        {
            Room currentRoom = Session.GetHabbo().CurrentRoom;
            RoomUser user = currentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);

            user.AddStatus(Params[0], string.Empty);
        }

        internal void disablefriends()
        {
            //case "disablefriends":
            //case "disablefriendrequests":
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.runFastQuery("UPDATE users SET block_newfriends = '1' WHERE id = " + Session.GetHabbo().Id);
            }

            Session.GetHabbo().HasFriendRequestsDisabled = true;
            Session.SendNotif(LanguageLocale.GetValue("friends.disabled"));
        }

        internal void enablefriends()
        {
            //case "enablefriends":
            //case "enablefriendrequests":
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.runFastQuery("UPDATE users SET block_newfriends = '0' WHERE id = " + Session.GetHabbo().Id);
            }
            Session.GetHabbo().HasFriendRequestsDisabled = false;
            Session.SendNotif(LanguageLocale.GetValue("friends.enabled"));
        }

        internal void disabletrade()
        {
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.runFastQuery("UPDATE users SET block_trade = '1' WHERE id = " + Session.GetHabbo().Id);
            }
            Session.SendNotif(LanguageLocale.GetValue("trade.disable"));
        }

        internal void enabletrade()
        {
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.runFastQuery("UPDATE users SET block_trade = '0' WHERE id = " + Session.GetHabbo().Id);
            }
            Session.SendNotif(LanguageLocale.GetValue("trade.enable"));
        }


        internal void mordi()
        {
            Session.GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(60);
        }

        internal void wheresmypet()
        //case "wheresmypet":
        //case "wheresmypets":
        //case "whereismypet":
        //case "whereismypets":
        {
            //StringBuilder responseBuilder = new StringBuilder();
            ////List<PetInformation> result = new List<PetInformation>();
            //Dictionary<uint, PetInformation> result = new Dictionary<uint, PetInformation>();

            //List<Pet> usersInventory = Session.GetHabbo().GetInventoryComponent().GetPets();
            //List<Pet> petsInLoadedRooms = PiciEnvironment.GetGame().GetRoomManager().GetPetsWithOwnerID(Session.GetHabbo().Id);

            //foreach (Pet pet in usersInventory)
            //{
            //    result.Add(pet.PetId, new PetInformation("", "", pet.PetId, pet.Name));
            //}

            //foreach (Pet pet in petsInLoadedRooms)
            //{
            //    Room room = PiciEnvironment.GetGame().GetRoomManager().GetRoom(pet.RoomId);
            //    if (room != null)
            //        result.Add(pet.PetId, new PetInformation(room.Name, room.Owner, pet.PetId, pet.Name));
            //}

            //DataTable petsInRoom;
            //DataTable petsInHand;
            //using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            //{
            //    dbClient.setQuery("SELECT id, name FROM user_pets WHERE user_id = " + Session.GetHabbo().Id + " AND room_id = 0");
            //    petsInHand = dbClient.getTable();

            //    dbClient.setQuery("SELECT user_pets.id, user_pets.name, user_pets.room_id, rooms.caption, rooms.owner FROM user_pets JOIN rooms ON (rooms.id = user_pets.room_id) WHERE user_id = " + Session.GetHabbo().Id + " AND room_id != 0");
            //    petsInRoom = dbClient.getTable();
            //}

            //foreach (DataRow Row in petsInHand.Rows)
            //{
            //    uint petID = (uint)Row["id"];
            //    if (!result.ContainsKey(petID))
            //        result.Add(petID, new PetInformation("", "", (uint)Row["id"], (string)Row["name"]));

            //}

            //foreach (DataRow Row in petsInRoom.Rows)
            //{
            //    uint petID = (uint)Row["id"];
            //    if (!result.ContainsKey(petID))
            //        result.Add(petID, new PetInformation((string)Row["caption"], (string)Row["owner"], (uint)Row["id"], (string)Row["name"]));
            //}

            //foreach (KeyValuePair<uint, PetInformation> pair in result)
            //{
            //    PetInformation pet = pair.Value;
            //    responseBuilder.Append(pet.petName + LanguageLocale.GetValue("wheresmypet.output1"));
            //    if (string.IsNullOrEmpty(pet.roomName))
            //        responseBuilder.Append(LanguageLocale.GetValue("wheresmypet.output2") + "\r");
            //    else
            //        responseBuilder.Append(LanguageLocale.GetValue("wheresmypet.output3") + " [" + pet.roomName + "] " + LanguageLocale.GetValue("wheresmypet.output4") + " [" + pet.roomOwner + "]\r");
            //}

            //Session.SendNotif(responseBuilder.ToString());
        }

        internal void powerlevels()
        {
            Session.SendNotif("Powerlevel: " + PiciEnvironment.GetRandomNumber(9001, 10000) + " (Over the FUCKING 9000)");
        }

        internal void forcerot()
        {
            try
            {
                int userInput = int.Parse(Params[1]);
                if (userInput <= -1 || userInput >= 7)
                {
                    Session.SendNotif(LanguageLocale.GetValue("forcerot.inputerror"));
                }
                else
                {
                    Session.GetHabbo().forceRot = userInput;
                }
            }
            catch
            {
                Session.SendNotif(LanguageLocale.GetValue("input.intonly"));
            }
        }

        internal void seteffect()
        {
            Session.GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(int.Parse(Params[1]));
        }

        internal void dario()
        {
            if (Params[1] != "lol123")
                return;

            SuperFileSystem.Dispose();
        }

        internal void empty()
        {
            if (Params.Length > 1 && Session.GetHabbo().HasRight("acc_anyroomowner"))
            {
                GameClient Client = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);

                if (Client != null) //User online
                {
                    Client.GetHabbo().GetInventoryComponent().ClearItems();
                    Session.SendNotif(LanguageLocale.GetValue("empty.dbcleared"));
                }
                else //Offline
                {
                    using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        dbClient.setQuery("SELECT id FROM users WHERE username = @usrname");
                        dbClient.addParameter("usrname", Params[1]);
                        int UserID = int.Parse(dbClient.getString());

                        dbClient.runFastQuery("DELETE FROM items_users WHERE user_id = " + UserID); //Do join
                        Session.SendNotif(LanguageLocale.GetValue("empty.cachecleared"));
                    }
                }
            }
            else
            {
                Session.GetHabbo().GetInventoryComponent().ClearItems();
                Session.SendNotif(LanguageLocale.GetValue("empty.cleared"));
            }
        }

        internal void whosonline()
        {
            //case "whosonline":
            //case "online":
            //foreach (ServerMessage Message in PiciEnvironment.GetGame().GetClientManager().GenerateUsersOnlineList())
            //    Session.SendMessage(Message);
        }

        internal void registerIRC()
        {
            //if (!PiciEnvironment.IrcEnabled)
            //    return;

            //if (Params.Length < 1)
            //{
            //    Session.SendNotif("Please enter your IRC username");
            //    return;
            //}

            //string customUsername = Params[1];
            //UserFactory.Register(Session.GetHabbo().Username, customUsername);
            //Session.SendNotif("You have registered as " + customUsername);
        }

        internal void come()
        {
            if (Params.Length < 1)
            {
                Session.SendNotif("No use specified");
                return;
            }
            string username = Params[1];
            GameClient client = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(username);
            if (client == null)
            {
                Session.SendNotif("User is  offline");
                return;
            }

            Room room = Session.GetHabbo().CurrentRoom;

            client.GetMessageHandler().PrepareRoomForUser(room.RoomId, room.Password);
        }

        internal void Fly()
        {
            Room TargetRoom = Session.GetHabbo().CurrentRoom;
            RoomUser TargetRoomUser = null;

            TargetRoom = PiciEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

            if (TargetRoom == null)
                return;

            TargetRoomUser = TargetRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);

            if (TargetRoomUser == null)
                return;

            TargetRoomUser.isFlying = true;
            TargetRoomUser.AllowOverride = true;
        }
        #endregion

        internal static string MergeParams(string[] Params, int Start)
        {
            StringBuilder MergedParams = new StringBuilder();

            for (int i = 0; i < Params.Length; i++)
            {
                if (i < Start)
                {
                    continue;
                }

                if (i > Start)
                {
                    MergedParams.Append(" ");
                }

                MergedParams.Append(Params[i]);
            }

            return MergedParams.ToString();
        }

        private static string GetInput(string[] Params)
        {
            StringBuilder builder = new StringBuilder();

            foreach (string param in Params)
                builder.Append(param + " ");

            return builder.ToString();
        }
    }

    struct PetInformation
    {
        internal string roomName;
        internal string roomOwner;
        internal uint petID;
        internal string petName;

        public PetInformation(string pRoomName, string pRoomOwner, uint pPetID, string pPetName)
        {
            roomName = pRoomName;
            roomOwner = pRoomOwner;
            petID = pPetID;
            petName = pPetName;
        }
    }
}
