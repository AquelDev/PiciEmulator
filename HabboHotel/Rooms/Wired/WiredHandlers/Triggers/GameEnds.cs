using System;
using Pici.HabboHotel.Items;
using Pici.HabboHotel.Rooms.Games;
using Pici.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;

namespace Pici.HabboHotel.Rooms.Wired.WiredHandlers.Triggers
{
    class GameEnds : IWiredTrigger 
    {
        private RoomItem item;
        private WiredHandler handler;
        private RoomEventDelegate gameEndsDeletgate;

        public GameEnds(RoomItem item, WiredHandler handler, GameManager gameManager)
        {
            this.item = item;
            this.handler = handler;
            this.gameEndsDeletgate = new RoomEventDelegate(gameManager_OnGameEnd);

            gameManager.OnGameEnd += gameEndsDeletgate;
        }

        private void gameManager_OnGameEnd(object sender, EventArgs e)
        {
            handler.RequestStackHandle(item.Coordinate, null, null, Team.none);
            handler.OnEvent(item.Id);
        }

        public void Dispose()
        {
            handler.GetRoom().GetGameManager().OnGameEnd -= gameEndsDeletgate;
            this.item = null;
            this.handler = null;
        }


        public void SaveToDatabase(Pici.Storage.Database.Session_Details.Interfaces.IQueryAdapter dbClient)
        {
        }

        public void LoadFromDatabase(Pici.Storage.Database.Session_Details.Interfaces.IQueryAdapter dbClient, Room insideRoom)
        {
        }

        public void DeleteFromDatabase(Pici.Storage.Database.Session_Details.Interfaces.IQueryAdapter dbClient)
        {
        }
    }
}
