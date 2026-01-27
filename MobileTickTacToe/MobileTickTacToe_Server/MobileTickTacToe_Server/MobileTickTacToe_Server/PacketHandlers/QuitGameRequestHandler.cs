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
    [HandlerRegister(PacketType.QuitGameRequest)]
    public class QuitGameRequestHandler : IPacketHandler
    {
        private readonly UsersManager _usersManager;
        private readonly GameManager _gameManager;
        private readonly NetworkServer _server;

        public QuitGameRequestHandler(UsersManager usersManager, GameManager gameManager, NetworkServer server)
        {
            _usersManager = usersManager;
            _gameManager = gameManager;
            _server = server;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            var connection = _usersManager.GetConnection(connectionId);
            var game = _gameManager.FindGame(connection.User.Id);
            var opponentId = game.GetOpponent(connection.User.Id);

            var rmsg = new Net_OnQuitGame
            {
                QuitterName = connection.User.Id
            };

            if (_gameManager.GameExist(connection.User.Id))
            {
                var closedGame = _gameManager.CloseGame(connection.User.Id);
                var opponent = closedGame.GetOpponent(connection.User.Id);
                var opponentConnection = _usersManager.GetConnection(opponent);
                _server.SendClient(opponentConnection.ConnectionId, rmsg);
            }

            _server.SendClient(connection.ConnectionId, rmsg);
        }
    }
}
