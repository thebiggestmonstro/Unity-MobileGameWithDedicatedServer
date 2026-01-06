using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkShared.Packets.ClientToServer
{
    public struct Net_MarkCellRequest : INetPacket
    {
        public PacketType Type => PacketType.MarkCellRequst;
        public byte Index { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            Index = reader.GetByte();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(Index);
        }
    }
}
