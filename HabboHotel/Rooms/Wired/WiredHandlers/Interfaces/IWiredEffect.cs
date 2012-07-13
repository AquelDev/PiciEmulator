using Pici.HabboHotel.Items;
using Pici.HabboHotel.Rooms.Games;

namespace Pici.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces
{
    interface IWiredEffect
    {
        bool Handle(RoomUser user, Team team, RoomItem item);
        bool IsSpecial(out SpecialEffects action);
    }
}
