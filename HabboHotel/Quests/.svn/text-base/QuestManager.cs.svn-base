using System.Collections.Generic;
using System.Data;
using System.Linq;
using Butterfly.HabboHotel.GameClients;
using Butterfly.HabboHotel.Quests.Composer;
using Butterfly.Messages;
using Database_Manager.Database.Session_Details.Interfaces;
using System;

namespace Butterfly.HabboHotel.Quests
{
    class QuestManager
    {
        private Dictionary<uint, Quest> quests;
        private Dictionary<string, int> questCount;

        public QuestManager()
        { }

        public void Initialize(IQueryAdapter dbClient)
        {
            quests = new Dictionary<uint, Quest>();
            questCount = new Dictionary<string,int>();

            ReloadQuests(dbClient);
        }

        public void ReloadQuests(IQueryAdapter dbClient)
        {
            quests.Clear();

            dbClient.setQuery("SELECT * FROM quests");
            DataTable dTable = dbClient.getTable();

            uint id;
            string category;
            int num;
            int type;
            uint goalData;
            string name;
            int reward;
            string dataBit;
            foreach (DataRow dRow in dTable.Rows)
            {
                id = Convert.ToUInt32(dRow["id"]);
                category = (string)dRow["category"];
                num = (int)dRow["series_number"];
                type = (int)dRow["goal_type"];
                goalData = Convert.ToUInt32(dRow["goal_data"]);
                name = (string)dRow["name"];
                reward = (int)dRow["reward"];
                dataBit = (string)dRow["data_bit"];

                Quest quest = new Quest(id, category, num, (QuestType)type, goalData, name, reward, dataBit);
                quests.Add(id, quest);
                AddToCounter(category);
            }
        }

        private void AddToCounter(string category)
        {
            int count = 0;
            if (questCount.TryGetValue(category, out count))
            {
                questCount[category] = count + 1;
            }
            else
            {
                questCount.Add(category, 1);
            }
        }

        internal Quest GetQuest(uint Id)
        {
            Quest quest = null;
            quests.TryGetValue(Id, out quest);
            return quest;
        }

        internal int GetAmountOfQuestsInCategory(string Category)
        {
            int count = 0;
            questCount.TryGetValue(Category, out count);
            return count;
        }

        internal void ProgressUserQuest(GameClient Session, QuestType QuestType, uint EventData = 0)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().CurrentQuestId <= 0)
            {
                return;
            }

            Quest UserQuest = GetQuest(Session.GetHabbo().CurrentQuestId);

            if (UserQuest == null || UserQuest.GoalType != QuestType)
            {
                return;
            }

            int CurrentProgress = Session.GetHabbo().GetQuestProgress(UserQuest.Id);
            int NewProgress = CurrentProgress;
            bool PassQuest = false;

            switch (QuestType)
            {
                default:

                    NewProgress++;

                    if (NewProgress >= UserQuest.GoalData)
                    {
                        PassQuest = true;
                    }

                    break;

                case QuestType.EXPLORE_FIND_ITEM:

                    if (EventData != UserQuest.GoalData)
                        return;

                    NewProgress = (int)UserQuest.GoalData;
                    PassQuest = true;
                    break;
            }

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.runFastQuery("UPDATE user_quests SET progress = " + NewProgress +  " WHERE user_id = " + Session.GetHabbo().Id + " AND quest_id =  " + UserQuest.Id);

