using System;
using Pici.Messages;
using Pici.Net;
using Pici.Core.ConnectionManager;
using System.IO;
using System.Collections.Generic;

namespace Pici.Messages
{
    public class QueuedServerMessage
    {
        private List<byte> packet;
        private ConnectionInformation userConnection;

        internal byte[] getPacket
        {
            get
            {
                return packet.ToArray();
            }
        }

        public QueuedServerMessage(ConnectionInformation connection)
        {
            this.userConnection = connection;
            this.packet = new List<byte>(4096);
        }

        internal void Dispose()
        {
            packet.Clear();
            userConnection = null;
        }

        private void Appends(byte[] bytes)
        {
            packet.AddRange(bytes);
        }

        internal void appendResponse(ServerMessage message)
        {
            Appends(message.GetBytes());
        }

        internal void addBytes(byte[] bytes)
        {
            Appends(bytes);
        }

        internal void sendResponse()
        {
            userConnection.SendData(packet.ToArray());
            Dispose();
        }
    }
}
