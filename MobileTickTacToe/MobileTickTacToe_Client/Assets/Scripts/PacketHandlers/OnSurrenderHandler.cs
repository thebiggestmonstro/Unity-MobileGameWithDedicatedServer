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
    [HandlerRegister(PacketType.OnSurrender)]
    public class OnSurrenderHandler : IPacketHandler
    {
        public static event Action<Net_OnSurrender> OnSurrender;

        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_OnSurrender)packet;
            OnSurrender?.Invoke(msg);
        }
    }
}
