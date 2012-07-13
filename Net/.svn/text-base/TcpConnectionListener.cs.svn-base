using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Butterfly.Core;

namespace Butterfly.Net
{
    //class TcpConnectionListener
    //{
    //    private const int QUEUE_LENGTH = 1;

    //    private Socket Listener;
    //    private bool IsListening;
    //    private AsyncCallback ConnectionReqCallback;
    //    private int mSystraID;

    //    private TcpConnectionManager Manager;

    //    internal TcpConnectionListener(int Port, TcpConnectionManager Manager)
    //    {
    //        this.mSystraID = Process.GetCurrentProcess().Id;
    //        //IPAddress IP = null;

    //        Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //        IPEndPoint Endpoint = new IPEndPoint(IPAddress.Any, Port);
    //        Listener.Bind(Endpoint);

    //        Listener.Listen(1000);
    //        ConnectionReqCallback = new AsyncCallback(ConnectionRequest);
    //        this.Manager = Manager;
            
            
    //    }

    //    private void ConnectionRequest(IAsyncResult iAr)
    //    {
    //        if (!IsListening)
    //            return;
    //        try
    //        {
    //            Socket ClientSock = ((Socket)iAr.AsyncState).EndAccept(iAr);
    //            if (TcpAuthorization.CheckConnection(ClientSock))
    //            {
    //                Manager.HandleNewConnection(ClientSock.DuplicateAndClose(mSystraID));
    //            }
    //            else
    //            {
    //                try
    //                {
    //                    ClientSock.Dispose();
    //                    ClientSock.Close();
    //                }
    //                catch (Exception e)
    //                {
    //                    Logging.HandleException(e, "TcpConnectionListener.ConnectionRequest");
    //                }
    //            }
    //        }
    //        //catch (SocketException) { }
    //        catch (Exception e)
    //        {
    //            //Logging.LogException("[TCPListener.OnRequest]: Could not handle new connection request: " + e.ToString());
    //            Logging.HandleException(e, "Socket listener");
    //        }
    //        finally
    //        {
    //            WaitForNextConnection();
    //        }
    //    }


    //    internal void Start()
    //    {
    //        if (IsListening)
    //        {
    //            return;
    //        }

    //        IsListening = true;

    //        Listener.BeginAccept(ConnectionReqCallback, Listener);
    //        //Logging.WriteLine("Listener -> STARTED");
    //        Logging.WriteLine("Socket -> READY!");
    //    }

    //    internal void Stop()
    //    {
    //        if (!IsListening)
    //            return;

    //        IsListening = false;
    //        try
    //        {
    //            Listener.Close();
    //        }
    //        catch { }
    //        Console.WriteLine("Listener -> Stopped!");
    //    }

    //    internal void Destroy()
    //    {
    //        Stop();

    //        //Listener = null;
    //        //Manager = null;
    //    }

    //    private void WaitForNextConnection()
    //    {
    //        try
    //        {
    //            //internal IAsyncResult BeginAccept(Socket acceptSocket, int receiveSize, AsyncCallback callback, object state);
    //            //Listener.BeginAccept(Listener, 1, ConnectionReqCallback, this);
    //            Listener.BeginAccept(ConnectionReqCallback, Listener);
    //        }
    //        catch (Exception e)
    //        {
    //            Logging.HandleException(e, "TcpConnectionListener.WaitForNextConnection");
    //        }
    //    }

    //    internal void Close()
    //    {
    //        IsListening = false;

    //        try
    //        {
    //            Listener.Shutdown(SocketShutdown.Both);
    //            Listener.Close();
    //            Listener.Dispose();
    //        }
    //        catch { }
    //    }
    //}
}
