//using System;
//using System.Threading;
//using System.Collections;
//using System.Collections.ObjectModel;
//using System.Collections.Generic;
//using Butterfly.Collections;

//namespace Butterfly.Collections
//{
//    /// <summary>
//    /// Implements a thread-safe list with support for multiple readers and one writer
//    /// </summary>
//    /// <typeparam name="T">Values</typeparam>
//    /// 
//    public class SafeList<T> : IList<T>, IDisposable
//    {
//        private readonly List<T> _inner;

//        private Locker _lock = new Locker();

//        public SafeList()
//        {
//            _inner = new List<T>();
//        }

//        public SafeList(List<T> inner)
//        {
//            _inner = inner;
//        }

//        ~SafeList()
//        {
//            Dispose(false);
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//        }

//        private void Dispose(bool disposing)
//        {
//            if (!disposing)
//            {
//                return;
//            }

//            _lock.ExitLock();
//        }

//        public int Count
//        {
//            get
//            {
//                using (new InterlockedLock(_lock))
//                {
//                    return _inner.Count;
//                }
//            }
//        }

//        public bool IsReadOnly
//        {
//            get { return false; }
//        }

//        public T this[int index]
//        {
//            get
//            {
//                using (new InterlockedLock(_lock))
//                {
//                    return _inner[index];
//                }
//            }
//            set
//            {
//                using (new InterlockedLock(_lock))
//                {
//                    _inner[index] = value;
//                }
//            }
//        }

//        IEnumerator<T> IEnumerable<T>.GetEnumerator()
//        {
//            using (new InterlockedLock(_lock))
//            {
//                return new SafeEnumerator<T>(_inner.GetEnumerator());
//            }
//        }

//        public void Add(T item)
//        {
//            using (new InterlockedLock(_lock))
//            {
//                _inner.Add(item);
//            }
//        }

//        public void Clear()
//        {
//            using (new InterlockedLock(_lock))
//            {
//                _inner.Clear();
//            }
//        }

//        public bool Contains(T item)
//        {
//            using (new InterlockedLock(_lock))
//            {
//                return _inner.Contains(item);
//            }
//        }

//        public void CopyTo(T[] array, int arrayIndex)
//        {
//            using (new InterlockedLock(_lock))
//            {
//                _inner.CopyTo(array, arrayIndex);
//            }
//        }

//        public bool Remove(T item)
//        {
//            using (new InterlockedLock(_lock))
//            {
//                return _inner.Remove(item);
//            }
//        }

//        public IEnumerator GetEnumerator()
//        {
//            using (new InterlockedLock(_lock))
//            {
//                return new SafeEnumerator<T>(_inner.GetEnumerator());
//            }
//        }

//        public int IndexOf(T item)
//        {
//            using (new InterlockedLock(_lock))
//            {
//                return _inner.IndexOf(item);
//            }
//        }

//        public void Insert(int index, T item)
//        {
//            using (new InterlockedLock(_lock))
//            {
//                _inner.Insert(index, item);
//            }
//        }

//        public void RemoveAt(int index)
//        {
//            using (new InterlockedLock(_lock))
//            {
//                _inner.RemoveAt(index);
//            }
//        }

//        public ReadOnlyCollection<T> AsReadOnly()
//        {
//            using (new InterlockedLock(_lock))
//            {
//                return new ReadOnlyCollection<T>(this);
//            }
//        }

//        public void ForEach(Action<T> action)
//        {
//            using (new InterlockedLock(_lock))
//            {
//                foreach (var item in _inner)
//                {
//                    action(item);
//                }
//            }
//        }

//        internal void AddRange(List<T> items)
//        {
//            using (new InterlockedLock(_lock))
//            {
//                _inner.AddRange(items);
//            }
//        }

//        public bool Exists(Predicate<T> match)
//        {
//            using (new InterlockedLock(_lock))
//            {
//                foreach (var item in _inner)
//                {
//                    if (match(item))
//                    {
//                        return true;
//                    }
//                }
//            }

//            return false;
//        }
//    }
//}