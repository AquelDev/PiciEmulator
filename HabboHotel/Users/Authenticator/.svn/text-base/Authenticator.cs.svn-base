using System;
using System.Data;
using Butterfly.HabboHotel.GameClients;

namespace Butterfly.HabboHotel.Users.Authenticator
{
    static class HabboFactory
    {
        internal static Habbo GenerateHabbo(DataRow dRow, DataRow group)
        {
            uint id = Convert.ToUInt32(dRow["id"]);
            string username = (string)dRow["username"];
            string realname = (string)dRow["real_name"];
            uint rank = Convert.ToUInt32(dRow["rank"]);
            string motto = (string)dRow["motto"];
            string look = (string)dRow["look"];
            string gender = (string)dRow["gender"];
            int credits = (int)dRow["credits"];
            int activityPoints = (int)dRow["activity_points"];
            double activityPointsLastUpdate = Convert.ToDouble(dRow["activity_points_lastupdate"]);
            bool isMuted = ButterflyEnvironment.EnumToBool(dRow["is_muted"].ToString());
            uint homeRoom = Convert.ToUInt32(dRow["home_room"]);
            int respect = (Int32)dRow["respect"];
            int dailyRespect = (int)dRow["daily_respect_points"];
            int dailyPetRespect = (int)dRow["daily_pet_respect_points"];
            bool mtantPenalty = (dRow["mutant_penalty"].ToString() != "0");
            bool blockFriends = ButterflyEnvironment.EnumToBool(dRow["block_newfriends"].ToString());
            uint questID = Convert.ToUInt32(dRow["currentquestid"]);
            int questProgress = (int)dRow["currentquestprogress"];
            int achiecvementPoints = (int)dRow["achievement_points"];

            return new Habbo(id, username, realname, rank, motto, look, gender, credits, activityPoints, activityPointsLastUpdate, isMuted, homeRoom, respect, dailyRespect, dailyPetRespect, mtantPenalty, blockFriends, questID, questProgress, group, achiecvementPoints);
        }
    }
}
