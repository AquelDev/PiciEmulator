//using System;
//using System.Net.Sockets;
//using Butterfly.Core;
//using System.Net;
//using System.Collections.Generic;
//using Butterfly.Collections;

//namespace Butterfly.Net
//{
//    static class TcpAuthorization
//    {
//        private static SafeDictionary<string, int> mConnectionStorage;
//        private static SafeList<string> mBannedIPs;
//        private static string mLastIpBlocked;
//        internal static bool Enabled;

//        internal static void SetupTcpAuthorization(int ConnectionCount)
//        {
//            //mConnectionStorage = new string[ConnectionCount];
//            Enabled = false;
//            mConnectionStorage = new SafeDictionary<string, int>();
//            mBannedIPs = new SafeList<string>();
//        }

//        internal static bool CheckConnection(Socket Sock)
//        {
//            if (!Enabled)
//                return true;
//            string SocketIP = Sock.RemoteEndPoint.ToString().Split(':')[0];

//            if (SocketIP == mLastIpBlocked)
//                return false;
//            else
//            {
//                int ConnectionAmount;
//                lock (mConnectionStorage)
//                {
//                    ConnectionAmount = GetConnectionAmount(SocketIP);
//                }
//                if (ConnectionAmount > 10)
//                {
//                    mLastIpBlocked = SocketIP;
//                    Logging.LogDDOS(SocketIP + "," + DateTime.Now.ToString());
//                    return false;
//                }
//                else
//                {
//                    lock (mConnectionStorage)
//                    {
//                        if (mConnectionStorage.ContainsKey(SocketIP))
//                            mConnectionStorage.Remove(SocketIP);

//                        mConnectionStorage.Add(SocketIP, ConnectionAmount + 1);
//                    }
//                }
//            }
//            if (mBannedIPs.Contains(SocketIP))
//                return false;

//            return true;
//            //if (SocketIP == mLastIpBlocked)
//            //{
//            //    //SocketIP = null;
//            //    return false;
//            //}
//            //else if (GetConnectionAmount(SocketIP) > 10) 
//            //{
//            //    //Console.WriteLine(SocketIP + " is banned.");
//            //    Logging.LogDDOS(SocketIP + DateTime.Now.ToString());
//            //    mLastIpBlocked = SocketIP;
//            //    //SocketIP = null;
//            //    return false;
//            //}
//            //int ConnectionID = GetFreeConnectionID();
//            //if (ConnectionID < 0)
//            //    return false;
//            //else
//            //    mConnectionStorage[ConnectionID] = SocketIP;

//            //SocketIP = null;
//        }

//        internal static void BannIP(string IP)
//        {
//            if (!mBannedIPs.Contains(IP))
//                mBannedIPs.Add(IP);
//        }

//        private static int GetConnectionAmount(string IP)
//        {
//            if (!mConnectionStorage.ContainsKey(IP))
//                return 0;
//            else
//                return mConnectionStorage[IP];
//            //int Count = 0;
//            //for (int i = 0; i < mMaxLoad; i++)
//            //{
//            //    if (mConnectionStorage[i] == IP)
//            //        Count++;
//            //}

//            //return Count;
//        }

//        internal static void FreeConnection(string IP)
//        {
//            int ConnectionAmount = GetConnectionAmount(IP) - 1;
//            mConnectionStorage.Remove(IP);
//            if (ConnectionAmount > 0)
//            {
//                mConnectionStorage.Remove(IP);
//                mConnectionStorage.Add(IP, ConnectionAmount);
//            }

//            //for (int i = 0; i < mConnectionStorage.Length; i++)
//            //{
//            //    if (mConnectionStorage[i] == IP)
//            //    {
//            //        mConnectionStorage[i] = null;
//            //        break;
//            //    }
//            //}
//        }
//        //private static int GetFreeConnectionID()
//        //{
//        //    for (int i = 0; i < mConnectionStorage.Length; i++)
//        //    {
//        //        if (mConnectionStorage[i] == null)
//        //        {
//        //            if (i > mMaxLoad)
//        //                mMaxLoad = i;
//        //            return i;
//        //        }
//        //    }
//        //    return -1;
//        //}

//        internal static void Flush()
//        {
//            mBannedIPs.Clear();
//            mLastIpBlocked = string.Empty;
//        }
//    }
//}
