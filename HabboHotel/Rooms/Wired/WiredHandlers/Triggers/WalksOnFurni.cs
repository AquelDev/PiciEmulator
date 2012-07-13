﻿using System.Collections.Generic;
using Pici.HabboHotel.Items;
using Pici.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
using System.Collections;
using Pici.Storage.Database.Session_Details.Interfaces;
using System.Data;
using System;

namespace Pici.HabboHotel.Rooms.Wired.WiredHandlers.Triggers
{
    class WalksOnFurni : IWiredTrigger, IWiredCycleable, IWiredTimer
    {
        private RoomItem item;
        private WiredHandler handler;
        private List<RoomItem> items;
        private UserWalksFurniDelegate delegateFunction;

        private int currentCycle;
        private int requiredCycles;
        private Queue requestQueue;

        private bool disposed;

        public WalksOnFurni(RoomItem item, WiredHandler handler, List<RoomItem> targetItems, int requiredCycles)
        {
            this.item = item;
            this.handler = handler;
            this.items = targetItems;
            this.delegateFunction = new UserWalksFurniDelegate(targetItem_OnUserWalksOnFurni);

            this.currentCycle = 0;
            this.requiredCycles = requiredCycles;
            this.requestQueue = new Queue();
            
            foreach (RoomItem targetItem in targetItems)
            {
                targetItem.OnUserWalksOnFurni += delegateFunction;
            }

            this.disposed = false;
        }

        public bool OnCycle()
        {
            if (currentCycle > requiredCycles)
            {
                if (requestQueue.Count > 0)
                {
                    lock (requestQueue.SyncRoot)
                    {
                        while (requestQueue.Count > 0)
                        {
                            UserWalksFurniValue obj = (UserWalksFurniValue)requestQueue.Dequeue();
                            handler.RequestStackHandle(item.Coordinate, obj.item, obj.user, Games.Team.none);
                            handler.OnEvent(item.Id);
                        }
                    }
                }
                return false;
            }
            else
            {
                currentCycle++;
                return true;
            }
        }

        private void targetItem_OnUserWalksOnFurni(object sender, UserWalksOnArgs e)
        {
            currentCycle = 0;
            if (requiredCycles > 0)
            {
                UserWalksFurniValue obj = new UserWalksFurniValue(e.user, (RoomItem)sender);
                lock (requestQueue.SyncRoot)
                {
                    requestQueue.Enqueue(obj);
                }

                handler.RequestCycle(this);
            }
            else
            {
                handler.RequestStackHandle(item.Coordinate, (RoomItem)sender, e.user, Games.Team.none);
                handler.OnEvent(item.Id);
            }
        }

        public void Dispose()
        {
            disposed = true;
            if (items != null)
            {
                foreach (RoomItem targetItem in items)
                {
                    targetItem.OnUserWalksOnFurni -= delegateFunction;
                }
                items.Clear();
            }
            items = null;
            this.item = null;
            this.handler = null;

        }

        public void ResetTimer()
        {
            currentCycle = requiredCycles;
        }


        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int)item.Id, "integer", string.Empty, requiredCycles.ToString(), false);
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
            DataRow dRow = dbClient.getRow();
            if (dRow != null)
                this.requiredCycles = Convert.ToInt32(dRow[0].ToString());
            else
                this.requiredCycles = 0;

            dbClient.setQuery("SELECT triggers_item FROM trigger_in_place WHERE original_trigger = " + this.item.Id);
            DataTable dTable = dbClient.getTable();
            RoomItem targetItem;
            foreach (DataRow dRows in dTable.Rows)
            {
                targetItem = insideRoom.GetRoomItemHandler().GetItem(Convert.ToUInt32(dRows[0]));
                if (targetItem == null || this.items.Contains(targetItem))
                    continue;
                targetItem.OnUserWalksOnFurni += targetItem_OnUserWalksOnFurni;
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
