using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Butterfly.Messages;
using Butterfly;

namespace Butterfly.HabboHotel.Users.Messenger
{
    struct SearchResult
    {
        internal uint userID;
        internal string username;
        internal string motto;
        internal string look;
        internal string last_online;

        public SearchResult(uint userID, string username, string motto, string look, string last_online)
        {
            this.userID = userID;
            this.username = username;
            this.motto = motto;
            this.look = look;
            this.last_online = last_online;
        }

        internal void Searialize(ServerMessage reply)
        {
            reply.AppendUInt(userID);
            reply.AppendStringWithBreak(username);
            reply.AppendStringWithBreak(motto);

            bool Online = (ButterflyEnvironment.GetGame().GetClientManager().GetClient(userID) != null);

            reply.AppendBoolean(Online);
          
            reply.AppendBoolean(false);

            reply.AppendStringWithBreak(string.Empty);
            reply.AppendBoolean(false);
            reply.AppendStringWithBreak(look);
            reply.AppendStringWithBreak(last_online);
            reply.AppendStringWithBreak(string.Empty);
        }
    }
}
