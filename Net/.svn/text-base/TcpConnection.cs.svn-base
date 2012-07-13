using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Butterfly.Core;
using Butterfly.Messages;
using Butterfly.Util;
using Database_Manager.Database;
using Database_Manager.Database.Session_Details.Interfaces;

namespace Butterfly.Net
{
    /// <summary>
    /// Represents a TCP network connection accepted by an IonTcpConnectionListener. Provides methods for sending and receiving data, aswell as disconnecting the connection.
    /// </summary>
    //internal class TcpConnection // : Socket
    //{
        //#region Fields
        //private Socket sock;

        //private bool mDisposed = false;
        ///// <summary>
        ///// The ID of this connection as a 32 bit unsigned integer.
        ///// </summary>
        //private readonly uint mID;
        ///// <summary>
        ///// The byte array holding the buffer for receiving data from client.
        ///// </summary>
        //private byte[] mDataBuffer;
        ///// <summary>
        ///// The AsyncCallback instance for the thread for receiving data asynchronously.
        ///// </summary>
        //private AsyncCallback mDataReceivedCallback;
        //private AsyncCallback mDataSentCallback;
        ///// <summary>
        ///// The RouteReceivedDataCallback to route received data to another object.
        ///// </summary>
        //private RouteReceivedDataCallback mRouteReceivedDataCallback;
        //#endregion

        //#region Members
        //internal delegate void RouteReceivedDataCallback(ref byte[] Data);
        //#endregion

        //#region Properties
        ///// <summary>
        ///// Gets the ID of this connection as a 32 bit unsigned integer.
        ///// </summary>
        //internal uint ID
        //{
        //    get { return mID; }
        //}
        //private string mIP;
        ///// <summary>
        ///// Gets the IP address of this connection as a string.
        ///// </summary>
        //internal string ipAddress
        //{
        //    get
        //    {
        //       return mIP;
        //    }
        //}

        ////private List<ServerMessage> packetQueue;
        //#endregion

        //#region Constructors
        ///// <summary>
        ///// Constructs a new instance of IonTcpConnection for a given connection identifier and socket.
        ///// </summary>
        ///// <param name="ID">The unique ID used to identify this connection in the environment.</param>
        ///// <param name="pSocket">The System.Net.Sockets.Socket of the new connection.</param>
        ////internal TcpConnection(uint ID, Socket pSocket)
        //internal TcpConnection(uint pSockID, SocketInformation pSockInfo)
        //   // : base(pSockInfo)
        //{
        //    sock = new Socket(pSockInfo);
        //    mID = pSockID;
        //    mIP = sock.RemoteEndPoint.ToString().Split(':')[0];
        //}

        //#endregion

        //#region Methods
        ///// <summary>
        ///// Starts the connection, prepares the received data buffer and waits for data.
        ///// </summary>
        //internal void Start(RouteReceivedDataCallback dataRouter)
        //{
        //    mDataBuffer = new byte[1024];
        //    mDataReceivedCallback = new AsyncCallback(DataReceived);
        //    mDataSentCallback = new AsyncCallback(DataSent);
        //    mRouteReceivedDataCallback = dataRouter;

        //    WaitForData();
        //}

        //private bool mDropped;

        //internal void DropConnection()
        //{
        //    if (mDropped)
        //        return;
        //    mDropped = true;

        //    //ThreadPool.QueueUserWorkItem(DropConnectionCallback);
        //    DropConnectionCallback();
        //}

        //private void DropConnectionCallback()
        //{
        //    try
        //    {
        //        FinalShutdown();
        //        ButterflyEnvironment.GetGame().GetClientManager().DisposeConnection(mID);
        //    }
        //    catch (Exception e) { Logging.HandleException(e, "TcpConnection.DropConnectionCallback"); }
        //}

        //internal void SendData(byte[] Data)
        //{
        //    if (mDisposed)
        //        return;
        //    try
        //    {
        //        if (Data.Length == 0)
        //            return;
        //        //base.Send(Data);
        //        //Logging.WriteLine("Data sent to client: [" + System.Text.Encoding.Default.GetString(Data, 0, Data.Length) + "]");
        //        int DataLength = Data.Length;
        //        mDataSent += DataLength;
        //        if (sock.Connected)
        //            sock.BeginSend(Data, 0, Data.Length, SocketFlags.None, mDataSentCallback, this);
        //    }
        //    catch
        //    {
        //        DropConnection();
        //    }
        //}

