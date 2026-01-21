using LiteNetLib.Utils;

namespace NetworkShared.Packets.ClientToServer
{
    public class Net_PlayAgainRequest : INetPacket
    {
        public PacketType Type => PacketType.PlayAgainRequest;

        

        public void Deserialize(NetDataReader reader)
        {
           
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}
