using System;
using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Rooms.Games;

namespace Butterfly.HabboHotel.Rooms
{
    public class UserSaysArgs : EventArgs
    {
        internal readonly RoomUser user;
        internal readonly string message;

        public UserSaysArgs(RoomUser user, string message)
        {
            this.user = user;
            this.message = message;
        }
    }

    public class ItemTriggeredArgs : EventArgs
    {
        internal readonly RoomUser TriggeringUser;
        internal readonly RoomItem TriggeringItem;

        public ItemTriggeredArgs(RoomUser user, RoomItem item)
        {
            this.TriggeringUser = user;
            this.TriggeringItem = item;
        }
    }

    public class TeamScoreChangedArgs : EventArgs
    {
        internal readonly int Points;
        internal readonly Team Team;
        internal readonly RoomUser user;

        public TeamScoreChangedArgs(int points, Team team, RoomUser user)
        {
            this.Points = points;
            this.Team = team;
            this.user = user;
        }
    }

    public class UserWalksOnArgs : EventArgs
    {
        internal readonly RoomUser user;

        public UserWalksOnArgs(RoomUser user)
        {
            this.user = user;
        }
    }
}
