using System.Data;
using System;

namespace Butterfly.HabboHotel.Rooms
{
    struct RoomLinkInformation
    {
        internal readonly uint roomID;
        internal readonly uint toRoomID;

        internal readonly int fromX;
        internal readonly int fromY;

        internal readonly int toX;
        internal readonly int toY;

        public RoomLinkInformation(DataRow row)
        {
            this.roomID = Convert.ToUInt32(row["roomid"]);
            this.toRoomID = Convert.ToUInt32(row["toroomid"]);

            this.fromX = (int)row["fromx"];
            this.fromY = (int)row["fromy"];

            this.toX = (int)row["tox"];
            this.toY = (int)row["toy"];
        }
    }
}
