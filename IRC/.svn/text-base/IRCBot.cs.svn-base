//using System;
//using System.IO;
//using System.Net.Sockets;
//using System.Threading;
//using System.Threading.Tasks;
//using Butterfly.Core;
//using System.Collections.Generic;
//using Butterfly.IRC;
//using System.Text;
//using Database_Manager.Database.Session_Details.Interfaces;
//using System.Data;
//using Butterfly.IRC.Messages;

//namespace Butterfly.IRC
//{
//    class IRCBot
//    {
//        #region Fields
//        private readonly string server;
//        private readonly int port;
//        private readonly string user;
//        private readonly string nick;
//        private readonly string channel;
//        private readonly string password;

//        private IRCPing ping;
//        private StreamWriter writer;
//        private Thread IRCLooper;
//        private bool isActive;
//        #endregion

//        #region Return values
//        internal StreamWriter ServerWriter
//        {
//            get
//            {
//                return writer;
//            }
//        }

//        internal string ConnectedServer
//        {
//            get
//            {
//                return server;
//            }
//        }

//        internal string Channel
//        {
//            get
//            {
//                return channel;
//            }
//        }
//        #endregion

//        #region Constructor / deconstructor
//        public IRCBot(string server, int port, string user, string nick, string channel, string password)
//        {
//            this.isActive = true;
//            this.server = server;
//            this.port = port;
//            this.user = user;
//            this.nick = nick;
//            this.channel = channel;
//            this.password = password;

//            this.ping = new IRCPing(this);
//            this.IRCLooper = new Thread(new ThreadStart(CallbackThread));
//        }
//        #endregion

//        #region Methods
//        internal void Start()
//        {
//            IRCLooper.Start();
//        }

//        private void CallbackThread()
//        {
//            try
//            {
//                TcpClient irc = new TcpClient(server, port);
//                NetworkStream stream = irc.GetStream();
//                StreamReader reader = new StreamReader(stream);
//                writer = new StreamWriter(stream);

//                ping.Start();

//                writer.WriteLine(user);
//                writer.Flush();
//                writer.WriteLine("NICK " + nick);
//                writer.Flush();
//                writer.WriteLine("NICKSERV IDENTIFY " + password);
//                writer.Flush();
//                writer.WriteLine(string.Format("JOIN {0} {1}", channel, password));
//                writer.Flush();

//                string inputLine;
//                while ((inputLine = reader.ReadLine()) != null && isActive)
//                {
//                    if (inputLine.Contains("PRIVMSG"))
//                    {
//                        string username = inputLine.Split('!')[0].Substring(1);

//                        List<IServerMessage> repliesFromCommand = new List<IServerMessage>();
//                        if (UserFactory.IsRegistered(username))
//                        {
//                            User cachedUser = UserFactory.GetUser(username);
//                            string[] input = inputLine.Split(':');
//                            repliesFromCommand = CommandHandler.InvokeCommand(MergeParams(input, 2, ':'), cachedUser);
//                        }
//                        else
//                        {
//                            repliesFromCommand.Add(new PrivateMessage(string.Format("Username {0} is not registered!", username)));
//                        }

//                        foreach (IServerMessage reply in repliesFromCommand)
//                        {
//                            string packet = reply.SerializeForUser(username);
//                            writer.WriteLine(packet);
//                        }
//                        writer.Flush();
//                    }
//                }
//            }
//            catch (ThreadAbortException) { }
//            catch (Exception e)
//            {
//                Logging.HandleException(e, "IRCBot.CallbackThread");
//                Console.WriteLine(string.Format("WARNING! IRC client crashed: {0}", e.ToString()));
//            }
//        }

//        internal void Shutdown()
//        {
//            try
//            {
//                IRCLooper.Abort();
//            }
//            catch (Exception e) { Logging.HandleException(e, "IRCBot.Shutdown"); }
//            isActive = false;
//            try
//            {
//                writer.Close();
//            }
//            catch (Exception e) { Logging.HandleException(e, "IRCBot.Shutdown"); }
//        }

//        internal void SendMassMessage(IServerMessage reply, bool flushAfterWrite)
//        {
//            writer.WriteLine(reply.SerializeForChannel(this.channel));
//            if (flushAfterWrite)
//                writer.Flush();
//        }

//        internal void SendPrivateMessage(IServerMessage reply, bool flushAfterWrite, string username)
//        {
//            writer.WriteLine(reply.SerializeForUser(username));
//            if (flushAfterWrite)
//                writer.Flush();
//        }


//        internal static string MergeParams(string[] Params, int Start, char appendChar)
//        {
//            StringBuilder MergedParams = new StringBuilder();

//            for (int i = 0; i < Params.Length; i++)
//            {
//                if (i < Start)
//                {
//                    continue;
//                }

//                if (i > Start)
//                {
//                    MergedParams.Append(appendChar);
//                }

//                MergedParams.Append(Params[i]);
//            }

//            return MergedParams.ToString();
//        }
//        #endregion
//    }
//}