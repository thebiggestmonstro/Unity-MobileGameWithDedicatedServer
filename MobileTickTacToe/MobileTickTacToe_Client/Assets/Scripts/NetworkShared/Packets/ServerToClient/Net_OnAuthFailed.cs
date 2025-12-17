using LiteNetLib.Utils;

namespace NetworkShared.Packets.ServerToClient
{
    public struct Net_OnAuthFailed : INetPacket
    {
        public PacketType Type => PacketType.OnAuthFailed;

        public void Deserialize(NetDataReader reader)
        {

        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}
