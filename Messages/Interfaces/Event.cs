using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pici.HabboHotel.GameClients;

namespace Pici.Messages.Interfaces
{
    interface Event
    {
        /// <summary>
        /// Handles the incoming packet.
        /// </summary>
        /// <param name="GameClient">Session to use</param>
        /// <param name="ClientMessage">Message builded in packet.</param>
        void Invoke(GameClient Session, ClientMessage Packet);
    }
}
