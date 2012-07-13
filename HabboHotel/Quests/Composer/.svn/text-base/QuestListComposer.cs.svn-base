using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Butterfly.Messages;
using Butterfly.HabboHotel.GameClients;
using Butterfly.HabboHotel.Quests;

namespace Butterfly.HabboHotel.Quests.Composer
{
    enum QuestRewardType
        {
            Pixels = 0,
            Snowflakes = 1,
            Love = 2,
            PixelsBROKEN = 3,
            Seashells = 4
        }

    class QuestListComposer
    {
        internal static ServerMessage Compose(GameClient Session, List<Quest> Quests, bool Send)
        {
            Dictionary<string, int> UserQuestGoals = new Dictionary<string, int>();
            Dictionary<string, Quest> UserQuests = new Dictionary<string, Quest>();

            foreach (Quest Quest in Quests)
            {
                if (!UserQuestGoals.ContainsKey(Quest.Category))
                {
                    UserQuestGoals.Add(Quest.Category, 1);
                    UserQuests.Add(Quest.Category, null);
                }

                if (Quest.Number >= UserQuestGoals[Quest.Category])
                {
                    int UserProgress = Session.GetHabbo().GetQuestProgress(Quest.Id);

                    if (Session.GetHabbo().CurrentQuestId != Quest.Id && UserProgress >= Quest.GoalData)
                    {
                        UserQuestGoals[Quest.Category] = Quest.Number + 1;
                    }
                }
            }

            foreach (Quest Quest in Quests)
            {
                foreach (KeyValuePair<string, int> Goal in UserQuestGoals)
                {
                    if (Quest.Category == Goal.Key && Quest.Number == Goal.Value)
                    {
                        UserQuests[Goal.Key] = Quest;
                        break;
                    }
                }
            }

            ServerMessage Message = new ServerMessage(800);
            Message.AppendInt32(UserQuests.Count);

            // Active ones first
            foreach (KeyValuePair<string, Quest> UserQuest in UserQuests)
            {
                if (UserQuest.Value == null)
                {
                    continue;
                }

                SerializeQuest(Message, Session, UserQuest.Value, UserQuest.Key);
            }

            // Dead ones last
            foreach (KeyValuePair<string, Quest> UserQuest in UserQuests)
            {
                if (UserQuest.Value != null)
                {
                    continue;
                }

                SerializeQuest(Message, Session, UserQuest.Value, UserQuest.Key);
            }

            Message.AppendBoolean(Send);
            return Message;
        }

        internal static void SerializeQuest(ServerMessage Message, GameClient Session, Quest Quest, string Category)
        {
            int AmountInCat = ButterflyEnvironment.GetGame().GetQuestManager().GetAmountOfQuestsInCategory(Category);
            int Number = Quest == null ? AmountInCat : Quest.Number - 1;
            int UserProgress = Quest == null ? 0 : Session.GetHabbo().GetQuestProgress(Quest.Id);

            if (Quest != null && Quest.IsCompleted(UserProgress))
            {
                Number++;
            }

            Message.AppendStringWithBreak(Category);
            Message.AppendInt32(Number); // Quest progress in this cat
            Message.AppendInt32(AmountInCat); // Total quests in this cat
            Message.AppendInt32((int)QuestRewardType.Pixels); // Reward type (1 = Snowflakes, 2 = Love hearts, 3 = Pixels, 4 = Seashells, everything else is pixels
            Message.AppendUInt(Quest == null ? 0 : Quest.Id); // Quest id
            Message.AppendBoolean(Quest == null ? false : Session.GetHabbo().CurrentQuestId == Quest.Id); // Quest started
            Message.AppendStringWithBreak(Quest == null ? string.Empty : Quest.ActionName);
            Message.AppendStringWithBreak(Quest == null ? string.Empty : Quest.DataBit);
            Message.AppendInt32(Quest == null ? 0 : Quest.Reward);
            Message.AppendStringWithBreak(Quest == null ? string.Empty : Quest.Name);
            Message.AppendInt32(UserProgress); // Current progress
            Message.AppendUInt(Quest == null ? 0 : Quest.GoalData); // Target progress
            Message.AppendInt32(0); // "Next quest available countdown" in seconds
        }
    }
}
