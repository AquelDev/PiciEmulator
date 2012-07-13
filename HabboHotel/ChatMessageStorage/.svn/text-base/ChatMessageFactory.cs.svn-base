using System;
using Butterfly.HabboHotel.GameClients;
using Butterfly.HabboHotel.Rooms;

namespace Butterfly.HabboHotel.ChatMessageStorage
{
    class ChatMessageFactory
    {
        internal static ChatMessage CreateMessage(string message, GameClient user, Room room)
        {
            uint userID = user.GetHabbo().Id;
            string username = user.GetHabbo().Username;
            uint roomID = room.RoomId;
            string roomName = room.Name;
            bool isPublic = room.IsPublic;
            DateTime timeSpoken = DateTime.Now;

            ChatMessage chatMessage = new ChatMessage(userID, username, roomID, roomName, isPublic, message, timeSpoken);
            return chatMessage;
        }
    }
}
