
using LiteNetLib.Utils;

namespace NetworkShared.Packets.ServerToClient
{
    public struct Net_OnStartGame : INetPacket
    {
        public PacketType Type => PacketType.OnStartGame;
        public string XUserName { get; set; }
        public string YUserName { get; set; }
        public Guid GameId { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            XUserName = reader.GetString();
            YUserName = reader.GetString();
            GameId = Guid.Parse(reader.GetString());
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(XUserName);
            writer.Put(YUserName);
            writer.Put(GameId.ToString());
        }
    }
}
