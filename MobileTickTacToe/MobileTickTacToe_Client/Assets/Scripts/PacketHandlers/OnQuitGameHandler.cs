using Assets.Scripts.Game;
using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ServerToClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace MobileTickTacToe_Client.PacketHandlers
{
    [HandlerRegister(PacketType.OnQuitGame)]
    public class OnQuitGameHandler : IPacketHandler
    {
        public static event Action<Net_OnQuitGame> OnQuitGame;

        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_OnQuitGame)packet;

            if (GameManager.Instance.MyUserName == msg.QuitterName)
            {
                SceneManager.LoadScene("01_Lobby");
                return;
            }

            OnQuitGame?.Invoke(msg);
        }
    }
}