        //private static int mDataSent = 0;
        //private static int mDataReceived = 0;

        //internal static int GetNumberOfSentBytes()
        //{
        //    int Tmp = mDataSent;
        //    mDataSent = 0;
        //    return Tmp;
        //}

        //internal static int GetNumberOfReceivedBytes()
        //{
        //    int Tmp = mDataReceived;
        //    mDataReceived = 0;
        //    return Tmp;
        //}

        //private void DataSent(IAsyncResult Iar)
        //{
        //    if (mDisposed)
        //        return;
        //    try
        //    {
        //        sock.EndSend(Iar);
        //    }
        //    catch
        //    {
        //        DropConnection();
        //    }
        //}

        //internal void SendData(string sData)
        //{
        //    SendData(ButterflyEnvironment.GetDefaultEncoding().GetBytes(sData));
        //}

        //internal void InvokeSendMessage(ServerMessage Message)
        //{
        //    if (Message == null)
        //        return;

        //    byte[] toSend = Message.GetBytes();
        //    //Logging.WriteLine("Data [" + Message.Id + "] sent to client: [" + System.Text.Encoding.Default.GetString(toSend, 0, toSend.Length) + "]");

        //    SendData(toSend);
        //}

        //private void sendDataCallback(object pCallbackObject)
        //{
        //    if (!mDisposed && sock.Connected)
        //    {
        //        try
        //        {
        //            byte[] BytesToSend = pCallbackObject as byte[];
        //            if (sock.Connected)
        //                sock.BeginSend(BytesToSend, 0, BytesToSend.Length, SocketFlags.None, mDataSentCallback, this);
        //            //Logging.WriteLine("Data sent to client: [" + System.Text.Encoding.Default.GetString(BytesToSend, 0, BytesToSend.Length) + "]");
        //        }
        //        catch
        //        {
        //            DropConnection();
        //        }
        //    }
        //}

        //private void WaitForData()
        //{
        //    if (mDisposed || !sock.Connected)
        //        return;
        //    try
        //    {
        //        sock.BeginReceive(mDataBuffer, 0, 1024, SocketFlags.None, mDataReceivedCallback, this);
        //    }
        //    catch
        //    {
        //        DropConnection();
        //    }
        //}

        //private void DataReceived(IAsyncResult iAr)
        //{
        //    if (mDisposed || !sock.Connected)
        //        return;
        //    try
        //    {
        //        int numReceivedBytes = 0;
        //        try
        //        {
        //            if (!sock.Connected)
        //                return;
        //            numReceivedBytes = sock.EndReceive(iAr);
        //            mDataReceived += numReceivedBytes;
        //        }
        //        catch 
        //        {
        //            DropConnection();
        //            return;
        //        }

        //        if (numReceivedBytes > 0)
        //        {
        //            byte[] dataToProcess = ByteUtil.ChompBytes(mDataBuffer, 0, numReceivedBytes);
                    
        //            RouteData(ref dataToProcess);
        //        }
        //        else
        //        {
        //            DropConnection();
        //            return;
        //        }

        //        WaitForData();
        //    }
        //    catch 
        //    {
        //        DropConnection();
        //        return;
        //    }
        //}

        //private void RouteData(ref byte[] Data)
        //{
        //    if (mRouteReceivedDataCallback != null)
        //        mRouteReceivedDataCallback.Invoke(ref Data);
        //}
        //#endregion

        //#region IDisposable members
        
        //private void FinalShutdown() //ITS THE FAINAL COUNTDOWN! *DURURU RUUU*
        //{
        //    if (!this.mDisposed)
        //    {
        //        mDisposed = true;

        //        try
        //        {
        //            sock.Shutdown(SocketShutdown.Both);
        //            sock.Close();
        //            sock.Dispose();
        //        }
        //        catch (Exception e)
        //        {
        //            Logging.HandleException(e, "TcpConnection.Dispose");
        //        }

        //        Array.Clear(mDataBuffer, 0, mDataBuffer.Length);

        //        ButterflyEnvironment.GetConnectionManager().CloseConnection(this.mID);
        //        TcpAuthorization.FreeConnection(mIP);

        //        mDataBuffer = null;
        //        mDataReceivedCallback = null;
        //        mDataSentCallback = null;
        //        mRouteReceivedDataCallback = null;
        //        sock = null;

        //        Logging.WriteLine("[" + this.mID + "] -> Disposed");
        //    }
        //}
        //#endregion
    //}
}
