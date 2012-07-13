//using System;
//using System.Collections;

//namespace Butterfly.Collections
//{
//    partial class Sandbox : Hashtable , IDisposable
//    {
//        #region Fields
//        private bool mDisposed;

//        #endregion

//        public Sandbox()
//            : base()
//        {
//            mDisposed = false;
//        }

//        internal ClonedTable GetThreadSafeTable
//        {
//            get
//            {
//                return new ClonedTable(
//                    base.Clone() as Hashtable);
//            }
//        }

//        //internal void Clear()
//        //{
//        //    Dispose();
//        //}

//        #region IDisposable members
//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        private void Dispose(bool Disposing)
//        {
//            if (!this.mDisposed)
//            {
//                mDisposed = true;
//                if (Disposing)
//                {
//                    base.Clear();
//                }
//            }
//        }
//        #endregion
//    }
//}
