using System.Collections.Generic;
using Pici.HabboHotel.GameClients;

namespace Pici.HabboHotel.NewAchievements
{
    class UserAchievementManager
    {
        private Dictionary<uint, Achievement> achivements;
        private GameClient client;

        public UserAchievementManager(GameClient client, Dictionary<uint, Achievement> achievements)
        {
            this.client = client;
            this.achivements = achievements;
        }
    }
}
