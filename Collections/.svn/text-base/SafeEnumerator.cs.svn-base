using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using Butterfly.Collections;

namespace Butterfly.Collections
{
    /// <summary>
    /// Implements thread-safe access for Enumerating through a collection of items
    /// </summary>
    /// <typeparam name="T">Items</typeparam>
    /// 
    public class SafeEnumerator<T> : IEnumerator<T>, IDisposable
    {
        private readonly IEnumerator<T> m_Inner;

        public SafeEnumerator(IEnumerator<T> inner)
        {
            m_Inner = inner;
        }

        /// <summary>
        /// Implementation of IDisposable
        /// </summary>
        public void Dispose()
        {
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

        public T Current
        {
            get { return m_Inner.Current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}
