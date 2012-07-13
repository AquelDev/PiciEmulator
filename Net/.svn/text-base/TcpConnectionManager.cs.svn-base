using System;
using System.Net.Sockets;
using Butterfly.Core;

namespace Butterfly.Net
{
    //class TcpConnectionManager
    //{
    //    private TcpConnectionListener Listener;
    //    private TcpConnection[] freeConnections;
        
    //    internal TcpConnectionManager(int Port, int maxSimultaneousConnections)
    //    {
    //        freeConnections = new TcpConnection[maxSimultaneousConnections];
    //        Listener = new TcpConnectionListener(Port, this);
    //    }

    //    internal void DestroyManager()
    //    {
    //        Array.Clear(freeConnections, 0, freeConnections.Length);
    //    }

    //    internal TcpConnectionListener GetListener()
    //    {
    //        return Listener;
    //    }

    //    internal uint GenerateConnectionID()
    //    {
    //        for (uint i = 0; i < freeConnections.Length; i++)
    //        {
    //            if (freeConnections[i] == null)
    //                return i;
    //        }

    //        throw new InvalidOperationException("Maximum connections reached");
    //    }

    //    internal void HandleNewConnection(SocketInformation connectioninfo)
    //    {
    //        uint connectionID = GenerateConnectionID();
    //        TcpConnection Connection = new TcpConnection(connectionID, connectioninfo);
    //        freeConnections[connectionID] = Connection;

    //        ButterflyEnvironment.GetGame().GetClientManager().CreateAndStartClient(connectionID, Connection);
    //        Logging.WriteLine("[" + Connection.ipAddress + "] -> [" + connectionID + "]");
    //    }

    //    internal void DropConnection(uint Id)
    //    {
    //        freeConnections[Id] = null;
    //    }

    //    internal void Shutdown()
    //    {
    //        Listener.Close();

    //        Console.Title = "<<- SERVER SHUTDOWN ->> CONNECTION SHUTDOWN";
    //        DestroyManager();
    //    }
    //}
}
