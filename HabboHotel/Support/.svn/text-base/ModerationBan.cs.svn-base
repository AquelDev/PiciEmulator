using System;
using Butterfly.Net;

namespace Butterfly.HabboHotel.Support
{
    enum ModerationBanType
    {
        IP = 0,
        USERNAME = 1
    }

    struct ModerationBan
    {
        internal ModerationBanType Type;
        internal string Variable;
        internal string ReasonMessage;
        internal Double Expire;

        internal Boolean Expired
        {
            get
            {
                if (ButterflyEnvironment.GetUnixTimestamp() >= Expire)
                    return true;

                return false;
            }
        }

        internal ModerationBan(ModerationBanType Type, string Variable, string ReasonMessage, Double Expire)
        {
            this.Type = Type;
            this.Variable = Variable;
            this.ReasonMessage = ReasonMessage;
            this.Expire = Expire;
        }
    }
}
