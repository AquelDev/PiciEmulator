using System;
using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Rooms.Games;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;

namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Triggers
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


        public void SaveToDatabase(Database_Manager.Database.Session_Details.Interfaces.IQueryAdapter dbClient)
        {
        }

        public void LoadFromDatabase(Database_Manager.Database.Session_Details.Interfaces.IQueryAdapter dbClient, Room insideRoom)
        {
        }

        public void DeleteFromDatabase(Database_Manager.Database.Session_Details.Interfaces.IQueryAdapter dbClient)
        {
        }
    }
}
