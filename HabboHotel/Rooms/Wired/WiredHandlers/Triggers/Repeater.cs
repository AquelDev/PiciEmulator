using Pici.HabboHotel.Items;
using Pici.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
using Pici.Storage.Database.Session_Details.Interfaces;
using System.Data;
using System;

namespace Pici.HabboHotel.Rooms.Wired.WiredHandlers
{
    public class Repeater : IWiredTrigger, IWiredCycleable, IWiredTimer
    {
        private int cyclesRequired;
        private int cycleCount;
        private WiredHandler handler;
        private RoomItem item;
        private bool disposed;

        public Repeater(WiredHandler handler, RoomItem item, int cyclesRequired)
        {
            this.handler = handler;
            this.cyclesRequired = cyclesRequired;
            this.item = item;

            handler.RequestCycle(this);
            this.disposed = false;
        }
        
        public bool OnCycle()
        {
            cycleCount++;

            if (cycleCount > cyclesRequired)
            {
                handler.RequestStackHandle(item.Coordinate, null, null, Games.Team.none);
                handler.OnEvent(item.Id);
                cycleCount = 0;
            }
            return true;
        }

        public void Dispose()
        {
            disposed = true;
            handler = null;
            item = null;
        }

        public void ResetTimer()
        {
            cycleCount = 0;
        }


        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int)item.Id, "integer", string.Empty, cyclesRequired.ToString(), false);
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT trigger_data FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int)this.item.Id);
            this.cyclesRequired = dbClient.getInteger();
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
