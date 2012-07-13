using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Butterfly.HabboHotel.Rooms;

namespace Butterfly.HabboHotel.Events
{
    class EventCategory
    {
        private readonly int categoryID;

        private Dictionary<RoomData, int> events;
        private IOrderedEnumerable<KeyValuePair<RoomData, int>> orderedEventRooms;

        private Queue addQueue;
        private Queue removeQueue;
        private Queue updateQueue;

        internal KeyValuePair<RoomData, int>[] GetActiveRooms()
        {
            return orderedEventRooms.ToArray();
        }

        internal EventCategory(int categoryID)
        {
            this.categoryID = categoryID;

            this.events = new Dictionary<RoomData, int>();
            this.orderedEventRooms = events.OrderByDescending(t => t.Value);
            this.addQueue = new Queue();
            this.removeQueue = new Queue();
            this.updateQueue = new Queue();
        }

        internal void onCycle()
        {
            workRemoveQueue();
            workAddQueue();
            workUpdate();

            SortCollection();
        }

        private void SortCollection()
        {
            orderedEventRooms = events.Take(40).OrderByDescending(t => t.Value);
        }

        private void workAddQueue()
        {
            if (addQueue.Count > 0)
            {
                lock (addQueue.SyncRoot)
                {
                    while (addQueue.Count > 0)
                    {
                        RoomData data = (RoomData)addQueue.Dequeue();
                        if (!events.ContainsKey(data))
                            events.Add(data, data.UsersNow);
                    }
                }
            }
        }

        private void workRemoveQueue()
        {
            if (removeQueue.Count > 0)
            {
                lock (removeQueue.SyncRoot)
                {
                    while (removeQueue.Count > 0)
                    {
                        RoomData data = (RoomData)removeQueue.Dequeue();
                        events.Remove(data);
                    }
                }
            }
        }

        private void workUpdate()
        {
            if (removeQueue.Count > 0)
            {
                lock (removeQueue.SyncRoot)
                {
                    while (removeQueue.Count > 0)
                    {
                        RoomData data = (RoomData)updateQueue.Dequeue();

                        if (!events.ContainsKey(data))
                            events.Add(data, data.UsersNow);
                        else
                            events[data] = data.UsersNow;
                    }
                }
            }
        }

        internal void QueueAddEvent(RoomData data)
        {
            lock (addQueue.SyncRoot)
            {
                addQueue.Enqueue(data);
            }
        }

        internal void QueueRemoveEvent(RoomData data)
        {
            lock (removeQueue.SyncRoot)
            {
                removeQueue.Enqueue(data);
            }
        }

        internal void QueueUpdateEvent(RoomData data)
        {
            lock (updateQueue.SyncRoot)
            {
                updateQueue.Enqueue(data);
            }
        }
    }
}
