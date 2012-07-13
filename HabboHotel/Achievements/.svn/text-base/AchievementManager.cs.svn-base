using System.Collections.Generic;
using System.Linq;
using Butterfly.HabboHotel.Achievements.Composer;
using Butterfly.HabboHotel.GameClients;
using Butterfly.Messages;
using Database_Manager.Database.Session_Details.Interfaces;
using System;

namespace Butterfly.HabboHotel.Achievements
{
    public class AchievementManager
    {
        internal Dictionary<string, Achievement> Achievements;

        internal AchievementManager(IQueryAdapter dbClient)
        {
            this.Achievements = new Dictionary<string, Achievement>();
            LoadAchievements(dbClient);
        }

        internal void LoadAchievements(IQueryAdapter dbClient)
        {
            AchievementLevelFactory.GetAchievementLevels(out Achievements, dbClient);
        }

        internal void GetList(GameClient Session, ClientMessage Message)
        {
            Session.SendMessage(AchievementListComposer.Compose(Session, Achievements.Values.ToList()));
        }

        internal bool ProgressUserAchievement(GameClient Session, string AchievementGroup, int ProgressAmount)
        {
            if (!Achievements.ContainsKey(AchievementGroup))
            {
                return false;
            }

            Achievement AchievementData = null;

            AchievementData = Achievements[AchievementGroup];

            UserAchievement UserData = Session.GetHabbo().GetAchievementData(AchievementGroup);

            if (UserData == null)
            {
                UserData = new UserAchievement(AchievementGroup, 0, 0);
                Session.GetHabbo().Achievements.Add(AchievementGroup, UserData);
            }

            int TotalLevels = AchievementData.Levels.Count;

            if (UserData != null && UserData.Level == TotalLevels)
            {
                return false; // done, no more.
            }

            int TargetLevel = (UserData != null ? UserData.Level + 1 : 1);

            if (TargetLevel > TotalLevels)
            {
                TargetLevel = TotalLevels;
            }

            AchievementLevel TargetLevelData = AchievementData.Levels[TargetLevel];

            int NewProgress = (UserData != null ? UserData.Progress + ProgressAmount : ProgressAmount);
            int NewLevel = (UserData != null ? UserData.Level : 0);
            int NewTarget = NewLevel + 1;

            if (NewTarget > TotalLevels)
            {
                NewTarget = TotalLevels;
            }

            if (NewProgress >= TargetLevelData.Requirement)
            {
                NewLevel++;
                NewTarget++;

                int ProgressRemainder = NewProgress - TargetLevelData.Requirement;
                NewProgress = 0;

                Session.GetHabbo().GetBadgeComponent().GiveBadge(AchievementGroup + TargetLevel, true);

                if (NewTarget > TotalLevels)
                {
                    NewTarget = TotalLevels;
                }

                Session.GetHabbo().ActivityPoints += TargetLevelData.RewardPixels;
                Session.GetHabbo().UpdateActivityPointsBalance(false);

                Session.SendMessage(AchievementUnlockedComposer.Compose(AchievementData, TargetLevel, TargetLevelData.RewardPoints,
                    TargetLevelData.RewardPixels));

                using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    if (dbClient.dbType == Database_Manager.Database.DatabaseType.MySQL)
                        dbClient.setQuery("REPLACE INTO user_achievement VALUES (" + Session.GetHabbo().Id + ", @group, " + NewLevel + ", " + NewProgress + ")");
                    else
                    {
                        dbClient.setQuery("IF EXISTS (SELECT userid FROM user_achievement WHERE userid = " + Session.GetHabbo().Id + " AND group = @group) " +
                                        "	UPDATE user_achievement SET level = " + NewLevel + ", progress = " + NewProgress + " WHERE userid = " + Session.GetHabbo().Id + " AND group = @group " +
                                        "ELSE" +
                                        "	INSERT INTO user_achievement VALUES (" + Session.GetHabbo().Id + ",@group," + NewLevel + "," + NewProgress + ")");
                    }
                    dbClient.addParameter("group", AchievementGroup);
                    dbClient.runQuery();
                }


                UserData.Level = NewLevel;
                UserData.Progress = NewProgress;

                Session.GetHabbo().AchievementPoints += TargetLevelData.RewardPoints;
                Session.GetHabbo().ActivityPoints += TargetLevelData.RewardPixels;
                Session.GetHabbo().UpdateActivityPointsBalance(false);
                Session.SendMessage(AchievementScoreUpdateComposer.Compose(Session.GetHabbo().AchievementPoints));

                
                AchievementLevel NewLevelData = AchievementData.Levels[NewTarget];
                Session.SendMessage(AchievementProgressComposer.Compose(AchievementData, NewTarget, NewLevelData,
                    TotalLevels, Session.GetHabbo().GetAchievementData(AchievementGroup)));

                return true;
            }
            else
            {
                UserData.Level = NewLevel;
                UserData.Progress = NewProgress;
                using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    if (dbClient.dbType == Database_Manager.Database.DatabaseType.MySQL)
                        dbClient.setQuery("REPLACE INTO user_achievement VALUES (" + Session.GetHabbo().Id + ", @group, " + NewLevel + ", " + NewProgress + ")");
                    else
                    {
                        dbClient.setQuery("IF EXISTS (SELECT userid FROM user_achievement WHERE userid = " + Session.GetHabbo().Id + " AND group = @group) " +
                                        "	UPDATE user_achievement SET level = " + NewLevel + ", progress = " + NewProgress + " WHERE userid = " + Session.GetHabbo().Id + " AND group = @group " +
                                        "ELSE" +
                                        "	INSERT INTO user_achievement VALUES (" + Session.GetHabbo().Id + ",@group," + NewLevel + "," + NewProgress + ")");
                    }
                    dbClient.addParameter("group", AchievementGroup);
                    dbClient.runQuery();
                }

