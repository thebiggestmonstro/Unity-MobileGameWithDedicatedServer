using NetworkShared;
using NetworkShared.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MobileTickTacToe_Client.PacketHandlers
{
    [HandlerRegister(PacketType.OnAuth)]
    public class OnAuthHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            SceneManager.LoadScene("01_Lobby");
        }
    }
}
