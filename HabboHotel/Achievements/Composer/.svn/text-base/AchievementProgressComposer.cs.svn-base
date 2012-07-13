using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Butterfly.Messages;

namespace Butterfly.HabboHotel.Achievements.Composer
{
    class AchievementProgressComposer
    {
        internal static ServerMessage Compose(Achievement Achievement, int TargetLevel, AchievementLevel TargetLevelData,
            int TotalLevels, UserAchievement UserData)
        {
            ServerMessage Message = new ServerMessage(913);
            Message.AppendUInt(Achievement.Id);                                               // Unknown (ID?)
            Message.AppendInt32(TargetLevel);                                                   // Target level
            Message.AppendStringWithBreak(Achievement.GroupName + TargetLevel);                 // Target name/desc/badge
            Message.AppendInt32(TargetLevelData.Requirement);                                   // Progress req/target        
            Message.AppendInt32(TargetLevelData.RewardPixels);                                   // Pixel reward       
            Message.AppendInt32(TargetLevelData.RewardPoints);                                  // Unknown(??)
            Message.AppendInt32(UserData != null ? UserData.Progress : 0);                      // Current progress
            Message.AppendBoolean(UserData != null ? (UserData.Level >= TotalLevels) : false);  // Set 100% completed(??)
            Message.AppendStringWithBreak(Achievement.Category);                                // Category
            Message.AppendInt32(TotalLevels);                                                   // Total amount of levels 
            return Message;
        }
    }
}
