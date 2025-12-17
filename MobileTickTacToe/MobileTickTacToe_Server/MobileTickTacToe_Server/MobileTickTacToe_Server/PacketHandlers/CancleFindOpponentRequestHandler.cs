using NetworkShared;
using NetworkShared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Server.PacketHandlers
{
    [HandlerRegister(PacketType.CancleFindOpponentRequest)]
    public class CancleFindOpponentRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            Console.WriteLine("Received CancleFindOpponent Packet");
        }
    }
}
