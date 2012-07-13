using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Butterfly.HabboHotel.Items;
using System.Drawing;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;

namespace Butterfly.HabboHotel.Rooms.Wired
{
    class ConditionHandler
    {
        private Room room;
        private Hashtable roomMatrix; //Coord | List<IWiredCondition>
        private Queue addQueue;
        private Queue removeQueue;

        public ConditionHandler(Room room)
        {
            this.room = room;
            this.roomMatrix = new Hashtable();
            this.addQueue = new Queue();
            this.removeQueue = new Queue();
        }

        internal void OnCycle()
        {
            WorkAddQueue();
            WorkRemoveQueue();
        }

        private void WorkAddQueue()
        {
            lock (addQueue.SyncRoot)
            {
                while (addQueue.Count > 0)
                {
                    RoomItem item = (RoomItem)addQueue.Dequeue();
                    AddOrUpdateRefferance(item.Coordinate, item.wiredCondition);
                }
            }
        }

        private void AddOrUpdateRefferance(Point coordinate, IWiredCondition item)
        {
            if (roomMatrix.ContainsKey(coordinate))
            {
                List<IWiredCondition> items = (List<IWiredCondition>)roomMatrix[coordinate];
                if (!items.Contains(item))
                    items.Add(item);
            }
            else
            {
                List<IWiredCondition> items = new List<IWiredCondition>(1);
                items.Add(item);
                roomMatrix.Add(coordinate, items);
            }
        }

        private void WorkRemoveQueue()
        {
            lock (removeQueue.SyncRoot)
            {
                while (removeQueue.Count > 0)
                {
                    Point coordinate = (Point)removeQueue.Dequeue();
                    roomMatrix.Remove(coordinate);
                }
            }
        }

        internal void AddOrIgnoreRefferance(RoomItem item)
        {
            lock (addQueue.SyncRoot)
            {
                addQueue.Enqueue(item);
            }
        }

        internal void ClearTile(Point coordinate)
        {
            lock (removeQueue.SyncRoot)
            {
                removeQueue.Enqueue(coordinate);
            }
        }

        internal bool AllowsHandling(Point coordinate, RoomUser user)
        {
            if (!roomMatrix.ContainsKey(coordinate))
                return true;

            List<IWiredCondition> conditions = (List<IWiredCondition>)roomMatrix[coordinate];
            foreach (IWiredCondition condition in conditions)
            {
                if (!condition.AllowsExecution(user))
                    return false;
            }

            return true;
        }
    }
}
