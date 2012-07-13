using System.Collections.Generic;
using System.Data;
using Database_Manager.Database.Session_Details.Interfaces;
using System;

namespace Butterfly.HabboHotel.Achievements
{
    class AchievementLevelFactory
    {
        internal static void GetAchievementLevels(out Dictionary<string, Achievement> achievements, IQueryAdapter dbClient)
        {
            achievements = new Dictionary<string, Achievement>();

            dbClient.setQuery("SELECT * FROM achievements");
            DataTable dTable = dbClient.getTable();

            uint id;
            string category;
            string groupName;
            int level;
            int rewardPixels;
            int rewardPoints;
            int progressNeeded;
            foreach (DataRow dRow in dTable.Rows)
            {
                id = Convert.ToUInt32(dRow["id"]);
                category = (string)dRow["category"];
                groupName = (string)dRow["group_name"];
                level = (int)dRow["level"];
                rewardPixels = (int)dRow["reward_pixels"];
                rewardPoints = (int)dRow["reward_points"];
                progressNeeded = (int)dRow["progress_needed"];

                AchievementLevel achievementLevel = new AchievementLevel(level, rewardPixels, rewardPoints, progressNeeded);

                if (!achievements.ContainsKey(groupName))
                {
                    Achievement achievement = new Achievement(id, groupName, category);
                    achievement.AddLevel(achievementLevel);

                    achievements.Add(groupName, achievement);
                }
                else
                {
                    achievements[groupName].AddLevel(achievementLevel);
                }
            }
        }
    }
}
