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
    [HandlerRegister(PacketType.PlayAgainRequest)]
    public class PlayAgainRequestHandler : IPacketHandler
    {
        private readonly UsersManager _usersManager;
        private readonly GameManager _gameManager;
        private readonly NetworkServer _networkServer;

        public PlayAgainRequestHandler(UsersManager usersManager, GameManager gameManager, NetworkServer networkServer)
        {
            _usersManager = usersManager;
            _gameManager = gameManager;
            _networkServer = networkServer;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            var connection = _usersManager.GetConnection(connectionId);
            var userId = connection.User.Id;
            var game = _gameManager.FindGame(userId);
            game.SetRematchReadiness(userId);

            var rmsg = new Net_OnPlayAgain();

            var opponentId = game.GetOpponent(userId);
            var opponentConnection = _usersManager.GetConnection(opponentId);
            _networkServer.SendClient(opponentConnection.ConnectionId, rmsg);
        }
    }
}
