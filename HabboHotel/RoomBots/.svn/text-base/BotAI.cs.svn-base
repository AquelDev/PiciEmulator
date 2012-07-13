using System;

using Butterfly.HabboHotel.GameClients;
using Butterfly.HabboHotel.Rooms;

namespace Butterfly.HabboHotel.RoomBots
{
    abstract class BotAI
    {
        internal Int32 BaseId;
        private Int32 RoomUserId;
        private UInt32 RoomId;
        private RoomUser roomUser;
        private Room room;

        internal BotAI() { }

        internal void Init(Int32 pBaseId, Int32 pRoomUserId, UInt32 pRoomId, RoomUser user, Room room)
        {
            this.BaseId = pBaseId;
            this.RoomUserId = pRoomUserId;
            this.RoomId = pRoomId;
            this.roomUser = user;
            this.room = room;
        }

        internal Room GetRoom()
        {
            //return ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(RoomId);
            return room;
        }

        internal RoomUser GetRoomUser()
        {
            //return GetRoom().GetRoomUserByVirtualId(RoomUserId);
            return roomUser;
        }

        internal RoomBot GetBotData()
        {
            RoomUser User = GetRoomUser();
            if (User == null)
                return null;
            else
                return GetRoomUser().BotData;
        }

        internal abstract void OnSelfEnterRoom();
        internal abstract void OnSelfLeaveRoom(bool Kicked);
        internal abstract void OnUserEnterRoom(RoomUser User);
        internal abstract void OnUserLeaveRoom(GameClient Client);
        internal abstract void OnUserSay(RoomUser User, string Message);
        internal abstract void OnUserShout(RoomUser User, string Message);
        internal abstract void OnTimerTick();
    }
}
