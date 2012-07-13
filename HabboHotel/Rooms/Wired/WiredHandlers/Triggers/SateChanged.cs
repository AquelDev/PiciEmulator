using System.Collections.Generic;
using Pici.HabboHotel.Items;
using Pici.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
using System.Collections;
using Pici.Storage.Database.Session_Details.Interfaces;
using System.Data;
using System;

namespace Pici.HabboHotel.Rooms.Wired.WiredHandlers
{
    public class SateChanged : IWiredTrigger, IWiredTimer, IWiredCycleable
    {
        private WiredHandler handler;
        private List<RoomItem> items;
        private RoomItem item;
        private OnItemTrigger delegateFunction;
        private Queue triggeringQueue;
        private int delay;
        private int cycleCount;
        private bool disposed;

        public SateChanged(WiredHandler handler, RoomItem item, List<RoomItem> items, int delay)
        {
            this.handler = handler;
            this.items = items;
            this.item = item;
            this.delay = delay;
            this.delegateFunction = new OnItemTrigger(itemTriggered);
            this.cycleCount = 0;
            this.triggeringQueue = new Queue();

            foreach (RoomItem _item in items)
            {
                _item.itemTriggerEventHandler += delegateFunction;
            }
            this.disposed = true;
        }

        public bool OnCycle()
        {
            if (cycleCount > delay)
            {
                if (triggeringQueue.Count > 0)
                {
                    lock (triggeringQueue.SyncRoot)
                    {
                        while (triggeringQueue.Count > 0)
                        {
                            ItemTriggeredArgs e = (ItemTriggeredArgs)triggeringQueue.Dequeue();
                            onTrigger(e);
                        }
                    }
                }
                return false;
            }
            else
            {
                cycleCount++;
                return true;
            }
        }

        private void itemTriggered(object sender, ItemTriggeredArgs e)
        {
            if (delay > 0)
            {
                triggeringQueue.Enqueue(e);
                handler.RequestCycle(this);
            }
            else
            {
                onTrigger(e);
            }
        }

        private void onTrigger(ItemTriggeredArgs e)
        {
            handler.RequestStackHandle(item.Coordinate, e.TriggeringItem, e.TriggeringUser, Games.Team.none);
            handler.OnEvent(item.Id);
        }

        public void Dispose()
        {
            disposed = true;
            handler = null;

            if (items != null)
            {
                foreach (RoomItem _item in items)
                {
                    _item.itemTriggerEventHandler -= delegateFunction;
                }

                items.Clear();
            }
            items = null;
        }

        public void ResetTimer()
        {
            cycleCount = delay;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int)item.Id, "integer", string.Empty, delay.ToString(), false);
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
            dbClient.setQuery("SELECT trigger_data FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int)this.item.Id);
            this.delay = dbClient.getInteger();

            dbClient.setQuery("SELECT triggers_item FROM trigger_in_place WHERE original_trigger = " + this.item.Id);
            DataTable dTable = dbClient.getTable();
            RoomItem targetItem;
            foreach (DataRow dRows in dTable.Rows)
            {
                targetItem = insideRoom.GetRoomItemHandler().GetItem(Convert.ToUInt32(dRows[0]));
                if (targetItem == null || this.items.Contains(targetItem))
                    continue;
                targetItem.itemTriggerEventHandler += delegateFunction;
                this.items.Add(targetItem);
            }
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = '" + this.item.Id + "'");
            dbClient.runFastQuery("DELETE FROM trigger_in_place WHERE original_trigger = '" + this.item.Id + "'");
        }

        public bool Disposed()
        {
            return disposed;
        }
    }
}
