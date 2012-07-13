using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Butterfly.Collections
{
    public delegate void onCycleDoneDelegate();

    class QueuedDictionary<T, V>
    {
        #region Fields
        private Dictionary<T, V> collection;

        private Queue addQueue;
        private Queue updateQueue;
        private Queue removeQueue;

        private Queue onCycleEventQueue;

        private EventHandler onAdd;
        private EventHandler onUpdate;
        private EventHandler onRemove;
        private EventHandler onCycleDone;


        #endregion

        #region Return values
        internal Dictionary<T, V>.ValueCollection Values
        {
            get
            {
                return collection.Values;
            }
        }

        internal Dictionary<T, V>.KeyCollection Keys
        {
            get
            {
                return collection.Keys;
            }
        }

        internal Dictionary<T, V> Inner
        {
            get
            {
                return collection;
            }
            set
            {
                collection = value;
            }
        }
        #endregion

        #region Constructor
        public QueuedDictionary()
        {
            this.collection = new Dictionary<T, V>();

            this.addQueue = new Queue();
            this.updateQueue = new Queue();
            this.removeQueue = new Queue();
            this.onCycleEventQueue = new Queue();
        }

        public QueuedDictionary(EventHandler onAddItem, EventHandler onUpdate, EventHandler onRemove, EventHandler onCycleDone)
        {
            this.collection = new Dictionary<T, V>();

            this.addQueue = new Queue();
            this.updateQueue = new Queue();
            this.removeQueue = new Queue();

            this.onAdd = onAddItem;
            this.onUpdate = onUpdate;
            this.onRemove = onRemove;
            this.onCycleDone = onCycleDone;

            this.onCycleEventQueue = new Queue();
        }
        #endregion

        #region Threading
        internal void OnCycle()
        {
            WorkRemoveQueue();
            WorkAddQueue();
            WorkUpdateQueue();

            WorkOnEventDoneQueue();

            if (onCycleDone != null)
                onCycleDone(null, new EventArgs());
        }

        private void WorkOnEventDoneQueue()
        {
            if (onCycleEventQueue.Count > 0)
            {
                lock (onCycleEventQueue.SyncRoot)
                {
                    while (onCycleEventQueue.Count > 0)
                    {
                        onCycleDoneDelegate function = (onCycleDoneDelegate)onCycleEventQueue.Dequeue();
                        function();
                    }
                }
            }
        }

        private void WorkAddQueue()
        {
            if (addQueue.Count > 0)
            {
                lock (addQueue.SyncRoot)
                {
                    while (addQueue.Count > 0)
                    {
                        KeyValuePair<T, V> pair = (KeyValuePair<T, V>)addQueue.Dequeue();

                        if (collection.ContainsKey(pair.Key))
                            collection[pair.Key] = pair.Value;
                        else
                            collection.Add(pair.Key, pair.Value);

                        if (onAdd != null)
                            onAdd(pair, null);
                    }
                }
            }
        }

        private void WorkUpdateQueue()
        {
            if (updateQueue.Count > 0)
            {
                lock (updateQueue.SyncRoot)
                {
                    while (updateQueue.Count > 0)
                    {
                        KeyValuePair<T, V> pair = (KeyValuePair<T, V>)addQueue.Dequeue();
                        if (collection.ContainsKey(pair.Key))
                            collection[pair.Key] = pair.Value;
                        else
                            collection.Add(pair.Key, pair.Value);

                        if (onUpdate != null)
                            onUpdate(pair, null);
                    }
                }
            }
        }

        private void WorkRemoveQueue()
        {
            if (removeQueue.Count > 0)
            {
                lock (removeQueue.SyncRoot)
                {
                    List<T> removeAddQueue = new List<T>();

                    while (removeQueue.Count > 0)
                    {
                        T key = (T)removeQueue.Dequeue();
                        if (collection.ContainsKey(key))
                        {
                            V value = collection[key];
                            collection.Remove(key);

                            KeyValuePair<T, V> removedPair = new KeyValuePair<T, V>(key, value);

                            if (onRemove != null)
                                onRemove(removedPair, null);
                        }
                        else
                        {
                            removeAddQueue.Add(key);
                        }
                    }

                    if (removeAddQueue.Count > 0)
                    {
                        foreach (T key in removeAddQueue)
                        {
                            removeQueue.Enqueue(key);
                        }
                    }
                }
            }
        }

        private void WorkEventQueue()
        {
            if (onCycleEventQueue.Count > 0)
            {
                lock (onCycleEventQueue.SyncRoot)
                {
                    while (onCycleEventQueue.Count > 0)
                    {
                        onCycleDoneDelegate function = (onCycleDoneDelegate)onCycleEventQueue.Dequeue();
                        function();
                    }
                }
            }
        }
        #endregion

        #region EventArgs
        private void onAddItem(object sender, EventArgs args)
        {

        }

        private void onUpdateItem(object sender, EventArgs args)
        {

        }

        private void onRemoveItem(object sender, EventArgs args)
        {

        }

        private void onCycleIsDone(object sender, EventArgs args)
        {

        }
        #endregion

        #region Methods
        internal void Add(T key, V value)
        {
            KeyValuePair<T, V> pair = new KeyValuePair<T, V>(key, value);

            lock (addQueue.SyncRoot)
            {
                addQueue.Enqueue(pair);
            }
        }

        internal void Update(T key, V value)
        {
            KeyValuePair<T, V> pair = new KeyValuePair<T, V>(key, value);

            lock (updateQueue.SyncRoot)
            {
                updateQueue.Enqueue(pair);
            }
        }

        internal void Remove(T key)
        {
            lock (removeQueue.SyncRoot)
            {
                removeQueue.Enqueue(key);
            }
        }

        internal V GetValue(T key)
        {
            if (collection.ContainsKey(key))
                return collection[key];

            return default(V);
        }

        internal bool ContainsKey(T key)
        {
            return collection.ContainsKey(key);
        }

        internal void Clear()
        {
            collection.Clear();
        }

        internal void QueueDelegate(onCycleDoneDelegate function)
        {
            lock (onCycleEventQueue.SyncRoot)
            {
                onCycleEventQueue.Enqueue(function);
            }
        }

        internal List<KeyValuePair<T, V>> ToList()
        {
            return collection.ToList();
        }

        internal void Destroy()
        {
            if (collection != null)
                collection.Clear();
            if (addQueue != null)
                addQueue.Clear();
            if (updateQueue != null)
                updateQueue.Clear();
            if (removeQueue != null)
                removeQueue.Clear();
            if (onCycleEventQueue != null)
                onCycleEventQueue.Clear();
            collection = null;
            addQueue = null;
            updateQueue = null;
            removeQueue = null;
            onCycleEventQueue = null;
            onAdd = null;
            onUpdate = null;
            onRemove = null;
            onCycleDone = null;
        }
        #endregion
    }
}
