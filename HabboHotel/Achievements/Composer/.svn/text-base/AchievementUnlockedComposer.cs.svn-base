using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Butterfly.Messages;

namespace Butterfly.HabboHotel.Achievements.Composer
{
    class AchievementUnlockedComposer
    {
        internal static ServerMessage Compose(Achievement Achievement, int Level, int PointReward, int PixelReward)
        {
            ServerMessage Message = new ServerMessage(437);
            Message.AppendUInt(Achievement.Id);                                           // Achievement ID
            Message.AppendInt32(Level);                                                     // Achieved level
            Message.AppendInt32(144);                                                         // Unknown. Random useless number.
            Message.AppendStringWithBreak(Achievement.GroupName + Level);                   // Achieved name
            Message.AppendInt32(PointReward);                                               // Point reward
            Message.AppendInt32(PixelReward);                                               // Pixel reward
            Message.AppendInt32(0);                                                         // Unknown.
            Message.AppendInt32(10);                                                         // Unknown.
            Message.AppendInt32(21);                                                         // Unknown. (Extra reward?)
            Message.AppendStringWithBreak(Level > 1 ? Achievement.GroupName + (Level - 1) : string.Empty);
            Message.AppendStringWithBreak(Achievement.Category);
            return Message;
        }
    }
}
