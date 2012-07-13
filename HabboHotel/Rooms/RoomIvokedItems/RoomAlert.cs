using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pici.HabboHotel.Rooms.RoomIvokedItems
{
    class RoomAlert
    {
        internal string message;
        internal int minrank;

        public RoomAlert(string message, int minrank)
        {
            this.message = message;
            this.minrank = minrank;
        }
    }
}
