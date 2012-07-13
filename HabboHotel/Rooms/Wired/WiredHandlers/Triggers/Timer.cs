using System;
using Pici.HabboHotel.Items;
using Pici.HabboHotel.Rooms.Games;
using Pici.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
using Pici.Storage.Database.Session_Details.Interfaces;
using System.Data;

namespace Pici.HabboHotel.Rooms.Wired.WiredHandlers.Triggers
{
    class Timer : IWiredTrigger, IWiredCycleable, IWiredTimer
    {
        private RoomItem item;
        private WiredHandler handler;
        private int requiredCycles;
        private int currentCycle;
        private RoomEventDelegate delegateFunction;
        private bool disposed;

        public Timer(RoomItem item, WiredHandler handler, int cycleCount, GameManager gameManager)
        {
            this.item = item;
            this.handler = handler;
            this.requiredCycles = cycleCount;
            this.currentCycle = 0;
            this.delegateFunction = new RoomEventDelegate(gameManager_OnGameEnd);

            gameManager.OnGameEnd += delegateFunction;
            this.disposed = false;
        }

        private void gameManager_OnGameEnd(object sender, EventArgs e)
        {
            continueTimer();   
        }

        private void continueTimer()
        {
            handler.RequestCycle(this);
        }

        private void resetTimer()
        {
            currentCycle = 0;
        }

        public bool OnCycle()
        {
            if (requiredCycles > currentCycle)
            {
                handler.RequestStackHandle(item.Coordinate, null, null, Team.none);
                handler.OnEvent(item.Id);
                resetTimer();
                return false;
            }
            else
            {
                currentCycle++;
                return true;
            }
        }

        public void Dispose()
        {
            disposed = true;
            this.handler.GetRoom().GetGameManager().OnGameEnd -= delegateFunction;
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
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT trigger_data FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int)this.item.Id);
            DataRow dRow = dbClient.getRow();
            if (dRow != null)
                this.requiredCycles = Convert.ToInt32(dRow[0].ToString());
            else
                this.requiredCycles = 20;
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = '" + this.item.Id + "'");
        }

        public bool Disposed()
        {
            return disposed;
        }
    }
}
