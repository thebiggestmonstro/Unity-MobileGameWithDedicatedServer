using LiteNetLib.Utils;
using NetworkShared;

namespace NetworkShared.Packets.ClientToServer
{
    public struct Net_AuthRequest : INetPacket
    {
        public PacketType Type => PacketType.AuthRequest;

        public string UserName { get; set; }
        public string Password { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            UserName = reader.GetString();
            Password = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(UserName);
            writer.Put(Password);
        }
    }
}
