using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Server.Handlers
{
    public interface IPacketHandler
    {
        void Handle(INetPacket packet, int connectionId);
    }
}
