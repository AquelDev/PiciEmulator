using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Butterfly.Messages;

namespace Butterfly.HabboHotel.Achievements.Composer
{
    class AchievementScoreUpdateComposer
    {
        internal static ServerMessage Compose(int Score)
        {
            ServerMessage Message = new ServerMessage(443);
            Message.AppendInt32(Score);
            return Message;
        }
    }
}
