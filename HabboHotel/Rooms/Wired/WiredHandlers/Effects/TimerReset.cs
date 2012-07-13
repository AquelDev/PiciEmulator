using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pici.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
using Pici.HabboHotel.Items;
using Pici.HabboHotel.Rooms.Games;
using System.Data;
using Pici.Storage.Database.Session_Details.Interfaces;

namespace Pici.HabboHotel.Rooms.Wired.WiredHandlers.Effects
{
    class TimerReset : IWiredEffect, IWiredTrigger, IWiredCycleable
    {
        private Room room;
        private uint itemID;
        private WiredHandler handler;
        private List<RoomItem> items;

        private int delay;
        private int cycles;

        private bool disposed;

        public TimerReset(Room room, WiredHandler handler, List<RoomItem> items, int delay, uint itemID)
        {
            this.room = room;
            this.handler = handler;
            this.items = items;
            this.delay = delay;
            this.cycles = 0;
            this.disposed = false;
        }

        public bool Handle(RoomUser user, Team team, RoomItem item)
        {
            if (delay > 0)
            {
                cycles = 0;
                handler.RequestCycle(this);
            }
            else
            {
                return ResetTimers();
            }

            return false;
        }

        public bool OnCycle()
        {
            if (cycles > delay)
            {
                ResetTimers();
                return false;
            }
            else
            {
                cycles++;
            }
            return true;
        }

        public void Dispose()
        {
            disposed = true;
            room = null;
            handler = null;
            if (items != null)  
                items.Clear();
            items = null;
        }

        private bool ResetTimers()
        {
            handler.OnEvent(itemID);
            bool itemReset = false;
            foreach (RoomItem item in items)
            {
                if (item.wiredHandler != null)
                {
                    IWiredTimer timer = item.wiredHandler as IWiredTimer;
                    if (timer == null)
                        continue;

                    timer.ResetTimer();
                    itemReset = true;
                }
            }

            return itemReset;
        }

        public bool IsSpecial(out SpecialEffects function)
        {
            function = SpecialEffects.None;
            return false;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int)itemID, "integer", string.Empty, delay.ToString(), false);
            lock (items)
            {
                dbClient.runFastQuery("DELETE FROM trigger_in_place WHERE original_trigger = '" + this.itemID + "'");
                foreach (RoomItem i in items)
                {
                    WiredUtillity.SaveTrigger(dbClient, (int)itemID, (int)i.Id);
                }
            }
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT trigger_data FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int)this.itemID);
            DataRow dRow = dbClient.getRow();
            if (dRow != null)
            {
                this.delay = Convert.ToInt32(dRow[0].ToString());
            }
            else
            {
                this.delay = 20;
            }

            dbClient.setQuery("SELECT triggers_item FROM trigger_in_place WHERE original_trigger = " + this.itemID);
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
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = '" + this.itemID + "'");
            dbClient.runFastQuery("DELETE FROM trigger_in_place WHERE original_trigger = '" + this.itemID + "'");
        }

        public bool Disposed()
        {
            return disposed;
        }
    }
}
