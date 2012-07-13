using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Butterfly.Core;

namespace Butterfly.HabboHotel.Navigators
{
    class NavigatorCache
    {
        private Task mWorker;
        private bool mTaskEnded;
        private Hashtable mCacheList;

        internal NavigatorCache()
        {
            mTaskEnded = false;
            mCacheList = new Hashtable();
            mWorker = new Task(CycleRooms);
            mWorker.Start();
        }

        private void CycleRooms()
        {
            while (!mTaskEnded)
            {
                try
                {
                    Hashtable NewTable = new Hashtable();

                    int i = -2;

                    NewTable.Add(i, Navigator.GetDynamicNavigatorPacket(null, i).GetBytes());
                    Hashtable CurrentTable = mCacheList;
                    mCacheList = NewTable;

                    CurrentTable.Clear();
                }
                catch (Exception e)
                {
                    Logging.LogThreadException(e.ToString(), "Navigator cache task");
                }
                Thread.Sleep(100000);
            }
        }

        internal byte[] GetPacket(int Mode)
        {
            try
            {
                return mCacheList[Mode] as byte[];
            }
            catch
            {
                return null;
            }
        }
    }
}
