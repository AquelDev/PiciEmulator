using System;

namespace Pici.HabboHotel.Rooms
{
    public delegate void RoomEventDelegate(object sender, EventArgs e);
    public delegate void RoomUserSaysDelegate(object sender, UserSaysArgs e, out bool messageHandled);
    public delegate void TeamScoreChangedDelegate(object sender, TeamScoreChangedArgs e);
    public delegate void UserWalksFurniDelegate(object sender, UserWalksOnArgs e);

}
