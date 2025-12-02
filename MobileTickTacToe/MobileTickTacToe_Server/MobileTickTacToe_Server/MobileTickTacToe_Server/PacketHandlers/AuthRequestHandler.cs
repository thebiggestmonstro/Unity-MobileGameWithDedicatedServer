using Microsoft.Extensions.Logging;
using MobileTickTacToe_Server.Game;
using MobileTickTacToe_Server.Handlers;
using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ClientToServer;

namespace MobileTickTacToe_Server.PacketHandlers
{
    [HandlerRegister(PacketType.AuthRequest)]
    public class AuthRequestHandler : IPacketHandler
    {
        private readonly ILogger<AuthRequestHandler> _logger;
        private readonly UsersManager _usersManager;

        public AuthRequestHandler(ILogger<AuthRequestHandler> logger, UsersManager usersManager)
        { 
            _logger = logger;
            _usersManager = usersManager;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            // 1) Logging
            var msg = (Net_AuthRequest)packet;

            _logger.LogInformation($"Received Login Request from user : {msg.UserName} with password : {msg.Password}");

            // 2) LoginOrRegisger

            bool loginSuccess = _usersManager.LoginOrRegister(connectionId, msg.UserName, msg.Password);

            // 3) Success -> Send Net_Auth Message / False -> Send Net_AuthFail Message
        }
    }
}
