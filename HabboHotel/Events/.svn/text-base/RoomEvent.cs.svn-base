using System;
using System.Collections.Generic;
using Butterfly.HabboHotel.GameClients;
using Butterfly.Messages;
using System.Collections;

namespace Butterfly.HabboHotel.Rooms
{
    class RoomEvent
    {
        internal string Name;
        internal string Description;
        internal int Category;
        internal ArrayList Tags;
        internal string StartTime;

        internal UInt32 RoomId;

        internal RoomEvent(UInt32 RoomId, string Name, string Description, int Category, List<string> tags)
        {
            this.RoomId = RoomId;
            this.Name = Name;
            this.Description = Description;
            this.Category = Category;

            this.StartTime = DateTime.Now.ToShortTimeString();

            this.Tags = new ArrayList();

            if (tags != null)
            {
                foreach (string tag in tags)
                {
                    this.Tags.Add(tag);
                }
            }
        }

        internal ServerMessage Serialize(GameClient Session)
        {
            ServerMessage Message = new ServerMessage(370);
            Message.AppendStringWithBreak(Session.GetHabbo().Id + "");
            Message.AppendStringWithBreak(Session.GetHabbo().Username);
            Message.AppendStringWithBreak(RoomId + "");
            Message.AppendInt32(Category);
            Message.AppendStringWithBreak(Name);
            Message.AppendStringWithBreak(Description);
            Message.AppendStringWithBreak(StartTime);
            Message.AppendInt32(Tags.Count);

            foreach (string Tag in Tags.ToArray())
            {
                Message.AppendStringWithBreak(Tag);
            }
            return Message;
        }
    }
}
