using System;

using Butterfly.Messages;

namespace Butterfly.HabboHotel.Users.Messenger
{
    class MessengerRequest
    {
        //private UInt32 xRequestId;

        private UInt32 ToUser;
        private UInt32 FromUser;
        private string mUsername;

        //internal UInt32 RequestId
        //{
        //    get
        //    {
        //        return FromUser;
        //    }
        //}

        internal UInt32 To
        {
            get
            {
                return ToUser;
            }
        }

        internal UInt32 From
        {
            get
            {
                return FromUser;
            }
        }

        //internal string SenderUsername
        //{
        //    get
        //    {
        //        return mUsername;
        //    }
        //}

        internal MessengerRequest(UInt32 ToUser, UInt32 FromUser, string pUsername)
        {
            //this.xRequestId = RequestId;
            this.ToUser = ToUser;  //Me
            this.FromUser = FromUser; //N00b
            this.mUsername = pUsername; //N00bs name
        }

        internal void Serialize(ServerMessage Request)
        {
            // BDhqu@UMeth0d1322033860

            Request.AppendUInt(FromUser);
            Request.AppendStringWithBreak(mUsername);
            Request.AppendStringWithBreak(FromUser.ToString());
        }
    }
}
