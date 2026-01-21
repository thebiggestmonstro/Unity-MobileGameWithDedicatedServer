using LiteNetLib.Utils;

namespace NetworkShared.Packets.ServerToClient
{
    public class Net_OnPlayAgain : INetPacket
    {
        public PacketType Type => PacketType.OnPlayAgain;

        public void Deserialize(NetDataReader reader)
        {

        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}
