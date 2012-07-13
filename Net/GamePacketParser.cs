using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedPacketLib;
using Pici.Util;
using Pici.Messages;
using Pici.Core;
using Pici.Core.ConnectionManager;
using Pici.Messages.ClientMessages;

namespace Pici.Net
{
    public class GamePacketParser : IDataParser
    {
        private ConnectionInformation con;

        public delegate void HandlePacket(ClientMessage message);
        public event HandlePacket onNewPacket;

        public void SetConnection(ConnectionInformation con)
        {
            this.con = con;
            this.onNewPacket = null;
        }

        public void handlePacketData(byte[] data)
        {
            int pos = 0;
            while (pos < data.Length)
            {
                try
                {
                    int MessageLength = Base64Encoding.DecodeInt32(new byte[] { data[pos++], data[pos++], data[pos++] });
                    int MessageId = Base64Encoding.DecodeInt32(new byte[] { data[pos++], data[pos++] });

                    byte[] Content = new byte[MessageLength - 2];

                    for (int i = 0; i < Content.Length && pos < data.Length; i++)
                    {
                        Content[i] = data[pos++];
                    }
                    if (onNewPacket != null)
                    {
                        using (ClientMessage message = ClientMessageFactory.GetClientMessage(MessageId, Content))
                        {
                            onNewPacket.Invoke(message);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.HandleException(e, "packet handling");
                    con.Dispose();
                }
            }
        }

        public void Dispose()
        {
            this.onNewPacket = null;
        }

        public object Clone()
        {
            return new GamePacketParser();
        }
    }
}
