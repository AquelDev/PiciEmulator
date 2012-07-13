using System.Collections.Generic;
using Butterfly.Messages;
using System;

namespace Butterfly.HabboHotel.ChatMessageStorage
{
    public class ChatMessageManager 
    {
        private LinkedList<ChatMessage> listOfMessages;
        private int amountOfMessagesInside;

        public ChatMessageManager()
        {
            listOfMessages = new LinkedList<ChatMessage>();
        }

        internal void AddMessage(ChatMessage message)
        {
            listOfMessages.AddFirst(message);

            if (amountOfMessagesInside == 100)
                listOfMessages.RemoveLast();
            else
                amountOfMessagesInside++;
        }

        internal Dictionary<int, List<ChatMessage>> GetSortedMessages()
        {
            int i = 0;
            Dictionary<int, List<ChatMessage>> messages = new Dictionary<int, List<ChatMessage>>();

            List<ChatMessage> currentWorking = new List<ChatMessage>();
            uint currentRoomID = 0;
            foreach (ChatMessage message in listOfMessages)
            {
                if (currentRoomID != message.roomID && currentWorking.Count > 0)
                {
                    i++;
                    messages.Add(i, currentWorking);
                    currentWorking.Clear();
                    currentRoomID = message.roomID;
                }

                currentWorking.Add(message);
            }

            return messages;
        }

        internal int messageCount
        {
            get
            {
                return amountOfMessagesInside;
            }
        }

        internal void Serialize(ref ServerMessage message)
        {
            foreach (ChatMessage chatMessage in listOfMessages)
            {
                chatMessage.Serialize(ref message);
            }
        }
    }
}
