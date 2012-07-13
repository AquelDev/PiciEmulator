using System;
using System.Collections.Generic;
using System.Linq;
using Pici.Core;
using Pici.HabboHotel.Misc;
using Pici.HabboHotel.Pathfinding;
using Pici.HabboHotel.Users;
using Pici.HabboHotel.Users.UserDataManagement;
using Pici.Messages;
using Pici.Net;
using Pici.Util;
using Pici.Core.ConnectionManager;
using System.Drawing;
using System.Reflection;
using Pici.Messages.Interfaces;
using System.IO;

namespace Pici.HabboHotel.GameClients
{
    class GameClient
    {
        private uint Id;

        private ConnectionInformation Connection;
        private GameClientMessageHandler MessageHandler;

        private Habbo Habbo;

        internal DateTime TimePingedReceived;
        internal DateTime TimePingSent;
        internal DateTime TimePingLastControl;

        internal bool SetDoorPos;
        internal Point newDoorPos;
        private GamePacketParser packetParser;

        internal uint ConnectionID
        {
            get
            {
                return Id;
            }
        }

        internal int CurrentRoomUserID;

        /// <summary>
        /// Storage of names from Info Events.
        /// </summary>
        public Dictionary<string, short> InfoEvents;

        /// <summary>
        /// Storage of Invokers from Message Events
        /// </summary>
        public Dictionary<int, Event> MessageEvents;

        internal GameClient(uint ClientId, ConnectionInformation pConnection)
        {
            Id = ClientId;
            Connection = pConnection;
            SetDoorPos = false;
            CurrentRoomUserID = -1;
            packetParser = new GamePacketParser();
        }

        /// <summary>
        /// Returns an header from an InfoEvent.
        /// </summary>
        /// <param name="Event"></param>
        /// <returns></returns>
        public short GetHeader(Event Event)
        {
            using (DictionaryAdapter<string, short> DA = new DictionaryAdapter<string, short>(InfoEvents))
            {
                return DA.TryPopValue(Event.GetType().Name);
            }
        }

        /// <summary>
        /// Returns an name of an InfoEvent.
        /// </summary>
        /// <param name="Header"></param>
        /// <returns></returns>
        public string GetName(short Header)
        {
            using (DictionaryAdapter<string, short> DA = new DictionaryAdapter<string, short>(InfoEvents))
            {
                return DA.TryPopKey(Header);
            }
        }

        void SwitchParserRequest()
        {
            if (MessageHandler == null)
            {
                InitHandler();
            }
            packetParser.SetConnection(Connection);
            packetParser.onNewPacket += new GamePacketParser.HandlePacket(parser_onNewPacket);
            byte[] data = (Connection.parser as InitialPacketParser).currentData;
            Connection.parser.Dispose();
            Connection.parser = packetParser;
            Connection.parser.handlePacketData(data);
        }

        void parser_onNewPacket(ClientMessage Message)
        {
            try
            {
                MessageHandler.HandleRequest(Message);
            }
            catch (Exception e) { Logging.LogPacketException(Message.ToString(), e.ToString()); }
        }

        void PolicyRequest()
        {
            Connection.SendData(PiciEnvironment.GetDefaultEncoding().GetBytes(CrossdomainPolicy.GetXmlPolicy()));
        }

        internal ConnectionInformation GetConnection()
        {
            return Connection;
        }

        internal GameClientMessageHandler GetMessageHandler()
        {
            return MessageHandler;
        }

        internal bool gotTheThing = false;

        internal Habbo GetHabbo()
        {
            return Habbo;
        }

        internal void StartConnection()
        {
            if (Connection == null)
            {
                return;
            }

            TimePingedReceived = DateTime.Now;
            
            (Connection.parser as InitialPacketParser).PolicyRequest += new InitialPacketParser.NoParamDelegate(PolicyRequest);
            (Connection.parser as InitialPacketParser).SwitchParserRequest += new InitialPacketParser.NoParamDelegate(SwitchParserRequest);

            Connection.startPacketProcessing();
        }

        internal void InitHandler()
        {
            MessageHandler = new GameClientMessageHandler(this);
        }

