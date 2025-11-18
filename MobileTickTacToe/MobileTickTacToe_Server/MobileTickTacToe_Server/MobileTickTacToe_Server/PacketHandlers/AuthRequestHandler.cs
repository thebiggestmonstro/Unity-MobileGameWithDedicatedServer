using MobileTickTacToe_Server.Handlers;
using NetworkShared;
using NetworkShared.Attributes;

namespace MobileTickTacToe_Server.PacketHandlers
{
    [HandlerRegister(PacketType.AuthRequest)]
    public class AuthRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            throw new NotImplementedException();
        }
    }
}
