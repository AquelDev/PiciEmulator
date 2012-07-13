using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Butterfly.Collections;
using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Rooms;
using System.Drawing;

namespace Butterfly.HabboHotel.Rooms
{
    class GameItemHandler
    {
        private QueuedDictionary<uint, RoomItem> banzaiTeleports;
        private QueuedDictionary<uint, RoomItem> banzaiPyramids;
        private Room room;
        private Random rnd;

        public GameItemHandler(Room room)
        {
            this.room = room;
            this.rnd = new Random();
            this.banzaiPyramids = new QueuedDictionary<uint, RoomItem>();
            this.banzaiTeleports = new QueuedDictionary<uint, RoomItem>();
        }

        internal void OnCycle()
        {
            CyclePyramids();
            CycleRandomTeleports();
        }

        private void CyclePyramids()
        {
            banzaiPyramids.OnCycle();

            Random rnd = new Random();

            foreach (RoomItem item in banzaiPyramids.Inner.Values)
            {
                if (item.interactionCountHelper == 0 && item.ExtraData == "1")
                {
                    room.GetGameMap().RemoveFromMap(item, false);
                    item.interactionCountHelper = 1;
                }

                if (string.IsNullOrEmpty(item.ExtraData))
                    item.ExtraData = "0";

                int randomNumber = rnd.Next(0, 30);
                if (randomNumber == 15)
                {
                    if (item.ExtraData == "0")
                    {
                        item.ExtraData = "1";
                        item.UpdateState();
                        room.GetGameMap().RemoveFromMap(item, false);
                    }
                    else
                    {
                        if (room.GetGameMap().itemCanBePlacedHere(item.GetX, item.GetY))
                        {
                            item.ExtraData = "0";
                            item.UpdateState();
                            room.GetGameMap().AddItemToMap(item, false);
                        }
                    }
                }
            }
        }

        private void CycleRandomTeleports()
        {
            banzaiTeleports.OnCycle();
        }

        internal void AddPyramid(RoomItem item, uint itemID)
        {
            if (banzaiPyramids.ContainsKey(itemID))
                banzaiPyramids.Inner[itemID] = item;
            else
                banzaiPyramids.Add(itemID, item);
        }

        internal void RemovePyramid(uint itemID)
        {
            banzaiPyramids.Remove(itemID);
        }

        internal void AddTeleport(RoomItem item, uint itemID)
        {
            if (banzaiTeleports.ContainsKey(itemID))
                banzaiTeleports.Inner[itemID] = item;
            else
                banzaiTeleports.Add(itemID, item);
        }

        internal void RemoveTeleport(uint itemID)
        {
            banzaiTeleports.Remove(itemID);
        }

        internal void onTeleportRoomUserEnter(RoomUser User, RoomItem Item)
        {

            IEnumerable<RoomItem> items = banzaiTeleports.Inner.Values.Where(p => p.Id != Item.Id);

            int count = items.Count();

            int countID = rnd.Next(0, count);
            int countAmount = 0;

            if (count == 0)
                return;

            foreach (RoomItem item in items)
            {
                if (item == null)
                    continue;
                if (countAmount == countID)
                {
                    //room.GetGameMap().GameMap[User.X, User.Y] = User.SqState;
                    //room.GetGameMap().UpdateUserMovement(new Point(User.Coordinate.X, User.Coordinate.Y), new Point(item.Coordinate.X, item.Coordinate.Y), User);
                    //User.X = item.GetX;
                    //User.Y = item.GetY;
                    //User.Z = item.GetZ;

                    //User.SqState = room.GetGameMap().GameMap[User.X, User.X];
                    //room.GetGameMap().GameMap[User.X, User.X] = 1;
                    //User.RotBody = item.Rot;
                    //User.RotHead = item.Rot;

                    room.GetGameMap().TeleportToItem(User, item);

                    item.ExtraData = "1";
                    item.UpdateNeeded = true;
                    Item.ExtraData = "1";
                    Item.UpdateNeeded = true;
                    item.UpdateState();
                    Item.UpdateState();

                }

                countAmount++;
            }

        }

        internal void Destroy()
        {
            if (banzaiTeleports != null)
                banzaiTeleports.Destroy();
            if (banzaiPyramids != null)
                banzaiPyramids.Clear();
            banzaiPyramids = null;
            banzaiTeleports = null;
            room = null;
            rnd = null;
        }
    }
}
