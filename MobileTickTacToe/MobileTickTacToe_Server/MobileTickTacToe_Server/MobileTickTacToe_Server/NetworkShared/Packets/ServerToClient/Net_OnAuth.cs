using LiteNetLib.Utils;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Server.NetworkShared.Packets.ServerToClient
{
    public class Net_OnAuth : INetPacket
    {
        public PacketType Type => PacketType.OnAuth;

        public void Deserialize(NetDataReader reader)
        {
            
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}
