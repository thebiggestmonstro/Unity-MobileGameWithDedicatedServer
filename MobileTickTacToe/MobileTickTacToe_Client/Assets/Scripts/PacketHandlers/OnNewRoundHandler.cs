using Assets.Scripts.Game;
using NetworkShared;
using NetworkShared.Attributes;
using System;

namespace MobileTickTacToe_Client.PacketHandlers
{
    [HandlerRegister(PacketType.OnNewRound)]
    public class OnNewRoundHandler : IPacketHandler
    {
        public static event Action OnNewRound;

        public void Handle(INetPacket packet, int connectionId)
        {
            OnNewRound?.Invoke();
            GameManager.Instance.ActiveGame.Reset();
            GameManager.Instance.InputEnabled = true;
        }
    }
}
