using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Butterfly.HabboHotel.GameClients;

namespace Butterfly.Messages
{
    struct ChatCommand
    {
        internal readonly int commandID;
        internal readonly string input;
        internal readonly int minrank;
        internal readonly string description;
        internal readonly string prefix;
        private string[] clubsAllowed;

        public ChatCommand(int pCommandID, string pInput, int pRank, string pDescription, string pPrefix, string[] clubsAllowed)
        {
            this.commandID = pCommandID;
            this.input = pInput;
            this.minrank = pRank;
            this.description = pDescription;
            this.prefix = pPrefix;
            this.clubsAllowed = clubsAllowed;
        }

        internal bool UserGotAuthorization(GameClient session)
        {
            foreach(string subs in clubsAllowed){
                if (session.GetHabbo().GetSubscriptionManager().HasSubscription(subs))
                    return true;
            }

            if (minrank == 0)
                return true;
            else if (minrank > 0)
            {
                if (minrank <= session.GetHabbo().Rank)
                    return true;
            }
            else if (minrank < 0)
            {
                if (minrank == -1)
                {
                    if (session.GetHabbo().CurrentRoom.CheckRights(session, false))
                        return true;
                }
                else if (minrank == -2)
                {
                    if (session.GetHabbo().CurrentRoom.CheckRights(session, true))
                        return true;
                }
            }

            return false;
        }
    }
}