                if (PassQuest)
                {
                    dbClient.runFastQuery("UPDATE users SET currentquestid = 0 WHERE id = " + Session.GetHabbo().Id);
                    //Quest NextQuest = GetNextQuestInSeries(UserQuest.Category, UserQuest.Number + 1);

                    //if (NextQuest != null)
                    //    dbClient.runFastQuery("INSERT INTO user_quests (user_id, quest_id, progress) VALUES (" + Session.GetHabbo().Id + ", " + NextQuest.Id + ", 0)");
                }
            }

            Session.GetHabbo().quests[Session.GetHabbo().CurrentQuestId] = NewProgress;
            Session.SendMessage(QuestStartedComposer.Compose(Session, UserQuest));

            if (PassQuest)
            {
                Session.GetHabbo().CurrentQuestId = 0;
                Session.GetHabbo().LastCompleted = UserQuest.Id;
                Session.SendMessage(QuestCompletedComposer.Compose(Session, UserQuest));
                Session.GetHabbo().ActivityPoints += UserQuest.Reward;
                Session.GetHabbo().UpdateActivityPointsBalance(false);
                GetList(Session, null);
            }
        }

        internal Quest GetNextQuestInSeries(string Category, int Number)
        {
                foreach (Quest Quest in quests.Values)
                {
                    if (Quest.Category == Category && Quest.Number == Number)
                    {
                        return Quest;
                    }
                }

            return null;
        }

        internal void GetList(GameClient Session, ClientMessage Message)
        {
            Session.SendMessage(QuestListComposer.Compose(Session, quests.Values.ToList(), (Message != null)));
        }

        internal void ActivateQuest(GameClient Session, ClientMessage Message)
        {
            Quest Quest = GetQuest(Message.PopWiredUInt());

            if (Quest == null)
            {
                return;
            }

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (dbClient.dbType == Database_Manager.Database.DatabaseType.MySQL)
                    dbClient.runFastQuery("REPLACE INTO user_quests VALUES (" + Session.GetHabbo().Id + ", " + Quest.Id + ", 0)");
                else
                    dbClient.runFastQuery("IF NOT EXISTS (SELECT user_id FROM user_quests WHERE user_id = " + Session.GetHabbo().Id + " AND quest_id = " + Quest.Id + ") " + 
	                                      "INSERT INTO user_quests VALUES (" + Session.GetHabbo().Id + ", " + Quest.Id + ", 0)");
                dbClient.runFastQuery("UPDATE users SET currentquestid = " + Quest.Id + " WHERE id = " + Session.GetHabbo().Id);
            }

            Session.GetHabbo().CurrentQuestId = Quest.Id;
            GetList(Session, null);
            Session.SendMessage(QuestStartedComposer.Compose(Session, Quest));
        }

        internal void GetCurrentQuest(GameClient Session, ClientMessage Message)
        {
            if (!Session.GetHabbo().InRoom)
            {
                return;
            }

            Quest UserQuest = GetQuest(Session.GetHabbo().LastCompleted);
            Quest NextQuest = GetNextQuestInSeries(UserQuest.Category, UserQuest.Number + 1);

            if (NextQuest == null)
            {
                return;
            }



            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (dbClient.dbType == Database_Manager.Database.DatabaseType.MySQL)
                    dbClient.runFastQuery("REPLACE INTO user_quests VALUES (" + Session.GetHabbo().Id + ", " + NextQuest.Id + ", 0)");
                else
                    dbClient.runFastQuery("IF NOT EXISTS (SELECT user_id FROM user_quests WHERE user_id = " + Session.GetHabbo().Id + " AND quest_id = " + NextQuest.Id + ") " +
                                          "INSERT INTO user_quests VALUES (" + Session.GetHabbo().Id + ", " + NextQuest.Id + ", 0)");
                dbClient.runFastQuery("UPDATE users SET currentquestid = " + NextQuest.Id + " WHERE id = " + Session.GetHabbo().Id);
            }

            Session.GetHabbo().CurrentQuestId = NextQuest.Id;
            GetList(Session, null);
            Session.SendMessage(QuestStartedComposer.Compose(Session, NextQuest));


            //Session.SendMessage(QuestStartedComposer.Compose(Session, NextQuest));
        }

        internal void CancelQuest(GameClient Session, ClientMessage Message)
        {
            Quest Quest = GetQuest(Session.GetHabbo().CurrentQuestId);

            if (Quest == null)
            {
                return;
            }

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.runFastQuery("DELETE FROM user_quests WHERE user_id = " + Session.GetHabbo().Id + " AND quest_id = " + Quest.Id);
            }

            Session.SendMessage(QuestAbortedComposer.Compose());
            GetList(Session, null);
        }

        //internal List<Quest> quests;

        //public QuestManager()
        //{
        //    this.quests = new List<Quest>();
        //}

        //internal void InitQuests()
        //{
        //    this.quests = new List<Quest>();

        //    DataTable dTable;
        //    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
        //    {
        //        dbClient.setQuery("SELECT id,type,action,needofcount,level_num,pixel_reward FROM quests WHERE enabled = '1' ORDER by level_num");
        //        dTable = dbClient.getTable();
        //    }

        //    uint id;
        //    string type;
        //    string action;
        //    int needofcount;
        //    int levelnum;
        //    int pixelreward;
        //    foreach (DataRow dRow in dTable.Rows)
        //    {
        //        id = (uint)dRow["id"];
        //        type = (string)dRow["type"];
        //        action = (string)dRow["action"];
        //        needofcount = (int)dRow["needofcount"];
        //        levelnum = (int)dRow["level_num"];
        //        pixelreward = (int)dRow["pixel_reward"];

        //        Quest quets = new Quest(id, type, action, needofcount, levelnum, pixelreward);
        //        quests.Add(quets);
        //    }

        //    Logging.WriteLine("Quest manager -> READY!");
        //}

        //internal void UpdateQuest(uint Id, GameClient Session)
        //{
        //    Session.GetHabbo().CurrentQuestProgress++;

        //    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
        //    {
        //        dbClient.runFastQuery("UPDATE users SET quest_progress = quest_progress + 1 WHERE id = '" + Session.GetHabbo().Id + "' LIMIT 1");
        //    }

        //    ButterflyEnvironment.GetGame().GetQuestManager().HandleQuest(Id, Session);
        //}

        //internal int GetHighestLevelForType(string Type)
        //{
        //    int i = 0;

        //    foreach (Quest Q in quests)
        //    {
        //        if (Q.Type == Type)
        //        {
        //            i++;
        //        }
        //    }

        //    return i;
        //}

        //internal uint GetQuestIdBy1More(int LevelType, string Type)
        //{
        //    foreach (Quest Q in quests)
        //    {
        //        if (Q.Type == Type)
        //        {
        //            if (Q.Level == LevelType + 1)
        //            {
        //                return Q.QuestId();
        //            }
        //        }
        //    }

        //    return 0;
        //}

        //public void ActivateNextQuest(GameClient Session)
        //{
        //    Quest Quest = GetQuest(Session.GetHabbo().LastQuestId); if (Quest == null) { return; }

        //    string Type = Quest.Type.ToLower();
        //    int Level = 0;

        //    switch (Type)
        //    {
        //        case "social":
        //            Level = Session.GetHabbo().LevelSocial;
        //            break;
        //        case "room_builder":
        //            Level = Session.GetHabbo().LevelBuilder;
        //            break;
        //        case "identity":
        //            Level = Session.GetHabbo().LevelIdentity;
        //            break;
        //        case "explore":
        //            Level = Session.GetHabbo().LevelExplorer;
        //            break;
        //    }

        //    if (GetQuestIdBy1More(Level, Type) != 0)
        //    {
        //        this.HandleQuest(GetQuestIdBy1More(Level, Type), Session);
        //    }
        //}

        //internal ServerMessage SerializeQuestList(GameClient Session)
        //{
        //    ServerMessage Message = new ServerMessage(800);
        //    Message.AppendInt32(4); // explore, social, identity, room_builder() 4

        //    ParseNeed(Session, Message);

        //    Message.AppendInt32(1); // end line

        //    return Message;
        //}

        //internal Quest GetQuest(uint Id)
        //{
        //    foreach (Quest Quest in quests)
        //    {
        //        if (Quest.QuestId() == Id)
        //        {
        //            return Quest;
        //        }
        //    }

        //    return null;
        //}

        //internal void HandleQuest(uint Id, GameClient Session)
        //{
        //    if (Session == null)
        //    {
        //        return;
        //    }

        //    if (Session.GetHabbo().CurrentQuestId != Id)
        //    {
        //        Session.GetHabbo().CurrentQuestId = Id;
        //        Session.GetHabbo().CurrentQuestProgress = 0;

        //        using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
        //        {
        //            dbClient.runFastQuery("UPDATE users SET quest_id = " + Id + ", quest_progress = '0' WHERE id = " + Session.GetHabbo().Id + " LIMIT 1");
        //        }
        //    }

        //    if (Id == 0)
        //    {
        //        Session.SendMessage(SerializeQuestList(Session));

        //        ServerMessage Message = new ServerMessage(803);
        //        Session.SendMessage(Message);
        //    }
        //    else
        //    {
        //        Quest Quest = GetQuest(Id);

        //        if (Quest == null)
        //        {
        //            return;
        //        }

        //        if (Quest.NeedForLevel <= Session.GetHabbo().CurrentQuestProgress)
        //        {
        //            CompleteQuest(Id, Session);
        //            return;
        //        }
        //        else
        //        {
        //            ServerMessage Message = new ServerMessage(802);

        //            Quest.Serialize(Message, Session, true);

        //            Session.SendMessage(Message);
        //        }
        //    }
        //}

        //internal void CompleteQuest(uint Id, GameClient Session)
        //{
        //    Session.GetHabbo().CurrentQuestId = 0;
        //    Session.GetHabbo().LastQuestId = Id;

        //    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
        //    {
        //        dbClient.runFastQuery("UPDATE users SET quest_id = '0',quest_progress = '0', lev_" + GetQuest(Id).Type.Replace("room_", "") + " = lev_" + GetQuest(Id).Type.Replace("room_", "") + " + 1 WHERE id = " + Session.GetHabbo().Id + " LIMIT 1");
        //        dbClient.runFastQuery("INSERT INTO user_quests (user_id,quest_id) VALUES (" + Session.GetHabbo().Id + "," + Id + ")");
        //    }

        //    switch (GetQuest(Id).Type.ToLower())
        //    {
        //        case "identity":
        //            Session.GetHabbo().LevelIdentity++;
        //            break;
        //        case "room_builder":
        //            Session.GetHabbo().LevelBuilder++;
        //            break;
        //        case "social":
        //            Session.GetHabbo().LevelSocial++;
        //            break;
        //        case "explore":
        //            Session.GetHabbo().LevelExplorer++;
        //            break;
        //    }

        //    int Amount = 50;
        //    Session.GetHabbo().ActivityPoints += Amount;
        //    Session.GetHabbo().UpdateActivityPointsBalance(Amount);

        //    ServerMessage Message = new ServerMessage(801);

        //    Quest Quest = GetQuest(Id);

        //    if (Quest == null)
        //    {
        //        return;
        //    }

        //    Quest.Serialize(Message, Session, true);
        //    ParseNeed(Session, Message);
        //    Message.AppendInt32(1);
        //    Session.SendMessage(Message);

        //    Session.GetHabbo().CurrentQuestProgress = 0;
        //}

        //internal void ParseNeed(GameClient Session, ServerMessage Message)
        //{
        //    bool DidSocial = false;
        //    bool DidBuilder = false;
        //    bool DidId = false;
        //    bool DidExplorer = false;

        //    foreach (Quest Quest in this.quests)
        //    {
        //        if (Quest.QuestId() == Session.GetHabbo().CurrentQuestId)
        //        {
        //            Quest.Serialize(Message, Session, false);

        //            switch (Quest.Type.ToLower())
        //            {
        //                case "social":
        //                    DidSocial = true;
        //                    break;
        //                case "room_builder":
        //                    DidBuilder = true;
        //                    break;
        //                case "identity":
        //                    DidId = true;
        //                    break;
        //                case "explore":
        //                    DidExplorer = true;
        //                    break;
        //            }
        //        }

        //        if (Quest.Type.ToLower() == "room_builder" && !DidBuilder && Quest.Level == GetHighestLevelForType("room_builder") && Session.GetHabbo().LevelBuilder == GetHighestLevelForType("room_builder"))
        //        {
        //            Quest.Serialize(Message, Session, false);
        //            DidBuilder = true;
        //        }

        //        if (Quest.Type.ToLower() == "social" && !DidSocial && Quest.Level == GetHighestLevelForType("social") && Session.GetHabbo().LevelSocial == GetHighestLevelForType("social"))
        //        {
        //            Quest.Serialize(Message, Session, false);
        //            DidSocial = true;
        //        }

        //        if (Quest.Type.ToLower() == "identity" && !DidId && Quest.Level == GetHighestLevelForType("identity") && Session.GetHabbo().LevelIdentity == GetHighestLevelForType("identity"))
        //        {
        //            Quest.Serialize(Message, Session, false);
        //            DidId = true;
        //        }

        //        if (Quest.Type.ToLower() == "explore" && !DidExplorer && Quest.Level == GetHighestLevelForType("explore") && Session.GetHabbo().LevelExplorer == GetHighestLevelForType("explore"))
        //        {
        //            Quest.Serialize(Message, Session, false);
        //            DidExplorer = true;
        //        }

        //        if (Quest.Type.ToLower() == "room_builder" && !DidBuilder && Quest.Level == Session.GetHabbo().LevelBuilder + 1)
        //        {
        //            Quest.Serialize(Message, Session, false);
        //            DidBuilder = true;
        //        }

        //        if (Quest.Type.ToLower() == "social" && !DidSocial && Quest.Level == Session.GetHabbo().LevelSocial + 1)
        //        {
        //            Quest.Serialize(Message, Session, false);
        //            DidSocial = true;
        //        }

        //        if (Quest.Type.ToLower() == "identity" && !DidId && Quest.Level == Session.GetHabbo().LevelIdentity + 1)
        //        {
        //            Quest.Serialize(Message, Session, false);
        //            DidId = true;
        //        }

        //        if (Quest.Type.ToLower() == "explore" && !DidExplorer && Quest.Level == Session.GetHabbo().LevelExplorer + 1)
        //        {
        //            Quest.Serialize(Message, Session, false);
        //            DidExplorer = true;
        //        }
        //    }
        //}
    }
}
