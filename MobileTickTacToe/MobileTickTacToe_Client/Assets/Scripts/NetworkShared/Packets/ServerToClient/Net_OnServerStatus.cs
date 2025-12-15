using LiteNetLib.Utils;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkShared.Packets.ServerToClient
{
    public struct Net_OnServerStatus : INetPacket
    {
        public PacketType Type => PacketType.OnServerStatus;

        public void Deserialize(NetDataReader reader)
        {
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}
