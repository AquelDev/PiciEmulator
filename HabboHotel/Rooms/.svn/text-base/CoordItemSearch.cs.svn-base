using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Pathfinding;
using Butterfly.Collections;
using System.Drawing;

namespace Butterfly.HabboHotel.Rooms
{
    class CoordItemSearch
    {
        private Hashtable items;
                            
        public CoordItemSearch(Hashtable itemArray)
        {
            this.items = itemArray;
        }

        internal List<RoomItem> GetRoomItemForSquare(int pX, int pY, double minZ)
        {
            List<RoomItem> itemsToReturn = new List<RoomItem>();

            Point coord = new Point(pX, pY);
            if (items.ContainsKey(coord))
            {
                List<RoomItem> itemsFromSquare = (List<RoomItem>)items[coord];

                foreach (RoomItem item in itemsFromSquare)
                    if (item.GetZ > minZ)
                        if (item.GetX == pX && item.GetY == pY)
                            itemsToReturn.Add(item);
            }

            return itemsToReturn;
        }

        internal List<RoomItem> GetRoomItemForSquare(int pX, int pY)
        {
            Point coord = new Point(pX, pY);
            //List<RoomItem> itemsFromSquare = new List<RoomItem>();
            List<RoomItem> itemsToReturn = new List<RoomItem>();

            if (items.ContainsKey(coord))
            {
                List<RoomItem> itemsFromSquare = (List<RoomItem>)items[coord];
                
                foreach (RoomItem item in itemsFromSquare)
                {
                    if (item.Coordinate.X == coord.X && item.Coordinate.Y == coord.Y)
                        itemsToReturn.Add(item);
                }
            }

            return itemsToReturn;
        }

        internal List<RoomItem> GetAllRoomItemForSquare(int pX, int pY)
        {
            Point coord = new Point(pX, pY);
            List<RoomItem> itemsToReturn = new List<RoomItem>();

            if (items.ContainsKey(coord))
            {
                List<RoomItem> itemsFromSquare = (List<RoomItem>)items[coord];

                foreach (RoomItem item in itemsFromSquare)
                {
                    if (!itemsToReturn.Contains(item))
                        itemsToReturn.Add(item);
                }
            }

            return itemsToReturn;
        }

        //internal void Dispose()
        //{
        //    items.Clear();
        //}


        //private Hashtable items;

        //public CoordItemSearch(Hashtable collection)
        //{
        //    this.items = collection.Clone() as Hashtable;
        //}

        //internal List<RoomItem> GetRoomItemForSquare(int pX, int pY, double minZ)
        //{
        //    List<RoomItem> Items = new List<RoomItem>();

        //    foreach (RoomItem Item in items.Values)
        //    {
        //        if (Item.GetX == pX && Item.GetY == pY && Item.GetZ > minZ)
        //            Items.Add(Item);
        //    }

        //    return Items;
        //}

        //internal Dictionary<RoomItem, List<RoomItem>> scanForItem(List<RoomItem> Rollers)
        //{
        //    Dictionary<RoomItem, List<RoomItem>> results = new Dictionary<RoomItem, List<RoomItem>>();

        //    foreach (RoomItem item in Rollers)
        //    {
        //        results.Add(item, new List<RoomItem>());
        //    }

        //    foreach (RoomItem Item in Rollers)
        //    {
        //        foreach (RoomItem subitem in Rollers)
        //        {
        //            if (Item.GetX == subitem.GetY && Item.GetY == subitem.GetY && subitem.GetZ > Item.GetZ)
        //            {
        //                results[subitem].Add(Item);
        //            }
        //        }
        //    }

        //    return results;
        //}
    }
}
