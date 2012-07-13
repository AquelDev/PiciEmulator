using Butterfly.HabboHotel.GameClients;
using Butterfly.HabboHotel.Rooms;
using Butterfly.Messages;

namespace Butterfly.HabboHotel.Users.Messenger
{
    class MessengerBuddy
    {
        #region Fields
        private readonly uint UserId;
        private readonly string mUsername;
        private readonly string mLook;
        private readonly string mMotto;
        private readonly string mLastOnline;

        private GameClient client;
        private Room currentRoom;
        #endregion

        #region Return values
        internal uint Id
        {
            get
            {
                return UserId;
            }
        }
        
        internal bool IsOnline
        {
            get
            {
                return (client != null && client.GetHabbo() != null && client.GetHabbo().GetMessenger() != null && !client.GetHabbo().GetMessenger().AppearOffline);
            }
        }

        private GameClient Client
        {
            get
            {
                return client;
            }
            set
            {
                client = value;
            }
        }

        internal bool InRoom
        {
            get
            {
                return (currentRoom != null);
            }
        }
        
        internal Room CurrentRoom
        {
            get
            {
                return currentRoom;
            }
            set
            {
                currentRoom = value;
            }
        }
        #endregion

        #region Constructor
        internal MessengerBuddy(uint UserId, string pUsername, string pLook, string pMotto, string pLastOnline)
        {
            this.UserId = UserId;
            this.mUsername = pUsername;
            this.mLook = pLook;
            this.mMotto = pMotto;
            this.mLastOnline = pLastOnline;
        }
        #endregion

        #region Methods
        internal void UpdateUser()
        {
            client = ButterflyEnvironment.GetGame().GetClientManager().GetClient(UserId);
            UpdateUser(client);
        }

        internal void UpdateUser(GameClient client)
        {
            this.client = client;
            if (client != null && client.GetHabbo() != null)
                currentRoom = client.GetHabbo().CurrentRoom;
        }

        internal void Serialize(ServerMessage reply)
        {
            /*
             Message.AppendUInt(UserId);
                Message.AppendStringWithBreak(Username);
                Message.AppendBoolean(true);
                Message.AppendBoolean(IsOnline);
                Message.AppendBoolean(InRoom);
                Message.AppendStringWithBreak(Look);
                Message.AppendBoolean(false);
                Message.AppendStringWithBreak(Motto);
                Message.AppendStringWithBreak(LastOnline);
                Message.AppendStringWithBreak(RealName);
                Message.AppendStringWithBreak("");
             */


            reply.AppendUInt(UserId);
            reply.AppendStringWithBreak(mUsername);
            reply.AppendBoolean(true);

            bool Online = IsOnline;

            reply.AppendBoolean(Online);

            if (Online)
                reply.AppendBoolean(InRoom);
            else
                reply.AppendBoolean(false);

            reply.AppendStringWithBreak(mLook);
            reply.AppendBoolean(false);
            reply.AppendStringWithBreak(mMotto);
            reply.AppendStringWithBreak(mLastOnline);
            reply.AppendStringWithBreak(string.Empty);
            reply.AppendStringWithBreak(string.Empty);
        }
        #endregion
    }
}
