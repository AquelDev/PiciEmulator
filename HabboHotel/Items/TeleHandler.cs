using System;
using System.Data;
using Pici.HabboHotel.Rooms;
using Pici.Storage.Database.Session_Details.Interfaces;

namespace Pici.HabboHotel.Items
{
    static class TeleHandler
    {
        internal static UInt32 GetLinkedTele(UInt32 TeleId, Room pRoom)
        {
            //foreach (RoomItem Item in pRoom.FloorItems.Values)
            //{
            //    if (Item.Id == TeleId)
            //        return Item.Id;
            //}

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT tele_two_id FROM items_tele_links WHERE tele_one_id = " + TeleId);
                DataRow Row = dbClient.getRow();

                if (Row == null)
                {
                    return 0;
                }

                return Convert.ToUInt32(Row[0]);
            }
        }

        internal static UInt32 GetTeleRoomId(UInt32 TeleId, Room pRoom)
        {
            if (pRoom.GetRoomItemHandler().GetItem(TeleId) != null)
                return pRoom.RoomId;

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT room_id FROM items_rooms WHERE item_id = " + TeleId);
                DataRow Row = dbClient.getRow();

                if (Row == null)
                {
                    return 0;
                }

                return Convert.ToUInt32(Row[0]);
            }
        }

        internal static bool IsTeleLinked(UInt32 TeleId, Room pRoom)
        {
            uint LinkId = GetLinkedTele(TeleId, pRoom);

            if (LinkId == 0)
            {
                return false;
            }


            RoomItem item = pRoom.GetRoomItemHandler().GetItem(LinkId);
            if (item != null && item.GetBaseItem().InteractionType == Pici.HabboHotel.Items.InteractionType.teleport)
                return true;

            uint RoomId = GetTeleRoomId(LinkId, pRoom);

            if (RoomId == 0)
            {
                return false;
            }

            return true;
        }
    }
}
