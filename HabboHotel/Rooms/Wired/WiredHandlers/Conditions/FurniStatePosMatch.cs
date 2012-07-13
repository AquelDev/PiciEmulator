using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pici.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
using Pici.HabboHotel.Items;
using Pici.Storage.Database.Session_Details.Interfaces;
using System.Data;

namespace Pici.HabboHotel.Rooms.Wired.WiredHandlers.Conditions
{
    class FurniStatePosMatch : IWiredCondition
    {
        private RoomItem item;
        private List<RoomItem> items;
        private bool isDisposed;

        public FurniStatePosMatch(RoomItem item, List<RoomItem> items)
        {
            this.item = item;
            this.items = items;
            this.isDisposed = false;
        }

        public bool AllowsExecution(RoomUser user)
        {
            foreach (RoomItem item in items)
            {
                if (item.ExtraData != item.originalExtraData || item.Coordinate != item.GetPlacementPosition())
                    return false;
            }

            return true;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            lock (items)
            {
                dbClient.runFastQuery("DELETE FROM trigger_in_place WHERE original_trigger = '" + this.item.Id + "'");
                foreach (RoomItem i in items)
                {
                    WiredUtillity.SaveTrigger(dbClient, (int)item.Id, (int)i.Id);
                }
            }
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT triggers_item FROM trigger_in_place WHERE original_trigger = " + this.item.Id);
            DataTable dTable = dbClient.getTable();
            RoomItem targetItem;
            foreach (DataRow dRows in dTable.Rows)
            {
                targetItem = insideRoom.GetRoomItemHandler().GetItem(Convert.ToUInt32(dRows[0]));
                if (targetItem == null || this.items.Contains(targetItem))
                    continue;
                this.items.Add(targetItem);
            }
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_in_place WHERE original_trigger = '" + this.item.Id + "'");
        }

        public void Dispose()
        {
            isDisposed = true;
            item = null;
            if (items != null)
                items.Clear();
            items = null;
        }

        public bool Disposed()
        {
            return isDisposed;
        }
    }
}
