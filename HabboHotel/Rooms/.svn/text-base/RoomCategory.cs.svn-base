using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Butterfly.HabboHotel.Rooms;
using Butterfly.Messages;

namespace Butterfly.HabboHotel.Rooms
{
    class RoomCategory
    {
        #region Fields
        private readonly int categoryID;
        private readonly string caption;

        private Queue addActiveRoomQueue;
        private Queue updateActiveRoomQueue;
        private Queue removeActiveRoomQueue;

        private Dictionary<RoomData, int> activeRooms;
        private IEnumerable<KeyValuePair<RoomData, int>> orderedRooms;

        #endregion

        #region Return values
        internal KeyValuePair<RoomData, int>[] GetOrderedRooms()
        {
            return orderedRooms.ToArray();
        }
        #endregion

        #region Constructor
        public RoomCategory(int categoryID, string caption)
        {
            this.categoryID = categoryID;
            this.caption = caption;

            this.activeRooms = new Dictionary<RoomData, int>();
            this.orderedRooms = activeRooms.OrderByDescending(t => t.Value).Take(0);

            this.addActiveRoomQueue = new Queue();
            this.updateActiveRoomQueue = new Queue();
            this.removeActiveRoomQueue = new Queue();
        }
        #endregion

        #region Threading
        internal void OnCycle()
        {
            bool itemsAdded = WorkActiveRoomsAddQueue();
            bool itemsRemoved = WorkActiveRoomsRemoveQueue();
            bool itemsUpdated = WorkActiveRoomsUpdateQueue();

            if (itemsAdded || itemsRemoved || itemsUpdated)
                SortCollection();
        }

        private void SortCollection()
        {
            orderedRooms = activeRooms.OrderByDescending(t => t.Value).Take(40);
        }

        private bool WorkActiveRoomsAddQueue()
        {
            if (addActiveRoomQueue.Count > 0)
            {
                lock (addActiveRoomQueue.SyncRoot)
                {
                    while (addActiveRoomQueue.Count > 0)
                    {
                        RoomData data = (RoomData)addActiveRoomQueue.Dequeue();
                        if (!activeRooms.ContainsKey(data))
                            activeRooms.Add(data, data.UsersNow);
                    }
                }

                return true;
            }
            return false;
        }

        private bool WorkActiveRoomsUpdateQueue()
        {
            if (updateActiveRoomQueue.Count > 0)
            {
                lock (updateActiveRoomQueue.SyncRoot)
                {
                    while (updateActiveRoomQueue.Count > 0)
                    {
                        RoomData data = (RoomData)updateActiveRoomQueue.Dequeue();
                        if (activeRooms.ContainsKey(data))
                            activeRooms[data] = data.UsersNow;
                        else
                            activeRooms.Add(data, data.UsersNow);
                    }
                }
                return true;
            }
            return false;
        }

        private bool WorkActiveRoomsRemoveQueue()
        {
            if (removeActiveRoomQueue.Count > 0)
            {
                lock (removeActiveRoomQueue.SyncRoot)
                {
                    while (removeActiveRoomQueue.Count > 0)
                    {
                        RoomData data = (RoomData)removeActiveRoomQueue.Dequeue();
                        activeRooms.Remove(data);
                    }
                }
                return true;
            }
            return false;
        }
        #endregion

        #region Methods
        internal void QueueAddActiveRooms(RoomData data)
        {
            lock (addActiveRoomQueue.SyncRoot)
            {
                addActiveRoomQueue.Enqueue(data);
            }
        }

        internal void QueueUpdateActiveRooms(RoomData data)
        {
            lock (updateActiveRoomQueue.SyncRoot)
            {
                updateActiveRoomQueue.Enqueue(data);
            }
        }

        internal void QueueRemoveActiveRooms(RoomData data)
        {
            lock (removeActiveRoomQueue.SyncRoot)
            {
                removeActiveRoomQueue.Enqueue(data);
            }
        }
        #endregion
    }
}
