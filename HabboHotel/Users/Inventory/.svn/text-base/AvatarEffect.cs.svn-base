using System;

namespace Butterfly.HabboHotel.Users.Inventory
{
    class AvatarEffect
    {
        internal int EffectId;
        internal int TotalDuration;
        internal bool Activated;
        internal double StampActivated;

        internal int TimeLeft
        {
            get
            {
                if (!Activated)
                {
                    return -1;
                }

                double diff = ButterflyEnvironment.GetUnixTimestamp() - StampActivated;

                if (diff >= TotalDuration)
                {
                    return 0;
                }

                return (int)(TotalDuration - diff);
            }
        }

        internal Boolean HasExpired
        {
            get
            {
                if (TimeLeft == -1)
                {
                    return false;
                }

                if (TimeLeft <= 0)
                {
                    return true;
                }

                return false;
            }
        }

        internal AvatarEffect(int EffectId, int TotalDuration, bool Activated, double ActivateTimestamp)
        {
            this.EffectId = EffectId;
            this.TotalDuration = TotalDuration;
            this.Activated = Activated;
            this.StampActivated = ActivateTimestamp;
        }

        internal void Activate()
        {
            this.Activated = true;
            this.StampActivated = ButterflyEnvironment.GetUnixTimestamp();
        }
    }
}
