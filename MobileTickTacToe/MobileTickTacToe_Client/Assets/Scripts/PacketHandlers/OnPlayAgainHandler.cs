using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ServerToClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Client.PacketHandlers
{
    [HandlerRegister(PacketType.OnPlayAgain)]
    public class OnPlayAgainHandler : IPacketHandler
    {
        public static event Action OnPlayAgain;

        public void Handle(INetPacket packet, int connectionId)
        {
            OnPlayAgain?.Invoke();  
        }
    }
}
