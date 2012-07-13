using Pici.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
using Pici.HabboHotel.Rooms.Games;
using Pici.HabboHotel.Items;
using Pici.Storage.Database.Session_Details.Interfaces;
using System.Data;
using System;

namespace Pici.HabboHotel.Rooms.Wired.WiredHandlers.Effects
{
    class GiveScore : IWiredEffect, IWiredTrigger
    {
        private int maxCountPerGame;
        private int currentGameCount;
        private int scoreToGive;
        private GameManager gameManager;
        private RoomEventDelegate delegateFunction;
        private uint itemID;
        
        public GiveScore(int maxCountPerGame, int scoreToGive, GameManager gameManager, uint itemID)
        {
            this.maxCountPerGame = maxCountPerGame;
            this.currentGameCount = 0;
            this.scoreToGive = scoreToGive;
            this.delegateFunction = new RoomEventDelegate(gameManager_OnGameStart);
            this.gameManager = gameManager;
            this.itemID = itemID;

            gameManager.OnGameStart += delegateFunction;
        }

        private void gameManager_OnGameStart(object sender, System.EventArgs e)
        {
            currentGameCount = 0;
        }

        public bool Handle(RoomUser user, Team team, RoomItem item)
        {
            if (team != Team.none && maxCountPerGame > currentGameCount)
            {
                currentGameCount++;
                gameManager.AddPointToTeam(team, scoreToGive, user);
                gameManager.GetRoom().GetWiredHandler().OnEvent(itemID);
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            gameManager.OnGameStart -= delegateFunction;
            gameManager = null;
            delegateFunction = null;
        }

        public bool IsSpecial(out SpecialEffects function)
        {
            function = SpecialEffects.None;
            return false;
        }


        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int)itemID, "integer", scoreToGive.ToString(), maxCountPerGame.ToString(), false);
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT trigger_data, trigger_data_2 FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int)this.itemID);
            DataRow dRow = dbClient.getRow();
            if (dRow != null)
            {
                this.maxCountPerGame = Convert.ToInt32(dRow[0].ToString());
                this.scoreToGive = Convert.ToInt32(dRow[1].ToString());
            }
            else
            {
                maxCountPerGame = 0;
                scoreToGive = 0;
            }
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = ' " + this.itemID + "'");
        }
    }
}
