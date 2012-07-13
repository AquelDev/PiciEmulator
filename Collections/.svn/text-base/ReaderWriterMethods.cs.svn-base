using System;
using System.Threading;
using Butterfly.Collections;

namespace Butterfly.Collections
{
    public class AcquireReaderLock : IDisposable
    {
        private ReaderWriterLockSlim m_Lock = null;
        private bool m_Disposed = false;

        public AcquireReaderLock(ReaderWriterLockSlim rwl)
        {
            m_Lock = rwl;
            m_Lock.EnterReadLock();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_Disposed)
            {
                if (disposing)
                {
                    m_Lock.ExitReadLock();
                }
            }
            m_Disposed = true;
        }
    }

    public class AcquireWriterLock : IDisposable
    {
        private ReaderWriterLockSlim m_Lock = null;
        private bool m_Disposed = false;

        public AcquireWriterLock(ReaderWriterLockSlim rwl)
        {
            m_Lock = rwl;
            m_Lock.EnterWriteLock();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_Disposed)
            {
                if (disposing)
                {
                    m_Lock.ExitWriteLock();
                }
            }
            m_Disposed = true;
        }
    }

    public class InterlockedLock : IDisposable
    {
        private const int timeout = 100;

        private bool disposed = false;
        private Locker locker;

        public InterlockedLock(Locker _locker)
        {
            this.locker = _locker;

            int count = 0;
            while (!locker.TryEnterLock())
            {
                count++;
                Thread.Sleep(1);

                if (count > timeout)
                    throw new LockTimeoutException("Unable to lock collection");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;

                if (!locker.ExitLock())
                    throw new LockTimeoutException("EH DARIO");
            }
        }
    }
}