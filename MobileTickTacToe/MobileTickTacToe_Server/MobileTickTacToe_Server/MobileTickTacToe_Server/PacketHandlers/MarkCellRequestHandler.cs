using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ClientToServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Server.PacketHandlers
{
    [HandlerRegister(PacketType.MarkCellRequst)]
    public class MarkCellRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_MarkCellRequest)packet;

            // TODO
            // 1) Validate the request
            // 2) Get current game and invoke game.MarkCell(), got outcome
            // 3) Do each action with following outcome
            // -- None : Switch Current Player
            // -- Win : Increase Player Score and add a win
            // -- Draw : Do Nothing
        }
    }
}
