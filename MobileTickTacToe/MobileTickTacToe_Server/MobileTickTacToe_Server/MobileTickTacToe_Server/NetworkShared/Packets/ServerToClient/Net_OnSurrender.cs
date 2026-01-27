using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkShared.Packets.ServerToClient
{
    public struct Net_OnSurrender : INetPacket
    {
        public PacketType Type => PacketType.OnSurrender;

        public string WinnerName { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            WinnerName = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(WinnerName);
        }
    }
}
