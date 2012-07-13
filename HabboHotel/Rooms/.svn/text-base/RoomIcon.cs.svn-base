using System.Collections.Generic;

using Butterfly.Messages;

namespace Butterfly.HabboHotel.Rooms
{
    class RoomIcon
    {
        internal int BackgroundImage;
        internal int ForegroundImage;
        internal Dictionary<int, int> Items;

        internal RoomIcon(int BackgroundImage, int ForegroundImage, Dictionary<int, int> Items)
        {
            this.BackgroundImage = BackgroundImage;
            this.ForegroundImage = ForegroundImage;
            this.Items = Items;
        }

        internal void Serialize(ServerMessage Message)
        {
            Message.AppendInt32(BackgroundImage);
            Message.AppendInt32(ForegroundImage);
            Message.AppendInt32(Items.Count);

            foreach (KeyValuePair<int, int> Item in Items)
            {
                Message.AppendInt32(Item.Key);
                Message.AppendInt32(Item.Value);
            }
        }

        internal void Destroy()
        {
            if (Items != null)
                Items.Clear();
            Items = null;
        }
    }
}
