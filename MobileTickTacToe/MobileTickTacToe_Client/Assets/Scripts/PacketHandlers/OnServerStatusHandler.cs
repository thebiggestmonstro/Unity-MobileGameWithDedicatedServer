using NetworkShared;
using NetworkShared.Attributes;
using UnityEngine;

namespace Assets.Scripts.PacketHandlers
{
    [HandlerRegister(PacketType.OnServerStatus)]
    public class OnServerStatusHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            Debug.Log("OnServerStatusHandler Triggered");
        }
    }
}