using Pici.HabboHotel.Items;
using Pici.Storage.Database.Session_Details.Interfaces;

namespace Pici.HabboHotel.SoundMachine
{
    class SongItem
    {
        internal uint itemID;
        internal int songID;
        internal Item baseItem;

        public SongItem(uint itemID, int songID, int baseItem)
        {
            this.itemID = itemID;
            this.songID = songID;
            this.baseItem = PiciEnvironment.GetGame().GetItemManager().GetItem((uint)baseItem);
        }

        public SongItem(UserItem item)
        {
            this.itemID = item.Id;
            this.songID = TextHandling.Parse(item.ExtraData);
            this.baseItem = item.GetBaseItem();
        }

        internal UserItem ToUserItem()
        {
            return new UserItem(itemID, baseItem.ItemId, songID.ToString());
        }

        internal void SaveToDatabase(uint roomID)
        {
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (dbClient.dbType == Pici.Storage.Database.DatabaseType.MSSQL)
                {
                    dbClient.runFastQuery("DELETE FROM items_rooms_songs WHERE itemid = " + itemID);
                    dbClient.runFastQuery("INSERT INTO items_rooms_songs VALUES (" + itemID + "," + roomID + "," + songID + ")");
                }
                else
                    dbClient.runFastQuery("REPLACE INTO items_rooms_songs VALUES (" + itemID + "," + roomID + "," + songID + ")");
            }
        }

        internal void RemoveFromDatabase()
        {
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.runFastQuery("DELETE FROM items_rooms_songs WHERE itemid = " + itemID);
            }
        }
    }
}
