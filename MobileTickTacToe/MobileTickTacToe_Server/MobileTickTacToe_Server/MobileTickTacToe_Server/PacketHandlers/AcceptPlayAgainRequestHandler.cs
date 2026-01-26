using Microsoft.Extensions.Logging;
using MobileTickTacToe_Server.Game;
using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ServerToClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TickTackToeWithDedicated_Server;

namespace MobileTickTacToe_Server.PacketHandlers
{
    [HandlerRegister(PacketType.AcceptPlayAgainRequest)]
    public class AcceptPlayAgainRequestHandler : IPacketHandler
    {
        private readonly UsersManager _usersManager;
        private readonly GameManager _gameManager;
        private readonly ILogger<AcceptPlayAgainRequestHandler> _logger;
        private readonly NetworkServer _server;

        public AcceptPlayAgainRequestHandler(UsersManager usersManager, GameManager gameManager, ILogger<AcceptPlayAgainRequestHandler> logger, NetworkServer server)
        {
            _usersManager = usersManager;
            _gameManager = gameManager;
            _logger = logger;
            _server = server;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            var connection = _usersManager.GetConnection(connectionId);
            var userId = connection.User.Id;
            var game = _gameManager.FindGame(userId);
            game.SetRematchReadiness(userId);

            if (!game.BothPlayersReady())
            {
                _logger.LogWarning("Both players arre not ready!!!");
            }

            game.NewRound();

            var opponentId = game.GetOpponent(userId);
            var opponentConnection = _usersManager.GetConnection(opponentId);

            var rmsg = new Net_OnNewRound();
            _server.SendClient(connection.ConnectionId, rmsg);
            _server.SendClient(opponentConnection.ConnectionId, rmsg);
        }
    }
}
