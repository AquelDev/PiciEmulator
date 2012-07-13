using System.Collections.Generic;
using Butterfly.HabboHotel.Rooms;
using System.Collections;
using System.Linq;

namespace Butterfly.HabboHotel.Events
{
    class EventManager
    {
        #region Fields
        private Dictionary<RoomData, int> events;
        private IOrderedEnumerable<KeyValuePair<RoomData, int>> orderedEventRooms;

        private Queue addQueue;
        private Queue removeQueue;
        private Queue updateQueue;

        private Dictionary<int, EventCategory> eventCategories;

        #endregion
        #region Return values
        internal KeyValuePair<RoomData, int>[] GetRoomsForCategory(int categoryID)
        {
            if (categoryID > 0)
                return eventCategories[categoryID].GetActiveRooms();
            else
                return orderedEventRooms.ToArray();
        }
        #endregion

        #region Constructor
        public EventManager()
        {
            this.eventCategories = new Dictionary<int, EventCategory>();
            this.events = new Dictionary<RoomData, int>();
            this.orderedEventRooms = events.OrderByDescending(t => t.Value);
            this.addQueue = new Queue();
            this.removeQueue = new Queue();
            this.updateQueue = new Queue();

            for (int i = 0; i < 30; i++)
            {
                eventCategories.Add(i, new EventCategory(i));
            }
        }
        #endregion

        #region Methods
        #region Threading

        internal void onCycle()
        {
            workRemoveQueue();
            workAddQueue();
            workUpdate();

            SortCollection();

            foreach (EventCategory category in eventCategories.Values)
            {
                category.onCycle();
            }
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

                        if (events.ContainsKey(data))
                            events[data] = data.UsersNow;
                    }
                }
            }
        }
        #endregion

        internal void QueueAddEvent(RoomData data, int roomEventCategory)
        {
            lock (addQueue.SyncRoot)
            {
                addQueue.Enqueue(data);
            }

            eventCategories[roomEventCategory].QueueAddEvent(data);
        }

        internal void QueueRemoveEvent(RoomData data, int roomEventCategory)
        {
            lock (removeQueue.SyncRoot)
            {
                removeQueue.Enqueue(data);
            }
            eventCategories[roomEventCategory].QueueRemoveEvent(data);
        }

        internal void QueueUpdateEvent(RoomData data, int roomEventCategory)
        {
            lock (updateQueue.SyncRoot)
            {
                updateQueue.Enqueue(data);
            }
            eventCategories[roomEventCategory].QueueUpdateEvent(data);
        }
        #endregion
    }
}
