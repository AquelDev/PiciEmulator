using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

namespace Butterfly.Collections
{
    /// <summary>
    /// Implements thread-safe access for Enumerating through a KeyValuePair collection of items
    /// </summary>
    /// <typeparam name="TKey">Key</typeparam>
    /// <typeparam name="TValue">Value</typeparam>
    /// 
    public class SafeEnumeratorKvp<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>>, IDisposable
    {
        private readonly IEnumerator<KeyValuePair<TKey, TValue>> m_Inner;
        private readonly ReaderWriterLockSlim m_Lock;

        public SafeEnumeratorKvp(IEnumerator<KeyValuePair<TKey, TValue>> inner, ReaderWriterLockSlim @lock)
        {
            m_Inner = inner;
            m_Lock = @lock;
            
            m_Lock.EnterReadLock();
        }

        /// <summary>
        /// Implementation of IDisposable
        /// </summary>
        public void Dispose()
        {
            m_Lock.ExitReadLock();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Implementation of the Enumeration
        /// </summary>
        /// <returns></returns>

        public bool MoveNext()
        {
            return m_Inner.MoveNext();
        }

        public void Reset()
        {
            m_Inner.Reset();
        }

        public KeyValuePair<TKey, TValue> Current
        {
            get { return m_Inner.Current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}
