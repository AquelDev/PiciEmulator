using System;
using System.Collections.Generic;

namespace Butterfly.HabboHotel.RoomBots
{
    class BotResponse
    {
        internal UInt32 BotId;

        internal List<string> Keywords;

        internal string ResponseText;
        internal string ResponseType;

        internal int ServeId;

        internal BotResponse(UInt32 BotId, string Keywords, string ResponseText, string ResponseType, int ServeId)
        {
            this.BotId = BotId;
            this.Keywords = new List<string>();
            this.ResponseText = ResponseText;
            this.ResponseType = ResponseType;
            this.ServeId = ServeId;

            foreach (string Keyword in Keywords.Split(';'))
            {
                this.Keywords.Add(Keyword.ToLower());
            }
        }

        internal bool KeywordMatched(string Message)
        {
            foreach (string Keyword in Keywords)
            {
                if (Message.ToLower().Contains(Keyword.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