                Session.SendMessage(AchievementProgressComposer.Compose(AchievementData, TargetLevel, TargetLevelData,
                TotalLevels, Session.GetHabbo().GetAchievementData(AchievementGroup)));
            }

            return false;
        }

        internal Achievement GetAchievement(string AchievementGroup)
        {
            if (Achievements.ContainsKey(AchievementGroup))
            {
                return Achievements[AchievementGroup];
            }

            return null;
        }
    }
    //class AchievementManager
    //{
    //    private Dictionary<uint, Achievement> Achievements;

    //    internal AchievementManager()
    //    {
    //        this.Achievements = new Dictionary<uint, Achievement>();
    //    }

    //    internal void LoadAchievements(IQueryAdapter dbClient)
    //    {
    //        Achievements.Clear();
    //        DataTable Data = new DataTable();

    //        dbClient.setQuery("SELECT * FROM achievements");
    //        Data = dbClient.getTable();

    //        if (Data == null)
    //        {
    //            return;
    //        }

    //        foreach (DataRow Row in Data.Rows)
    //        {
    //            Achievements.Add((uint)Row["id"], new Achievement((uint)Row["id"], (int)Row["levels"], (string)Row["badge"], (int)Row["pixels_base"], (double)Row["pixels_multiplier"], ButterflyEnvironment.EnumToBool(Row["dynamic_badgelevel"].ToString())));
    //        }
    //    }

    //    internal Boolean UserHasAchievement(GameClient Session, uint Id, int MinLevel)
    //    {
    //        if (!Session.GetHabbo().Achievements.ContainsKey(Id))
    //        {
    //            return false;
    //        }

    //        if (Session.GetHabbo().Achievements[Id] >= MinLevel)
    //        {
    //            return true;
    //        }

    //        return false;
    //    }

    //    public ServerMessage SerializeAchievementList(GameClient Session)
    //    {
    //        var AchievementsToList = new List<Achievement>();

    //        var NextAchievementLevels = new Dictionary<uint, int>();

    //        foreach (Achievement Achievement in Achievements.Values)
    //        {
    //            if (!Session.GetHabbo().Achievements.ContainsKey(Achievement.Id))
    //            {
    //                AchievementsToList.Add(Achievement);
    //                NextAchievementLevels.Add(Achievement.Id, 1);
    //            }
    //            else
    //            {
    //                if (Session.GetHabbo().Achievements[Achievement.Id] >= Achievement.Levels)
    //                {
    //                    continue;
    //                }

    //                AchievementsToList.Add(Achievement);
    //                NextAchievementLevels.Add(Achievement.Id, Session.GetHabbo().Achievements[Achievement.Id] + 1);
    //            }
    //        }

    //        ServerMessage Message = new ServerMessage(436);
    //        Message.AppendInt32(AchievementsToList.Count);

    //        foreach (Achievement Achievement in AchievementsToList)
    //        {
    //            int Level = NextAchievementLevels[Achievement.Id];
    //            int Pixels = CalculateAchievementValue(Achievement.PixelBase, Achievement.PixelMultiplier, Level);

    //            Achievement.Serialize(Message, Pixels, Level, FormatBadgeCode(Achievement.BadgeCode, Level, Achievement.DynamicBadgeLevel));
    //        }

    //        return Message;
    //    }

    //    private int CalculateAchievementValue(int BaseValue, Double Multiplier, int Level)
    //    {
    //        return (BaseValue + (50 * Level));
    //    }

    //    internal void UnlockAchievement(GameClient Session, uint AchievementId, int Level)
    //    {
    //        // Get the achievement
    //        Achievement Achievement = Achievements[AchievementId];

    //        // Make sure the achievement is valid and has not already been unlocked
    //        if (Achievement == null || UserHasAchievement(Session, Achievement.Id, Level) || Level < 1 || Level > Achievement.Levels)
    //        {
    //            return;
    //        }

    //        // Calculate the pixel value for this achievement
    //        int Value = CalculateAchievementValue(Achievement.PixelBase, Level);

    //        Session.GetHabbo().GetBadgeComponent().GiveBadge(FormatBadgeCode(Achievement.BadgeCode, Level, Achievement.DynamicBadgeLevel), true);

    //        // Update or set the achievement level for the user
    //        if (Session.GetHabbo().Achievements.ContainsKey(Achievement.Id))
    //        {
    //            Session.GetHabbo().Achievements[Achievement.Id] = Level;

    //            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
    //            {
    //                dbClient.runFastQuery("UPDATE user_achievements SET achievement_level = " + Level + " WHERE user_id = " + Session.GetHabbo().Id + " AND achievement_id = " + Achievement.Id + " LIMIT 1");
    //            }
    //        }
    //        else
    //        {
    //            Session.GetHabbo().Achievements.Add(Achievement.Id, Level);

    //            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
    //            {
    //                dbClient.runFastQuery("INSERT INTO user_achievements (user_id,achievement_id,achievement_level) VALUES (" + Session.GetHabbo().Id + "," + Achievement.Id + "," + Level + ")");
    //            }
    //        }

    //        // Notify the user of the achievement gain
    //        Session.GetMessageHandler().GetResponse().Init(437);
    //        Session.GetMessageHandler().GetResponse().AppendUInt(Achievement.Id);
    //        Session.GetMessageHandler().GetResponse().AppendInt32(Level);
    //        Session.GetMessageHandler().GetResponse().AppendStringWithBreak(FormatBadgeCode(Achievement.BadgeCode, Level, Achievement.DynamicBadgeLevel));

    //        if (Level > 1)
    //        {
    //            Session.GetMessageHandler().GetResponse().AppendStringWithBreak(FormatBadgeCode(Achievement.BadgeCode, (Level - 1), Achievement.DynamicBadgeLevel));
    //        }
    //        else
    //        {
    //            Session.GetMessageHandler().GetResponse().AppendStringWithBreak("");
    //        }

    //        Session.GetMessageHandler().SendResponse();

    //        Session.GetHabbo().ActivityPoints += Value;
    //        Session.GetHabbo().UpdateActivityPointsBalance(Value);
    //    }

    //    internal int CalculateAchievementValue(int BaseValue, int Level)
    //    {
    //        return (BaseValue + (50 * Level));
    //    }

    //    internal string FormatBadgeCode(string BadgeTemplate, int Level, bool Dyn)
    //    {
    //        if (!Dyn)
    //        {
    //            return BadgeTemplate;
    //        }

    //        return BadgeTemplate + Level;
    //    }

    //    internal Achievement GetAchievement(uint id)
    //    {
    //        Achievement achievement;

    //        if (Achievements.TryGetValue(id, out achievement))
    //            return achievement;
    //        return null;
    //    }
    //}
}
