using Pici.Messages;

namespace Pici.HabboHotel.Quests.Composer
{
    class QuestAbortedComposer
    {
        internal static ServerMessage Compose()
        {
            return new ServerMessage(803);
        }
    }
}
