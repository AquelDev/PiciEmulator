using System.Collections;
using System.Collections.Generic;
using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Pets;
using Butterfly.HabboHotel.Rooms;
using Butterfly.HabboHotel.Users.Badges;
using Butterfly.HabboHotel.Users.Inventory;
using Butterfly.HabboHotel.Users.Subscriptions;
using Butterfly.HabboHotel.Users;
using Butterfly.HabboHotel.Users.Messenger;
using Butterfly.HabboHotel.Achievements;

namespace Butterfly.HabboHotel.Users.UserDataManagement
{
    class UserData
    {
        internal uint userID;

        internal Dictionary<string, UserAchievement> achievements;
        internal List<uint> favouritedRooms;
        internal List<uint> ignores;
        internal List<string> tags;
        internal Dictionary<string, Subscription> subscriptions;
        internal List<Badge> badges;
        internal List<UserItem> inventory;
        internal Hashtable inventorySongs;
        internal List<AvatarEffect> effects;
        internal Dictionary<uint, MessengerBuddy> friends;
        internal Dictionary<uint, MessengerRequest> requests;
        internal List<RoomData> rooms;
        internal Dictionary<uint, Pet> pets;
        internal Dictionary<uint, int> quests;
        internal Habbo user;

        public UserData(uint userID, Dictionary<string, UserAchievement> achievements, List<uint> favouritedRooms, List<uint> ignores, List<string> tags, 
            Dictionary<string, Subscription> subscriptions, List<Badge> badges, List<UserItem> inventory, List<AvatarEffect> effects,
            Dictionary<uint, MessengerBuddy> friends, Dictionary<uint, MessengerRequest> requests, List<RoomData> rooms, Dictionary<uint, Pet> pets, Dictionary<uint, int> quests, Hashtable inventorySongs, Habbo user)
        {
            this.userID = userID;
            this.achievements = achievements;
            this.favouritedRooms = favouritedRooms;
            this.ignores = ignores;
            this.tags = tags;
            this.subscriptions = subscriptions;
            this.badges = badges;
            this.inventory = inventory;
            this.effects = effects;
            this.friends = friends;
            this.requests = requests;
            this.rooms = rooms;
            this.pets = pets;
            this.quests = quests;
            this.inventorySongs = inventorySongs;
            this.user = user;
        }
    }
}
