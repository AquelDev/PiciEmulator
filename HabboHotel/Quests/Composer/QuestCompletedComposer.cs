using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pici.Messages;
using Pici.HabboHotel.GameClients;
using Pici.HabboHotel.Quests;

namespace Pici.HabboHotel.Quests.Composer
{
    class QuestCompletedComposer
    {
        internal static ServerMessage Compose(GameClient Session, Quest Quest)
        {
            ServerMessage Message = new ServerMessage(801);
            QuestListComposer.SerializeQuest(Message, Session, Quest, Quest.Category);
            return Message;
        }
    }
}
