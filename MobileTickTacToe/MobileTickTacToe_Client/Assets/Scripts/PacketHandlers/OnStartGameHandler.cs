using Assets.Scripts.Game;
using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ServerToClient;
using UnityEngine.SceneManagement;

namespace MobileTickTacToe_Client.PacketHandlers
{
    [HandlerRegister(PacketType.OnStartGame)]
    public class OnStartGameHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_OnStartGame)packet;
            GameManager.Instance.RegisterGame(msg.GameId, msg.XUserName, msg.YUserName);
            SceneManager.LoadScene("02_Game");
        }
    }
}