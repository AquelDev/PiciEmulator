using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pici.HabboHotel.Wired
{
    [Flags]
    public enum WireCurrentTransfer
    {
        NONE = 0,
        UP = 1,
        RIGHT = 2,
        DOWN = 4,
        LEFT = 8

    }
}
