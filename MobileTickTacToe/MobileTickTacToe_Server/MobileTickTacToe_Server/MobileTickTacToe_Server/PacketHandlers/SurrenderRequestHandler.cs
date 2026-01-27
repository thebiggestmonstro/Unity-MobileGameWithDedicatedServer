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
    [HandlerRegister(PacketType.SurrenderRequest)]

    public class SurrenderRequestHandler : IPacketHandler
    {

        private readonly UsersManager _usersManager;
        private readonly GameManager _gameManager;
        private readonly NetworkServer _server;

        public SurrenderRequestHandler(UsersManager usersManager, GameManager gameManager, NetworkServer server)
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
            game.AddWin(opponentId);
            _usersManager.IncreaseScore(opponentId);

            var rmsg = new Net_OnSurrender
            { 
                WinnerName = opponentId
            };

            var opponentConnection = _usersManager.GetConnection(opponentId);
            _server.SendClient(opponentConnection.ConnectionId, rmsg);
            _server.SendClient(connectionId, rmsg);
        }
    }
}
