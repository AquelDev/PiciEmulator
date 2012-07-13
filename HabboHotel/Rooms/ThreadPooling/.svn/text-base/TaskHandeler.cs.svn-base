using System;
using System.Collections.Generic;
using System.Threading;

namespace Butterfly.HabboHotel.Rooms.ThreadPooling
{
    class TaskHandeler
    {
        private delegate void noParameterDelegate();
        private event noParameterDelegate threadStopped;

        /// <summary>
        /// The checkhold wheter this item is choked or not, in MS
        /// </summary>
        private int chokeTreshHold
        {
            get;
            set;
        }

        /// <summary>
        /// The sleeptime in MS
        /// </summary>
        private int sleepTime
        {
            get;
            set;
        }

        /// <summary>
        /// Returns an indication wheter 
        /// </summary>
        public bool isChoked
        {
            get
            {
                if (this.procesTime != 0 && this.procesTime < chokeTreshHold)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// Checks if the handeler is running
        /// </summary>
        public bool running
        {
            get;
            private set;
        }

        /// <summary>
        /// Indicates if this item is currently synchronized
        /// </summary>
        public bool isSynchronized
        {
            get;
            private set;
        }

        /// <summary>
        /// The ID of this handeler
        /// </summary>
        public int id
        {
            get;
            private set;
        }

        /// <summary>
        /// The time the last cycle took for processing
        /// </summary>
        public int procesTime
        {
            get;
            private set;
        }
        /// <summary>
        /// A private handeler for the items
        /// </summary>
        private Thread handeler;

        /// <summary>
        /// The queue which contains the current messages which need processing
        /// </summary>
        private Queue<IProcessable> messageQueue;

        /// <summary>
        /// creates a new MessageHandeler
        /// </summary>
        /// <param name="id">The id of the processor</param>
        /// <param name="sleepTime">The sleeptime in milli seconds</param>
        /// <param name="chokeTreshHold">The chokeTreshHold in milli seconds</param>
        public TaskHandeler(int id, int sleepTime, int chokeTreshHold)
        {
            this.id = id;
            this.sleepTime = sleepTime;
            this.chokeTreshHold = chokeTreshHold;
            messageQueue = new Queue<IProcessable>();
        }

        /// <summary>
        /// Start handeling of the messages
        /// </summary>
        public void startHandeling()
        {
            if (!running)
            {
                this.handeler = new Thread(handleMessages);
                running = true;
                handeler.Start();
            }

        }

        /// <summary>
        /// Restarts the handeler
        /// </summary>
        public void restartHandeler()
        {
            threadStopped += startHandeling;
            this.running = false;
        }

        /// <summary>
        /// Starts handeling 
        /// </summary>
        public void stopHandeling()
        {
            if (this.handeler != null)
            {
                this.running = false;
            }
        }
        /// <summary>
        /// Enqueue's a message so it will be handeled
        /// </summary>
        /// <param name="information">The message information which needs to be enqueued</param>
        public void enqueueMessages(IProcessable information)
        {
            messageQueue.Enqueue(information);
        }

        /// <summary>
        /// handles all messages
        /// </summary>
        private void handleMessages()
        {
            DateTime startTime;
            IProcessable information;
            while (running)
            {
                startTime = DateTime.Now; ;
                if (messageQueue.Count != 0)
                {
                    information = messageQueue.Dequeue();
                    TaskProcessLogic.processMessage(information);
                }
                this.procesTime = (int)Math.Round((DateTime.Now - startTime).TotalMilliseconds);
                if (this.procesTime < sleepTime)
                    Thread.Sleep(this.sleepTime - this.procesTime);

            }
            this.handeler = null;
            this.running = false;
            if (threadStopped != null)
                threadStopped.Invoke();
            threadStopped = null;
        }

    }
}
