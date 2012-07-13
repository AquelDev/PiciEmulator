using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pici.Messages;

namespace Pici.Messages
{
    struct FusedPacket
    {
        internal readonly ServerMessage content;
        internal readonly string requirements;

        public FusedPacket(ServerMessage content, string requirements)
        {
            this.content = content;
            this.requirements = requirements;
        }
    }
}
