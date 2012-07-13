using System;
using System.Net.Sockets;
using Butterfly;
using Butterfly.Core;
using Database_Manager.Database.Session_Details.Interfaces;
using Database_Manager.Database;

namespace Butterfly.ServerManager
{
    class Session
    {
        private Socket mSock;
        private AsyncCallback mReceivedCallback;
        private bool mClosed;
        private byte[] mDataBuffer;
        private string mIP;
        private string mLongIP;
        private bool Authorizated;

        private int mDisconnections;
        internal int Disconnection
        {
            get
            {
                return mDisconnections;
            }
            set
            {
                mDisconnections = value;
            }
        }

        private int mDisconnectionErrors;
        internal int DisconnectionError
        {
            get
            {
                return mDisconnectionErrors;
            }
            set
            {
                mDisconnectionErrors = value;
            }
        }

        public Session(Socket pSock)
        {
            mSock = pSock;
            mDataBuffer = new byte[1024];
            mReceivedCallback = new AsyncCallback(BytesReceived);
            mClosed = false;
            Authorizated = false;
            Logging.WriteLine("Received connection");

            mIP = mSock.RemoteEndPoint.ToString().Split(':')[0];
            mLongIP = pSock.RemoteEndPoint.ToString();
            SendData("authreq");
            ContinueListening();
        }

        private int count = 0;
        private int queryspersecond = 0;

        private void BytesReceived(IAsyncResult pIar)
        {
            try
            {
                int BytesReceived = mSock.EndReceive(pIar);
                try
                {

                    byte[] ReceivedData = new byte[BytesReceived];
                    Array.Copy(mDataBuffer, ReceivedData, BytesReceived);
                    string[] Packets = System.Text.Encoding.Default.GetString(mDataBuffer, 0, BytesReceived).Split('|');
                    foreach (string Packet in Packets)
                    {
                        if (string.IsNullOrEmpty(Packet))
                            continue;
                        string[] Data = Packet.Split(':');
                        if (Data[0].Length == 0)
                        {
                            Close();
                            return;
                        }

                        switch (Data[0])
                        {
                            /*
                             * 

                             */
                            case "auth":
                                {
                                    if (Data[1] == "bitch")
                                    {
                                        Logging.WriteLine(mIP + " -> Reached session");
                                        SendData("authok");
                                        SessionManagement.RegisterSession(this);
                                        Authorizated = true;
                                        
                                        DatabaseStats.totalQueries = 0;
                                        DatabaseStats.totalQueryTime = 0;
                                    }
                                    else
                                    {
                                        Close();
                                    }
                                    break;
                                }
                            case "upda":
                                {
                                    if (!Authorizated)
                                        Close();
                                    else
                                    {
                                        int Rate;
                                        if (mDisconnections == 0)
                                            Rate = 0;
                                        else if (mDisconnections == 0 && mDisconnectionErrors > 0)
                                            Rate = mDisconnectionErrors;
                                        else
                                            Rate = mDisconnectionErrors / mDisconnections;
                                        mDisconnections = 0;
                                        mDisconnectionErrors = 0;

                                        int totalQueries = DatabaseStats.totalQueries;
                                        double totalQueryTime = DatabaseStats.totalQueryTime;

                                        DatabaseStats.totalQueries = 0;
                                        DatabaseStats.totalQueryTime = 0;

                                        int rate = 0;

                                        if (totalQueryTime != 0 && totalQueries != 0)
                                            rate = (int)(totalQueryTime / totalQueries);

                                        if (count == 0)
                                        {
                                            queryspersecond = totalQueries;
                                            count++;
                                        }
                                        else
                                        {
                                            count = 0;
                                        }

                                        //
                                        SendData("data:onlinecount=" + (ButterflyEnvironment.GetGame().GetClientManager().ClientCount) + ",roomloadcount=" + ButterflyEnvironment.GetGame().GetRoomManager().LoadedRoomsCount + ",disconnectionrate=" + Rate + ",qurate=" + rate + ",qps=" + queryspersecond);
                                    }
                                    break;
                                }
                        }
                    }
                    ContinueListening();

                }
                catch
                {
                    Close();
                }
            }
            catch
            {
                Close();
            }
        }


        private void ContinueListening()
        {
            try
            {
                mSock.BeginReceive(mDataBuffer, 0, mDataBuffer.Length, SocketFlags.None, mReceivedCallback, this);
            }
            catch
            {
                Close();
            }
        }

        private void SendData(string pData)
        {
            try
            {
                //Logging.WriteLine("Data sent ->" + pData);
                byte[] BytesToSend = System.Text.Encoding.Default.GetBytes(pData + "|");
                mSock.Send(BytesToSend);
            }
            catch
            {
                Close();
            }
        }

        internal void SendMessage(string data)
        {
            SendData(data);
        }

        internal void Close()
        {
            Authorizated = false;
            if (mClosed)
                return;
            else
                mClosed = true;

            try
            {
                mSock.Close();
            }
            catch { }

            //mSock = null;
            //mDataBuffer = null;
            SessionManagement.RemoveSession(this);
            Logging.WriteLine("Reached end of session");
            //GC.Collect();
        }

        internal string GetLongIP
        {
            get
            {
                return mSock.RemoteEndPoint.ToString();
            }
        }

        internal bool GetState()
        {
            try
            {
                return mSock.Connected;
            }
            catch
            {
                return false;
            }
        }
    }

}
