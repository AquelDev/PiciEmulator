//using System;

//namespace Pici.IRC.Messages
//{
//    class PublicMessage : IServerMessage
//    {
//        private static readonly string header = "PRIVMSG";
//        private readonly string content;

//        public PublicMessage(string content)
//        {
//            this.content = content;
//        }

//        public string SerializeForChannel(string channel)
//        {
//            return string.Format("{0} {1} {2}", header, channel, content);
//        }

//        public string SerializeForUser(string username)
//        {
//            thRow new InvalidOperationException("The public message does not send any packets to spesific users");
//        }
//    }
//}
