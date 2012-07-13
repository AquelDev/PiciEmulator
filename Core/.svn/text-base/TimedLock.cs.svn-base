//using System;
//using System.Runtime.Serialization;
//using System.Threading;

//namespace Butterfly
//{
//    public struct TimedLock : IDisposable
//    {
//        //Author: Vista4life
//        #region Fields

//        private readonly object target;

//        #endregion

//        #region Lock

//        private TimedLock(object o)
//        {
//            target = o;
//        }

//        internal static TimedLock Lock(object o)
//        {
//            return Lock(o, TimeSpan.FromSeconds(0));
//        }

//        internal static TimedLock Lock(object o, TimeSpan timeout)
//        {
//            var tl = new TimedLock(o);

//            if (!Monitor.TryEnter(o, timeout))
//            {
//                throw new LockTimeoutException();
//            }
//            return tl;
//        }
//        #endregion

//        #region Release
//        public void Dispose()
//        {
//            Monitor.Exit(target);
//        }
//        #endregion

//        #region Exception
//        [Serializable()]
//        internal class LockTimeoutException : ApplicationException
//        {
//            public LockTimeoutException()
//                : base("Timeout, Waiting for Lock...")
//            { }
//            protected LockTimeoutException(SerializationInfo pInfo, StreamingContext pContext)
//                : base("Timeout, waiting for lock...")
//            { }
//            public LockTimeoutException(string pException)
//                : base(pException)
//            { }
//            public LockTimeoutException(string pException, Exception pExc)
//                : base(pException, pExc)
//            { }
//        }
//        #endregion
//    }
//}