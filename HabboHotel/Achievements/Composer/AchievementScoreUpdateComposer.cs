using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pici.Messages;

namespace Pici.HabboHotel.Achievements.Composer
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
