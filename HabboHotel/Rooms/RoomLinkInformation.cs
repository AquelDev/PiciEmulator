using System.Data;
using System;

namespace Pici.HabboHotel.Rooms
{
    struct RoomLinkInformation
    {
        internal readonly uint roomID;
        internal readonly uint toRoomID;

        internal readonly int fromX;
        internal readonly int fromY;

        internal readonly int toX;
        internal readonly int toY;

        public RoomLinkInformation(DataRow Row)
        {
            this.roomID = Convert.ToUInt32(Row["roomid"]);
            this.toRoomID = Convert.ToUInt32(Row["toroomid"]);

            this.fromX = (int)Row["fromx"];
            this.fromY = (int)Row["fromy"];

            this.toX = (int)Row["tox"];
            this.toY = (int)Row["toy"];
        }
    }
}
