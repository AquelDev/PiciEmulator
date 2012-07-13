using System;
using System.Collections.Generic;
using System.Data;
using Butterfly.Messages;
using Database_Manager.Database.Session_Details.Interfaces;

namespace Butterfly.HabboHotel.Support
{
    class HelpTool
    {
        internal Dictionary<uint, HelpCategory> Categories;
        internal Dictionary<uint, HelpTopic> Topics;

        internal List<HelpTopic> ImportantTopics;
        internal List<HelpTopic> KnownIssues;

        internal HelpTool()
        {
            Categories = new Dictionary<uint, HelpCategory>();
            Topics = new Dictionary<uint, HelpTopic>();

            ImportantTopics = new List<HelpTopic>();
            KnownIssues = new List<HelpTopic>();
        }

        internal void LoadCategories(IQueryAdapter dbClient)
        {
            Categories.Clear();
            DataTable CategoryData = null;

            dbClient.setQuery("SELECT id, caption FROM help_subjects");
            CategoryData = dbClient.getTable();

            if (CategoryData == null)
            {
                return;
            }

            foreach (DataRow Row in CategoryData.Rows)
            {
                Categories.Add(Convert.ToUInt32(Row["id"]), new HelpCategory(Convert.ToUInt32(Row["id"]), (string)Row["caption"]));
            }
        }

        internal HelpCategory GetCategory(uint CategoryId)
        {
            if (Categories.ContainsKey(CategoryId))
            {
                return Categories[CategoryId];
            }

            return null;
        }

        //internal void ClearCategories()
        //{
        //    Categories.Clear();
        //}

        internal void LoadTopics(IQueryAdapter dbClient)
        {
            Topics.Clear();
            DataTable TopicData = null;

            dbClient.setQuery("SELECT id, title, body, subject, known_issue FROM help_topics");
            TopicData = dbClient.getTable();

            if (TopicData == null)
            {
                return;
            }

            foreach (DataRow Row in TopicData.Rows)
            {
                HelpTopic NewTopic = new HelpTopic(Convert.ToUInt32(Row["id"]), (string)Row["title"], (string)Row["body"], Convert.ToUInt32(Row["subject"]));

                Topics.Add(Convert.ToUInt32(Row["id"]), NewTopic);

                int Importance = int.Parse(Row["known_issue"].ToString());

                if (Importance == 1)
                {
                    KnownIssues.Add(NewTopic);
                }
                else if (Importance == 2)
                {
                    ImportantTopics.Add(NewTopic);
                }
            }
        }

        internal HelpTopic GetTopic(uint TopicId)
        {
            if (Topics.ContainsKey(TopicId))
            {
                return Topics[TopicId];
            }

            return null;            
        }

        //internal void ClearTopics()
        //{
        //    Topics.Clear();

        //    ImportantTopics.Clear();
        //    KnownIssues.Clear();
        //}

        internal int ArticlesInCategory(uint CategoryId)
        {
            int i = 0;

            foreach (HelpTopic Topic in Topics.Values)
            {
                if (Topic.CategoryId == CategoryId)
                {
                    i++;
                }
            }

            return i;
        }

        internal ServerMessage SerializeFrontpage()
        {
            ServerMessage Frontpage = new ServerMessage(518);
            Frontpage.AppendInt32(ImportantTopics.Count);

            foreach (HelpTopic Topic in ImportantTopics)
            {
                Frontpage.AppendUInt(Topic.TopicId);
                Frontpage.AppendStringWithBreak(Topic.Caption);
            }

            Frontpage.AppendInt32(KnownIssues.Count);

            foreach (HelpTopic Topic in KnownIssues)
            {
                Frontpage.AppendUInt(Topic.TopicId);
                Frontpage.AppendStringWithBreak(Topic.Caption);
            }

            return Frontpage;
        }

        internal ServerMessage SerializeIndex()
        {
            ServerMessage Index = new ServerMessage(519);
            Index.AppendInt32(Categories.Count);

            foreach (HelpCategory Category in Categories.Values)
            {
                Index.AppendUInt(Category.CategoryId);
                Index.AppendStringWithBreak(Category.Caption);
                Index.AppendInt32(ArticlesInCategory(Category.CategoryId));
            }

            return Index;
        }

        internal static ServerMessage SerializeTopic(HelpTopic Topic)
        {
            ServerMessage Top = new ServerMessage(520);
            Top.AppendUInt(Topic.TopicId);
            Top.AppendStringWithBreak(Topic.Body);
            return Top;
        }

        internal static ServerMessage SerializeSearchResults(string Query)
        {
            DataTable Results = null;

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (dbClient.dbType == Database_Manager.Database.DatabaseType.MySQL)
                    dbClient.setQuery("SELECT id,title FROM help_topics WHERE title LIKE @query OR body LIKE @query LIMIT 25");
                else
                    dbClient.setQuery("SELECT TOP 25 id,title FROM help_topics WHERE title LIKE @query OR body LIKE @query");
                dbClient.addParameter("query", Query + "%");
                Results = dbClient.getTable();
            }

            // HII[KBCan I pay to be unbanned?

            ServerMessage Search = new ServerMessage(521);

            if (Results == null)
            {
                Search.AppendBoolean(false);
                return Search;
            }

            Search.AppendInt32(Results.Rows.Count);

            foreach (DataRow Row in Results.Rows)
            {
                Search.AppendUInt(Convert.ToUInt32(Row["id"]));
                Search.AppendStringWithBreak((string)Row["title"]);
            }

            return Search;
        }

        internal ServerMessage SerializeCategory(HelpCategory Category)
        {
            ServerMessage Cat = new ServerMessage(522);
            Cat.AppendUInt(Category.CategoryId);
            Cat.AppendStringWithBreak("");
            Cat.AppendInt32(ArticlesInCategory(Category.CategoryId));

            foreach (HelpTopic Topic in Topics.Values)
            {
                if (Topic.CategoryId == Category.CategoryId)
                {
                    Cat.AppendUInt(Topic.TopicId);
                    Cat.AppendStringWithBreak(Topic.Caption);
                }
            }

            return Cat;
        }
    }
}
