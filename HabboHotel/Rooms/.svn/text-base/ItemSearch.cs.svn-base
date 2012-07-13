//using System.Collections.Generic;
//using Butterfly.HabboHotel.Items;
//using Butterfly.HabboHotel.Pathfinding;
//using Butterfly.HabboHotel.Rooms;
//using Butterfly.Collections;

//namespace Butterfly.HabboHotel.Rooms
//{
//    class ItemSearch
//    {
//        #region Fields
//        private List<AffectedTile> searchedCoords;
//        private List<RoomItem> result;
//        //private Hashtable items;
//        private SafeDictionary<Coord, List<RoomItem>> items;
//        #endregion

//        #region Constructor
//        public ItemSearch(SafeDictionary<Coord, List<RoomItem>> items)
//        {
//            this.searchedCoords = new List<AffectedTile>();
//            this.result = new List<RoomItem>();
//            this.items = items;
//        }
//        #endregion

//        #region Methods
//        //internal void runSearch(RoomItem item)
//        //{
//        //    searchedCoords.Add(new AffectedTile(item.GetX, item.GetY, 0));
//        //    List<RoomItem> itemsForSquare = GetRoomItemForSquare(item.GetX, item.GetY);

//        //    foreach (RoomItem subitem in itemsForSquare)
//        //    {
//        //        result.Add(subitem);
//        //        result.AddRange(getItems(subitem));
//        //    }
//        //}

//        //private List<RoomItem> getItems(RoomItem item)
//        //{
//        //    List<RoomItem> items = new List<RoomItem>();
//        //    //searchedCoords.Add(new AffectedTile(item.GetX, item.GetY, 0));
//        //    foreach (AffectedTile tile in item.GetAffectedTiles.Values)
//        //    {
//        //        if (!searchedCoords.Contains(tile))
//        //        {
//        //            searchedCoords.Add(tile);
//        //            List<RoomItem> Item = GetRoomItemForSquare(tile.X, tile.Y);
//        //            foreach (RoomItem subitem in Item)
//        //            {
//        //                if (!result.Contains(subitem))
//        //                    result.Add(subitem);
//        //                items.AddRange(getItems(subitem));
//        //            }
//        //        }
//        //    }

//        //    return items;
//        //}


//        private bool IsScanned(AffectedTile tile)
//        {
//            foreach (AffectedTile tiles in searchedCoords)
//            {
//                if (tile.X == tiles.X && tile.Y == tiles.Y)
//                    return true;
//            }
//            return false;
//        }

//        private List<RoomItem> GetRoomItemForSquare(int pX, int pY)
//        {
//            Coord coord = new Coord(pX, pY);
//            List<RoomItem> itemsFromSquare = new List<RoomItem>();

//            if (items.TryGetValue(coord, out itemsFromSquare))
//                return itemsFromSquare;

//            return new List<RoomItem>();
//        }
//        #endregion

//        #region Return values
//        internal List<RoomItem> itemSearchResult
//        {
//            get
//            {
//                return result;
//            }
//        }

//        internal List<AffectedTile> coordSearchResult
//        {
//            get
//            {
//                return searchedCoords;
//            }
//        }

//        #endregion
//    }
//}
