using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ServerToClient;
using System;
using UnityEngine;

namespace Assets.Scripts.PacketHandlers
{
    [HandlerRegister(PacketType.OnAuthFailed)]
    public class OnAuthFailedHandler : IPacketHandler
    {
        public static event Action<Net_OnAuthFailed> OnAuthFailed;

        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_OnAuthFailed)packet;
            OnAuthFailed?.Invoke(msg);
        }
    }
}