        internal bool tryLogin(string AuthTicket)
        {
            try
            {
                string ip = GetConnection().getIp();
                byte errorCode = 0;
                UserData userData = UserDataFactory.GetUserData(AuthTicket, ip, out errorCode);
                if (errorCode == 1)
                {
                    SendNotifWithScroll(LanguageLocale.GetValue("login.invalidsso"));
                    return false;
                }
                else if (errorCode == 2)
                {
                    SendNotifWithScroll(LanguageLocale.GetValue("login.loggedin"));
                    return false;
                }

                PiciEnvironment.GetGame().GetClientManager().RegisterClient(this, userData.userID, userData.user.Username);
                this.Habbo = userData.user;
                userData.user.LoadData(userData);

                if (userData.user.Username == null)
                {
                    SendBanMessage("You have no username.");
                    return false;
                }
                string banReason = PiciEnvironment.GetGame().GetBanManager().GetBanReason(userData.user.Username, ip);
                if (!string.IsNullOrEmpty(banReason))
                {
                    SendBanMessage(banReason);
                    return false;
                }

                userData.user.Init(this, userData);

                QueuedServerMessage response = new QueuedServerMessage(Connection);

                userData.user.SerializeQuests(ref response);

                GetMessageHandler().GetResponse().Init(2);
                if (userData.user.HasRight("acc_anyroomowner"))
                    GetMessageHandler().GetResponse().AppendInt32(7);
                else if (userData.user.HasRight("acc_anyroomrights"))
                    GetMessageHandler().GetResponse().AppendInt32(5);
                else if (userData.user.HasRight("acc_supporttool"))
                    GetMessageHandler().GetResponse().AppendInt32(4);
                else if (GetHabbo().GetSubscriptionManager().HasSubscription("habbo_vip"))
                    GetMessageHandler().GetResponse().AppendInt32(2);
                else if (GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
                    GetMessageHandler().GetResponse().AppendInt32(1);
                else
                    GetMessageHandler().GetResponse().AppendInt32(0);

                GetMessageHandler().SendResponse();

                if (userData.user.HasRight("acc_supporttool"))
                {
                    SendMessage(PiciEnvironment.GetGame().GetModerationTool().SerializeTool());
                    PiciEnvironment.GetGame().GetModerationTool().SerializeOpenTickets(this);
                }

                SendMessage(userData.user.GetAvatarEffectsInventoryComponent().Serialize());

                GetMessageHandler().GetResponse().Init(290);
                GetMessageHandler().GetResponse().AppendBoolean(true);
                GetMessageHandler().GetResponse().AppendBoolean(false);
                GetMessageHandler().SendResponse();

                GetMessageHandler().GetResponse().Init(3);
                GetMessageHandler().SendResponse();

                GetMessageHandler().GetResponse().Init(517);
                GetMessageHandler().GetResponse().AppendBoolean(true);
                GetMessageHandler().SendResponse();

                //if (PixelManager.NeedsUpdate(this))
                //    PixelManager.GivePixels(this);

                if (PiciEnvironment.GetGame().GetClientManager().pixelsOnLogin > 0)
                {
                    PixelManager.GivePixels(this, PiciEnvironment.GetGame().GetClientManager().pixelsOnLogin);
                }

                

                if (PiciEnvironment.GetGame().GetClientManager().creditsOnLogin > 0)
                {
                    userData.user.Credits += PiciEnvironment.GetGame().GetClientManager().creditsOnLogin;
                    userData.user.UpdateCreditsBalance();
                }

                if (userData.user.HomeRoom > 0)
                {
                    GetMessageHandler().GetResponse().Init(455);
                    GetMessageHandler().GetResponse().AppendUInt(userData.user.HomeRoom);
                    GetMessageHandler().SendResponse();
                }

                GetMessageHandler().GetResponse().Init(458);
                GetMessageHandler().GetResponse().AppendInt32(30);
                GetMessageHandler().GetResponse().AppendInt32(userData.user.FavoriteRooms.Count);

                foreach (uint Id in userData.user.FavoriteRooms.ToArray())
                {
                    GetMessageHandler().GetResponse().AppendUInt(Id);
                }

                GetMessageHandler().SendResponse();

                if (!userData.user.GetBadgeComponent().HasBadge("ACH_BasicClub1"))
                {
                    userData.user.GetBadgeComponent().GiveBadge("ACH_BasicClub1", true);
                }
                else if (userData.user.GetBadgeComponent().HasBadge("ACH_BasicClub1"))
                {
                    userData.user.GetBadgeComponent().RemoveBadge("ACH_BasicClub1");
                }


                if (!userData.user.GetBadgeComponent().HasBadge("Z63"))
                    userData.user.GetBadgeComponent().GiveBadge("Z63", true);

                if (userData.user.Rank > 5 && !userData.user.GetBadgeComponent().HasBadge("ADM"))
                    userData.user.GetBadgeComponent().GiveBadge("ADM", true);


                ServerMessage alert = new ServerMessage(810);
                alert.AppendUInt(1);
                alert.AppendStringWithBreak(LanguageLocale.welcomeAlert);
                SendMessage(alert);
                Logging.WriteLine("[" + Habbo.Username + "] logged in");

                return true;
            }
            catch (UserDataNotFoundException e)
            {
                SendNotifWithScroll(LanguageLocale.GetValue("login.invalidsso") + "extra data: " + e.ToString());
            }
            catch (Exception e)
            {
                Logging.LogCriticalException("Invalid Dario bug duing user login: " + e.ToString());
                //SendNotif("Login error: " + e.ToString());
                SendNotifWithScroll("Login error: " + e.ToString());
            }
            return false;
        }

       /* internal bool tryLogin(string AuthTicket)
        {
            try
            {
                string ip = GetConnection().getIp();
                byte errorCode = 0;
                UserData userData = UserDataFactory.GetUserData(AuthTicket, ip, out errorCode);
                if (errorCode == 1)
                {
                    SendNotifWithScroll(LanguageLocale.GetValue("login.invalidsso"));
                    return false;
                }
                else if (errorCode == 2)
                {
                    SendNotifWithScroll(LanguageLocale.GetValue("login.loggedin"));
                    return false;
                }

                PiciEnvironment.GetGame().GetClientManager().RegisterClient(this, userData.userID, userData.user.Username);
                this.Habbo = userData.user;
                userData.user.LoadData(userData);

                if (userData.user.Username == null)
                {
                    SendBanMessage("You have no username.");
                    return false;
                }
                string banReason = PiciEnvironment.GetGame().GetBanManager().GetBanReason(userData.user.Username, ip);
                if (!string.IsNullOrEmpty(banReason))
                {
                    SendBanMessage(banReason);
                    return false;
                }

                userData.user.Init(this, userData);

                QueuedServerMessage response = new QueuedServerMessage(Connection);

                userData.user.SerializeQuests(ref response);

                //List<string> Rights = PiciEnvironment.GetGame().GetRoleManager().GetRightsForHabbo(userData.user);

                GetMessageHandler().GetResponse().Init(2);
                if (userData.user.HasRight("acc_anyroomowner"))
                    GetMessageHandler().GetResponse().Append(7);
                else if (userData.user.HasRight("acc_anyroomrights"))
                    GetMessageHandler().GetResponse().Append(5);
                else if (userData.user.HasRight("acc_supporttool"))
                    GetMessageHandler().GetResponse().Append(4);
                else if (GetHabbo().GetSubscriptionManager().HasSubscription("habbo_vip"))
                    GetMessageHandler().GetResponse().AppendInt32(2);
                else if (GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
                    GetMessageHandler().GetResponse().AppendInt32(1);
                else
                    GetMessageHandler().GetResponse().AppendInt32(0);

                GetMessageHandler().SendResponse();
                /*foreach (string Right in Rights)
                {
                    appendingResponse.AppendStringWithBreak(Right);
                }

                if (userData.user.HasRight("acc_supporttool"))
                {
                    SendMessage(PiciEnvironment.GetGame().GetModerationTool().SerializeTool());
                    PiciEnvironment.GetGame().GetModerationTool().SerializeOpenTickets(this);
                }

                SendMessage(userData.user.GetAvatarEffectsInventoryComponent().Serialize());

                //GetResponse().Init(290);
                GetMessageHandler().GetResponse().Init(290);
                GetMessageHandler().GetResponse().AppendBoolean(true);
                GetMessageHandler().GetResponse().AppendBoolean(false);
                GetMessageHandler().SendResponse();

                GetMessageHandler().GetResponse().Init(3);
                GetMessageHandler().SendResponse();

                GetMessageHandler().GetResponse().Init(517);
                GetMessageHandler().GetResponse().AppendBoolean(true);
                GetMessageHandler().SendResponse();

                //if (PixelManager.NeedsUpdate(this))
                //    PixelManager.GivePixels(this);

                if (PiciEnvironment.GetGame().GetClientManager().pixelsOnLogin > 0)
                {
                    PixelManager.GivePixels(this, PiciEnvironment.GetGame().GetClientManager().pixelsOnLogin);
                }

                if (PiciEnvironment.GetGame().GetClientManager().creditsOnLogin > 0)
                {
                    userData.user.Credits += PiciEnvironment.GetGame().GetClientManager().creditsOnLogin;
                    userData.user.UpdateCreditsBalance();
                }

                if (userData.user.HomeRoom > 0)
                {
                    GetMessageHandler().GetResponse().Init(455);
                    GetMessageHandler().GetResponse().AppendUInt(userData.user.HomeRoom);
                    GetMessageHandler().SendResponse();
                }

                GetMessageHandler().GetResponse().Init(458);
                GetMessageHandler().GetResponse().AppendInt32(30);
                GetMessageHandler().GetResponse().AppendInt32(userData.user.FavoriteRooms.Count);

                foreach (uint Id in userData.user.FavoriteRooms.ToArray())
                {
                    GetMessageHandler().GetResponse().AppendUInt(Id);
                }

                GetMessageHandler().SendResponse();

                if (!userData.user.GetBadgeComponent().HasBadge("ACH_BasicClub1"))
                {
                    userData.user.GetBadgeComponent().GiveBadge("ACH_BasicClub1", true);
                }
                else if (userData.user.GetBadgeComponent().HasBadge("ACH_BasicClub1"))
                {
                    userData.user.GetBadgeComponent().RemoveBadge("ACH_BasicClub1");
                }


                if (!userData.user.GetBadgeComponent().HasBadge("Z63"))
                    userData.user.GetBadgeComponent().GiveBadge("Z63", true);

                ServerMessage alert = new ServerMessage(810);
                alert.AppendUInt(1);
                alert.AppendStringWithBreak("Welcome bro!");
                SendMessage(alert);

                return true;
            }
            catch (UserDataNotFoundException e)
            {
                SendNotifWithScroll(LanguageLocale.GetValue("login.invalidsso") + "extra data: " + e.ToString());
            }
            catch (Exception e)
            {
                Logging.LogCriticalException("Invalid Dario bug duing user login: " + e.ToString());
                //SendNotif("Login error: " + e.ToString());
                SendNotifWithScroll("Login error: " + e.ToString());
            }
            return false;
        }
        */
        internal void SendNotifWithScroll(string message)
        {
            ServerMessage notification = new ServerMessage(810);
            notification.AppendUInt(1);
            notification.AppendStringWithBreak(message);

            SendMessage(notification);
        }

        internal void SendBanMessage(string Message)
        {
            ServerMessage BanMessage = new ServerMessage(35);
            BanMessage.AppendStringWithBreak(LanguageLocale.GetValue("moderation.banmessage"), 13);
            BanMessage.AppendStringWithBreak(Message);
            GetConnection().SendData(BanMessage.GetBytes());
        }

        internal void SendNotif(string Message)
        {
            SendNotif(Message, false);
        }

        internal void SendNotif(string Message, Boolean FromHotelManager)
        {
            ServerMessage nMessage = new ServerMessage();

            if (FromHotelManager)
            {
                nMessage.Init(139);
            }
            else
            {
                nMessage.Init(161);
            }

            nMessage.AppendStringWithBreak(Message);
            GetConnection().SendData(nMessage.GetBytes());
        }

        internal void Stop()
        {
            if (GetMessageHandler() != null)
                MessageHandler.Destroy();

            if (GetHabbo() != null)
                Habbo.OnDisconnect();
            CurrentRoomUserID = -1;

            this.MessageHandler = null;
            this.Habbo = null;
            this.Connection = null;
        }

        private bool Disconnected = false;

        internal void Disconnect()
        {
            if (GetHabbo() != null && GetHabbo().GetInventoryComponent() != null)
                GetHabbo().GetInventoryComponent().RunDBUpdate();
            if (!Disconnected)
            {
                if (Connection != null)
                    Connection.Dispose();
                Disconnected = true;
            }
        }

        internal void HandleConnectionData(ref byte[] data)
        {
            if (data[0] == 64)
            {
                int pos = 0;

                while (pos < data.Length)
                {
                    try
                    {
                        int MessageLength = Base64Encoding.DecodeInt32(new byte[] { data[pos++], data[pos++], data[pos++] });
                        int MessageId = Base64Encoding.DecodeInt32(new byte[] { data[pos++], data[pos++] });

                        byte[] Content = new byte[MessageLength - 2];

                        for (int i = 0; i < Content.Length; i++)
                        {
                            Content[i] = data[pos++];
                        }

                        ClientMessage Message = new ClientMessage(MessageId, Content);

                        Console.WriteLine("[Request] >> [" + MessageId + "] " + Message.ToString());

                        if (MessageHandler == null)
                        {
                            InitHandler();
                        }

                        //DateTime PacketMsgStart = DateTime.Now;
                    }
                    catch (Exception e)
                    {
                        Logging.HandleException(e, "packet handling");
                        Disconnect();
                    }
                }
            }
            else
            {
                Connection.SendData(PiciEnvironment.GetDefaultEncoding().GetBytes(CrossdomainPolicy.GetXmlPolicy()));
            }
        }

        internal void SendMessage(ServerMessage Message)
        {
            if (Message == null)
                return;
            if (GetConnection() == null)
                return;
            GetConnection().SendData(Message.GetBytes());
        }

        internal void UnsafeSendMessage(ServerMessage Message)
        {
            if (Message == null)
                return;
            if (GetConnection() == null)
                return;
            GetConnection().SendUnsafeData(Message.GetBytes());
        }
    }
}
