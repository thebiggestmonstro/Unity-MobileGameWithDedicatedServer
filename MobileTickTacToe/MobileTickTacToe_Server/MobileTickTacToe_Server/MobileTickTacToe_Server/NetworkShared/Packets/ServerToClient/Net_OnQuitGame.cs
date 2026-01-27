using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkShared.Packets.ServerToClient
{
    public struct Net_OnQuitGame : INetPacket
    {
        public PacketType Type => PacketType.OnQuitGame;

        public string QuitterName { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            QuitterName = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(QuitterName);
        }
    }
}
