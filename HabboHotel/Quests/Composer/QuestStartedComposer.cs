using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pici.Messages;
using Pici.HabboHotel.Quests;
using Pici.HabboHotel.GameClients;

namespace Pici.HabboHotel.Quests.Composer
{
    class QuestStartedComposer
    {
        internal static ServerMessage Compose(GameClient Session, Quest Quest)
        {
            ServerMessage Message = new ServerMessage(802);
            QuestListComposer.SerializeQuest(Message, Session, Quest, Quest.Category);
            return Message;
        }
    }
}
