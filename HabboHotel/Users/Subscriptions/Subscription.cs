using System;
using Pici.Core;

namespace Pici.HabboHotel.Users.Subscriptions
{
    class Subscription
    {
        private string Caption;

        //private int TimeActivated;
        private int TimeExpire;

        internal string SubscriptionId
        {
            get
            {
                return Caption;
            }
        }

        internal int ExpireTime
        {
            get
            {
                return TimeExpire;
            }
        }

        internal Subscription(string Caption, int TimeExpire)
        {
            this.Caption = Caption;
            //this.TimeActivated = TimeActivated;
            this.TimeExpire = TimeExpire;
        }

        internal Boolean IsValid()
        {
            if (TimeExpire <= PiciEnvironment.GetUnixTimestamp())
            {
                return false;
            }

            return true;
        }

        internal void SetEndTime(int time)
        {
            this.TimeExpire = time;
        }

        internal void ExtendSubscription(int Time)
        {
            try
            {
                TimeExpire = TimeExpire + Time;
            }
            catch (Exception e)
            {
                Logging.LogException("T: " + TimeExpire + "." + Time + e.ToString());
            }
        }
    }
}
