using Assets.Scripts.Game;
using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Models;
using NetworkShared.Packets.ServerToClient;
using System;
using UnityEngine;

namespace MobileTickTacToe_Client.PacketHandlers 
{

    [HandlerRegister(PacketType.OnMarkCell)]
    public class OnMarkCellHandler : IPacketHandler
    {
        public static event Action<Net_OnMarkCell> OnMarkCell;

        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_OnMarkCell)packet;

            GameManager.Instance.ActiveGame.SwitchCurrentPlayer();
            if (GameManager.Instance.IsMyTurn && msg.Outcome == MarkOutcome.None)
            {
                GameManager.Instance.InputEnabled = true;
            }

            if(msg.Outcome > MarkOutcome.None)
            {
                GameManager.Instance.InputEnabled = false;
            }

            OnMarkCell?.Invoke(msg);
        }
    }
}
