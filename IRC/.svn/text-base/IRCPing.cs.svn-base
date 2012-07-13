//using System;
//using System.Threading;
//using Butterfly.Core;

//namespace Butterfly.IRC
//{
//    class IRCPing
//    {
//        #region Fields
//        private static readonly string PING = "PING :";
//        private Thread pingSender;
//        private IRCBot bot;
//        #endregion

//        #region Constructor/deconstructor
//        public IRCPing(IRCBot bot)
//        {
//            this.bot = bot;
//            this.pingSender = new Thread(new ThreadStart(this.Run));
//        }
//        #endregion

//        #region Methods
//        internal void Start()
//        {
//            pingSender.Start();
//        }

//        internal void Run()
//        {
//            while (true)
//            {
//                try
//                {
//                    bot.ServerWriter.WriteLine(PING + bot.ConnectedServer);
//                    bot.ServerWriter.Flush();
//                }
//                catch (Exception e)
//                {
//                    Logging.HandleException(e, "IRCPing.Run");
//                }
//                Thread.Sleep(15000);
//            }
//        }
//        #endregion
//    }
//}