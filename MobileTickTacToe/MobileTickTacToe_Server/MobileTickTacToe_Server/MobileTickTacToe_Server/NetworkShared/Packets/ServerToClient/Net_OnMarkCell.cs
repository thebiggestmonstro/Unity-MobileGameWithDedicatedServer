using LiteNetLib.Utils;
using NetworkShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkShared.Packets.ServerToClient
{
    public struct Net_OnMarkCell : INetPacket
    {
        public PacketType Type => PacketType.OnMarkCell;
        public string PlayerName { get; set; }
        public byte Index { get; set; }
        public MarkOutcome Outcome { get; set; }
        public WinLineType WinLineType { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            PlayerName = reader.GetString();
            Index = reader.GetByte();
            Outcome = (MarkOutcome)reader.GetByte();
            WinLineType = (WinLineType)reader.GetByte();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(PlayerName);
            writer.Put(Index);
            writer.Put((byte)Outcome);
            writer.Put((byte)WinLineType);
        }
    }
}
