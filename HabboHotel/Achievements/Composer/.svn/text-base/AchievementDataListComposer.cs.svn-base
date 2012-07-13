using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Butterfly.Messages;

namespace Butterfly.HabboHotel.Achievements.Composer
{
    class AchievementDataListComposer
    {
        internal static ServerMessage Compose(List<Achievement> Achievements)
        {
            ServerMessage Message = new ServerMessage(627); //627
            Message.AppendInt32(Achievements.Count);

            foreach (Achievement Achievement in Achievements)
            {
                string DisplayName = Achievement.GroupName;

                if (DisplayName.StartsWith("ACH_"))
                {
                    DisplayName = DisplayName.Substring(4);
                }

                Message.AppendStringWithBreak(DisplayName);
                Message.AppendInt32(Achievement.Levels.Count);

                foreach (AchievementLevel Level in Achievement.Levels.Values)
                {
                    Message.AppendInt32(Level.Level);
                    Message.AppendInt32(Level.Requirement);
                }
            }

            return Message;
        }
    }
}
