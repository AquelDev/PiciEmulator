using System;
using Butterfly.Messages;

namespace Butterfly.HabboHotel.ChatMessageStorage
{
    class ChatMessage
    {
        private readonly uint userID;
        private readonly string username;
        internal readonly uint roomID;
        private readonly string message;

        private readonly DateTime timeSpoken;

        internal readonly string roomName;
        internal readonly bool roomIsPublic;

        public ChatMessage(uint userID, string username, uint roomID, string roomName, bool roomIsPublic, string message, DateTime timeSpoken)
        {
            this.userID = userID;
            this.username = username;
            this.roomID = roomID;
            this.message = message;
            this.timeSpoken = timeSpoken;
            this.roomName = roomName;
            this.roomIsPublic = roomIsPublic;
        }

        internal void Serialize(ref ServerMessage packet)
        {
            packet.AppendInt32(timeSpoken.Hour);
            packet.AppendInt32(timeSpoken.Minute);
            packet.AppendUInt(userID);
            packet.AppendStringWithBreak(username);
            packet.AppendStringWithBreak(message);
        }
    }
}
