
using Butterfly.Messages;
using System.Collections.Generic;
namespace Butterfly.HabboHotel.Achievements
{
    class Achievement
    {
        internal readonly uint Id;
        internal readonly string GroupName;
        internal readonly string Category;
        internal readonly Dictionary<int, AchievementLevel> Levels;

        public Achievement(uint Id, string GroupName, string Category)
        {
            this.Id = Id;
            this.GroupName = GroupName;
            this.Category = Category;
            this.Levels = new Dictionary<int, AchievementLevel>();
        }

        public void AddLevel(AchievementLevel Level)
        {
            Levels.Add(Level.Level, Level);
        }
    }
}
