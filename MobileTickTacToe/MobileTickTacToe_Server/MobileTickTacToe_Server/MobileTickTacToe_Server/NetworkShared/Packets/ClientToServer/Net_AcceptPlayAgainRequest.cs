using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkShared.Packets.ClientToServer
{
    public struct Net_AcceptPlayAgainRequest : INetPacket
    {
        public PacketType Type => PacketType.AcceptPlayAgainRequest;

        public void Deserialize(NetDataReader reader)
        {

        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}
