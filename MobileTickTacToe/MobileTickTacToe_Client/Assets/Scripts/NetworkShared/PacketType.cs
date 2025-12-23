using LiteNetLib.Utils;

namespace NetworkShared
{

    public enum PacketType : byte
    {
        #region ClientToServer
        Invalid = 0,
        AuthRequest = 1,
        ServerStatusRequest = 2,
        FindOpponentRequest = 3,
        CancleFindOpponentRequest = 4,
        #endregion

        #region ServerToClient
        OnAuth = 100,
        OnAuthFailed = 101,
        OnServerStatus = 102,
        OnFindOpponent = 103,
        OnStartGame = 104,
        #endregion
    }


    public interface INetPacket : INetSerializable
    {
        PacketType Type { get; }
    }
}