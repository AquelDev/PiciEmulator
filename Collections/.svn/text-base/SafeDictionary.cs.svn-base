using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using Butterfly.Collections;

namespace Butterfly.Collections
{
    /// <summary>
    /// Implements a thread-safe dictionary with multiple readers and one writer
    /// </summary>
    /// <typeparam name="T">Key</typeparam>
    /// <typeparam name="V">Value</typeparam>
    /// 
    public class SafeDictionary<T, V> : IDictionary<T, V>, IDisposable
    {
        private readonly Dictionary<T, V> _inner;
        //private Locker _lock = new Locker();
        private readonly object lockObject;

        public SafeDictionary()
        {
            _inner = new Dictionary<T, V>();
            lockObject = new object();
        }

        public SafeDictionary(Dictionary<T, V> inner)
        {
            _inner = inner;
            lockObject = new object();
        }

        internal Dictionary<T, V> getInner
        {
            get
            {
                return _inner;
            }
        }

        internal Dictionary<T, V> getNonThreadSafeCollection()
        {
            return _inner;
        }

        ~SafeDictionary()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
        }

        public int Count
        {
            get
            {
                return _inner.Count;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Clear()
        {
            lock (lockObject)
            {
                _inner.Clear();
            }
        }

        public void Add(T Item, V Value)
        {
            lock (lockObject)
            {
                _inner.Add(Item, Value);
            }
        }

        public void Remove(T Item)
        {
            lock (lockObject)
            {
                _inner.Remove(Item);
            }
        }

        public bool ContainsKey(T Item)
        {
            return _inner.ContainsKey(Item);
        }

        public ICollection<T> Keys
        {
            get
            {
                lock (lockObject)
                {
                    return new List<T>(_inner.Keys);
                }
            }
        }

        public ICollection<V> Values
        {
            get
            {
                lock (lockObject)
                {
                    return new List<V>(_inner.Values);
                }
            }
        }

        public V GetValue(T Item)
        {
            if (_inner.ContainsKey(Item))
            {
                return _inner[Item];
            }

            return default(V);
        }

        public IEnumerator<KeyValuePair<T, V>> GetEnumerator()
        {
        //    return new SafeEnumeratorKvp<T, V>(_inner.GetEnumerator(), _lock);
            throw new NotImplementedException();
        }

        public V this[T Item]
        {
            get
            {
                return _inner[Item];
            }
            set
            {
                _inner[Item] = value;
            }
        }

        bool IDictionary<T, V>.Remove(T key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(T key, out V value)
        {
            return _inner.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<T, V> item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<T, V> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<T, V>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<T, V> item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
