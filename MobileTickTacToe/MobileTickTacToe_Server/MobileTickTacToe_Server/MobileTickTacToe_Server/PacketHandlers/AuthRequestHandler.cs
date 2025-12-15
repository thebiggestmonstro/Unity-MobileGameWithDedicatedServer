using Microsoft.Extensions.Logging;
using MobileTickTacToe_Server.Data;
using MobileTickTacToe_Server.Game;
using MobileTickTacToe_Server.NetworkShared.Packets.ServerToClient;
using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ClientToServer;
using TickTackToeWithDedicated_Server;

namespace MobileTickTacToe_Server.PacketHandlers
{
    [HandlerRegister(PacketType.AuthRequest)]
    public class AuthRequestHandler : IPacketHandler
    {
        private readonly ILogger<AuthRequestHandler> _logger;
        private readonly UsersManager _usersManager;
        private readonly NetworkServer _server;
        private readonly IUserRepository _usersRepository;

        public AuthRequestHandler(ILogger<AuthRequestHandler> logger, UsersManager usersManager, NetworkServer server, IUserRepository userRepository)
        { 
            _logger = logger;
            _usersManager = usersManager;
            _server = server;
            _usersRepository = userRepository;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            // 1) Logging
            var msg = (Net_AuthRequest)packet;

            _logger.LogInformation($"Received Login Request from user : {msg.UserName} with password : {msg.Password}");

            // 2) LoginOrRegisger

            bool loginSuccess = _usersManager.LoginOrRegister(connectionId, msg.UserName, msg.Password);

            INetPacket rmsg;

            if (loginSuccess)
            {
                rmsg = new Net_OnAuth();
            }
            else
            {
                rmsg = new Net_OnAuthFailed();
            }

            _server.SendClient(connectionId, rmsg);

            if (loginSuccess)
            {
                NotifyOtherPlayers(connectionId);
            }

            // 3) Success -> Send Net_Auth Message / False -> Send Net_AuthFail Message
        }

        private void NotifyOtherPlayers(int excluededConnectionId)
        {
            var rmsg = new Net_OnServerStatus()
            {
                PlayersCount = _usersRepository.GetTotalCount(),
                TopPlayers = _usersManager.GetTopPlayers()
            };

            var otherIds = _usersManager.GetOtherConnectionIds(excluededConnectionId);

            foreach (var connectId in otherIds)
            {
                _server.SendClient(connectId, rmsg);
            }
        }
    }
}
