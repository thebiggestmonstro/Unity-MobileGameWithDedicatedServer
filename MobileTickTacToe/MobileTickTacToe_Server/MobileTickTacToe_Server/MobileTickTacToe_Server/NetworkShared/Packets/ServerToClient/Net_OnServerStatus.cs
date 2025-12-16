using LiteNetLib.Utils;

namespace NetworkShared.Packets.ServerToClient
{
    // DTO(Data Transfer Object) : 데이터를 주고받기 위한 객체
    public struct PlayersNetDto : INetSerializable
    {
        public string UserName { get; set; }
        public ushort Score { get; set; }
        public bool IsOnline { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            UserName = reader.GetString();
            Score = reader.GetUShort();
            IsOnline = reader.GetBool();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(UserName);
            writer.Put(Score);
            writer.Put(IsOnline);
        }
    }

    public struct Net_OnServerStatus : INetPacket
    {
        public ushort PlayersCount { get; set; }

        public PlayersNetDto[] TopPlayers { get; set; }

        public PacketType Type => PacketType.OnServerStatus;

        public void Deserialize(NetDataReader reader)
        {
            PlayersCount = reader.GetUShort();

            var topPlayersLength = reader.GetUShort();
            TopPlayers = new PlayersNetDto[topPlayersLength];
            for (int i = 0; i < topPlayersLength; i++)
            {
                TopPlayers[i] = reader.Get<PlayersNetDto>();
            }
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(PlayersCount);

            writer.Put((ushort)TopPlayers.Length);
            for (int i = 0; i < TopPlayers.Length; i++)
            {
                writer.Put(TopPlayers[i]);
            }
        }
    }
}
