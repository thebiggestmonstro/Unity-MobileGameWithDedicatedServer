using MobileTickTacToe_Server.Data;
using MobileTickTacToe_Server.Game;
using NetworkShared.Packets.ServerToClient;
using NetworkShared;
using NetworkShared.Attributes;
using TickTackToeWithDedicated_Server;

namespace MobileTickTacToe_Server.PacketHandlers
{
    [HandlerRegister(PacketType.ServerStatusRequest)]
    public class ServerStatusRequestHandler : IPacketHandler
    {
        private readonly NetworkServer _server;
        private readonly IUserRepository _userRepository;
        private readonly UsersManager _usersManager;

        public ServerStatusRequestHandler(NetworkServer server, IUserRepository userRepository, UsersManager usersManager)
        { 
            _server = server;
            _userRepository = userRepository;
            _usersManager = usersManager;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = new Net_OnServerStatus
            {
                PlayersCount = _userRepository.GetTotalCount(),
                TopPlayers = _usersManager.GetTopPlayers()
            };

            _server.SendClient(connectionId, msg);
        }
    }
}
