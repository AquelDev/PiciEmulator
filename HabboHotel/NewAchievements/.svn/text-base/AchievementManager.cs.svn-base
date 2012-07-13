using System.Collections.Generic;
using System.Data;
using Butterfly;
using Database_Manager.Database.Session_Details.Interfaces;
using System;

namespace Butterfly.HabboHotel.NewAchievements
{
    class AchievementManager
    {
        private static Dictionary<uint, AchievementBase> achievements;

        internal static void InitAchievementManager(IQueryAdapter dbClient)
        {
            dbClient.setQuery("SELECT achievements.*, achievement_categories.name " +
                                "FROM achievements " +
                                "JOIN achievement_categories " +
                                "ON achievement_categories.id = achievements.category");

            DataTable dTable = dbClient.getTable();

            achievements = new Dictionary<uint, AchievementBase>();

            uint achievementID;
            uint levels;
            uint badgelevel;
            string badge;
            int pixelBase;
            int pixelMMPORG;
            int pixelReward;
            string category;
            foreach (DataRow dRow in dTable.Rows)
            {
                achievementID = Convert.ToUInt32(dRow[0]);
                levels = Convert.ToUInt32(dRow[1]);
                badgelevel = Convert.ToUInt32(dRow[2]);
                badge = (string)dRow[3];
                pixelBase = (int)dRow[4];
                pixelMMPORG = (int)dRow[5];
                pixelReward = (int)dRow[6];
                category = (string)dRow[8];

                AchievementBase achivement = new AchievementBase(achievementID, levels, badgelevel, badge, pixelBase, pixelMMPORG, pixelReward, category);
                achievements.Add(achievementID, achivement);
            }
        }

        internal static void SaveAchievement(Achievement achievement)
        {
            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("UPDATE achievement_info SET level = " + achievement.Level + ", progress = " + achievement.Progress);
                dbClient.runQuery();
            }
        }

        internal static AchievementBase GetAchivement(uint id)
        {
            return achievements[id];
        }
    }
}
