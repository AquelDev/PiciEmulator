using Butterfly.Core;

namespace Butterfly.HabboHotel.Rooms.ThreadPooling
{
    class TaskManager
    {

        /// <summary>
        /// Holds information about the sleep time for the handelers
        /// </summary>
        private int taskHandelerSleepTime;

        /// <summary>
        /// The queue information
        /// </summary>
        private TaskHandeler[] queues;


        /// <summary>
        /// Creates a new message queue handeler
        /// Notice!
        /// Setting the amount of queue's above the "Environment.ProcessorCount" will cause 
        /// </summary>
        /// <param name="amounOfQueues">The amount of queue's that should handle the messages</param>
        /// <param name="taskHandelerSleepTime">The sleep time in milliseconds</param>
        /// <param name="taskHandelerBusyThreshold">The treshold when a "task handeler" is marked busy <see cref="explanation"/> time in ms </param>
        /// <example>
        /// Items are marked "busy" if the last proces time took longer than the given "taskHandelerBusyThreshold" if the proces time was 50 ms, and the "taskHandelerBusyThreshold" was set to 40, the item will be marked as busy
        /// </example>
        public TaskManager(int amounOfQueues, int taskHandelerSleepTime, int taskHandelerBusyThreshold)
        {
            Logging.LogMessage(string.Format("Initializing Task manager with {0} queues, sleep time: {1}ms, Busy trehold {2}ms ", amounOfQueues, taskHandelerSleepTime, taskHandelerBusyThreshold));
            this.taskHandelerSleepTime = taskHandelerSleepTime;
            queues = new TaskHandeler[amounOfQueues];
            for (int i = 0; i < amounOfQueues; i++)
            {
                queues[i] = new TaskHandeler(i, taskHandelerSleepTime, taskHandelerBusyThreshold);
                queues[i].startHandeling();
            }
            Logging.LogMessage("Done Initializing Task manager");
        }
        /// <summary>
        /// Enqueue's a processable message
        /// </summary>
        /// <param name="message"></param>
        public void enqueueMessage(IProcessable message)
        {

            Logging.LogMessage("Enqueing message long running is " + message.isLongRunTask());
            if (message.isLongRunTask())
            {
                longRunningProcessing(message);
            }
            else
            {
                TaskHandeler lowestHandeler = queues[0];
                int lowestProcesTime = queues[0].procesTime;
                for (int i = 1; i < queues.Length; i++)
                {
                    if (queues[i].procesTime < lowestProcesTime)
                    {
                        lowestHandeler = queues[i];
                        lowestProcesTime = queues[i].procesTime;
                    }
                }
                lowestHandeler.enqueueMessages(message);
            }
        }

        /// <summary>
        /// Destroys this handeler instance and all his childs
        /// </summary>
        public void destroy()
        {
            for (int i = 0; i < queues.Length; i++)
            {
                if (queues[i] != null)
                    queues[i].stopHandeling();
            }
        }
        /// <summary>
        /// Creates a long running processer
        /// </summary>
        /// <param name="message"></param>
        private void longRunningProcessing(IProcessable message)
        {
            //ImmidiateTaskProcessor processor = new ImmidiateTaskProcessor(message);
            //processor.processMessage();
        }
    }

}
