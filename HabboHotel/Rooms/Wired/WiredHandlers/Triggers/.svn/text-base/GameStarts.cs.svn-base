using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Rooms.Games;

namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Triggers
{
    class GameStarts : IWiredTrigger 
    {
        private RoomItem item;
        private WiredHandler handler;
        private RoomEventDelegate gameStartsDeletgate;

        public GameStarts(RoomItem item, WiredHandler handler, GameManager gameManager)
        {
            this.item = item;
            this.handler = handler;
            this.gameStartsDeletgate = new RoomEventDelegate(gameManager_OnGameStart);

            gameManager.OnGameStart += gameStartsDeletgate;
        }

        private void gameManager_OnGameStart(object sender, EventArgs e)
        {
            handler.RequestStackHandle(item.Coordinate, null, null, Team.none);
            handler.OnEvent(item.Id);
        }

        public void Dispose()
        {
            handler.GetRoom().GetGameManager().OnGameStart -= gameStartsDeletgate;
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
