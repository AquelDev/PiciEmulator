using System;
using Pici.HabboHotel.Items;
using Pici.HabboHotel.Rooms.Games;
using Pici.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
using Pici.Storage.Database.Session_Details.Interfaces;
using System.Data;

namespace Pici.HabboHotel.Rooms.Wired.WiredHandlers.Triggers
{
    class ScoreAchieved : IWiredTrigger 
    {
        private RoomItem item;
        private WiredHandler handler;
        private int scoreLevel;
        private bool used;
        private TeamScoreChangedDelegate scoreChangedDelegate;
        private RoomEventDelegate gameEndDeletgate;

        public ScoreAchieved(RoomItem item, WiredHandler handler, int scoreLevel, GameManager gameManager)
        {
            this.item = item;
            this.handler = handler;
            this.scoreLevel = scoreLevel;
            this.used = false;
            this.scoreChangedDelegate = new TeamScoreChangedDelegate(gameManager_OnScoreChanged);
            this.gameEndDeletgate = new RoomEventDelegate(gameManager_OnGameEnd);


            gameManager.OnScoreChanged += scoreChangedDelegate;
            gameManager.OnGameEnd += gameEndDeletgate;
        }

        private void gameManager_OnGameEnd(object sender, EventArgs e)
        {
            this.used = false;
        }

        private void gameManager_OnScoreChanged(object sender, TeamScoreChangedArgs e)
        {
            if (e.Points > scoreLevel && !used)
            {
                used = true;
                handler.RequestStackHandle(item.Coordinate, null, e.user, e.Team);
                handler.OnEvent(item.Id);
            }
        }

        public void Dispose()
        {
            handler.GetRoom().GetGameManager().OnScoreChanged -= scoreChangedDelegate;
            handler.GetRoom().GetGameManager().OnGameEnd -= gameEndDeletgate;
            this.item = null;
            this.handler = null;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int)item.Id, "integer", string.Empty, scoreLevel.ToString(), false);
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT trigger_data FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int)this.item.Id);
            DataRow dRow = dbClient.getRow();
            if (dRow != null)
                this.scoreLevel = Convert.ToInt32(dRow[0].ToString());
            else
                this.scoreLevel = 200;
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = '" + this.item.Id + "'");
        }
    }
}
