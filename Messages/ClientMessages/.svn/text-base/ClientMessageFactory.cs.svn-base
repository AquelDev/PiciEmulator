using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Butterfly.Messages.ClientMessages
{
    class ClientMessageFactory
    {
        private static Queue freeObjects;

        internal static void Init()
        {
            freeObjects = new Queue();
        }

        internal static ClientMessage GetClientMessage(int messageID, byte[] body)
        {
            if (freeObjects.Count > 0)
            {
                ClientMessage message;

                lock (freeObjects.SyncRoot)
                {
                    message = (ClientMessage)freeObjects.Dequeue();
                }
                if (message == null)
                    return new ClientMessage(messageID, body);
                
                message.Init(messageID, body);
                return message;
            }
            else
            {
                return new ClientMessage(messageID, body);
            }
        }

        internal static void ObjectCallback(ClientMessage message)
        {
            lock (freeObjects.SyncRoot)
            {
                freeObjects.Enqueue(message);
            }
        }
    }
}
