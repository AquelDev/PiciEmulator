using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Butterfly.HabboHotel.NewAchievements
{
    struct Achievement
    {
        private readonly uint id;
        private readonly AchievementBase achievement;
        private uint progress;
        private uint level;

        public Achievement(uint id, uint achievementID, uint level, uint progress)
        {
            this.id = id;
            this.achievement = AchievementManager.GetAchivement(achievementID);
            this.progress = progress;
            this.level = level;
        }

        internal AchievementBase AchievementBase
        {
            get
            {
                return achievement;
            }
        }

        internal uint Progress
        {
            get
            {
                return progress;
            }
            set
            {
                progress = value;
            }
        }

        internal uint Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }
    }
}
