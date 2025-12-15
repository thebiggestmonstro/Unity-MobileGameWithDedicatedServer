using LiteNetLib.Utils;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Server.NetworkShared.Packets.ServerToClient
{
    public struct Net_OnAuthFailed : INetPacket
    {
        public PacketType Type => PacketType.OnAuthFailed;

        public void Deserialize(NetDataReader reader)
        {
            throw new NotImplementedException();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}
