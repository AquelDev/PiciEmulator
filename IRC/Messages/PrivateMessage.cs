//using System;

//namespace Pici.IRC.Messages
//{
//    class PrivateMessage : IServerMessage
//    {
//        private static readonly string header = "PRIVMSG";
//        private readonly string content;

//        public PrivateMessage(string content)
//        {
//            this.content = content;
//        }

//        public string SerializeForChannel(string channel)
//        {
//            thRow new InvalidOperationException("The public message does not send any packets to spesific users");
//        }

//        public string SerializeForUser(string username)
//        {
//            return string.Format("{0} {1} {2}", header, username.Replace(Environment.NewLine, string.Empty), content.Replace(Environment.NewLine, string.Empty));
//        }
//    }
//}
