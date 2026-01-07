using Microsoft.Extensions.Logging;
using MobileTickTacToe_Server.Game;
using MobileTickTacToe_Server.Utils;
using NetworkShared;
using NetworkShared.Models;
using NetworkShared.Attributes;
using NetworkShared.Packets.ClientToServer;
using NetworkShared.Packets.ServerToClient;
using TickTackToeWithDedicated_Server;

namespace MobileTickTacToe_Server.PacketHandlers
{
    [HandlerRegister(PacketType.MarkCellRequst)]
    public class MarkCellRequestHandler : IPacketHandler
    {
        private readonly UsersManager _usersManager;
        private readonly GameManager _gameManager;
        private readonly NetworkServer _server;
        private readonly ILogger<MarkCellRequestHandler> _logger;

        public MarkCellRequestHandler(UsersManager usersManager, GameManager gameManager, NetworkServer server, ILogger<MarkCellRequestHandler> logger)
        {
            _usersManager = usersManager;
            _gameManager = gameManager;
            _server = server;   
            _logger = logger;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_MarkCellRequest)packet;
            var connection = _usersManager.GetConnection(connectionId);
            var userId = connection.User.Id;
            var game = _gameManager.FindGame(userId);

            Validate(msg.Index, userId, game);

            var result = game.MarkCell(msg.Index);
            var rmsg = new Net_OnMarkCell
            {
                PlayerName = userId,
                Index = msg.Index,
                Outcome = result.Outcome,
                WinLineType = result.WinLineType
            };

            var opponentId = game.GetOpponent(userId);
            var opponentConnection = _usersManager.GetConnection(opponentId);

            _server.SendClient(connection.ConnectionId, rmsg);
            _server.SendClient(opponentConnection.ConnectionId, rmsg);

            _logger.LogInformation($"`{userId}` marked cell at index `{msg.Index}`!");

            if (result.Outcome == MarkOutcome.None)
            {
                game.SwitchCurrentPlayer();
                return;
            }

            if (result.Outcome == MarkOutcome.Win)
            {
                game.AddWin(userId);
                _usersManager.IncreaseScore(userId);

                _logger.LogInformation($"`{userId}` is a winner! Increasing score and win counter!");
            }
        }

        private void Validate(byte index, string playerName, MobileTickTacToe_Server.Game.Game game)
        {
            if (game.CurrentUserName != playerName)
            {
                throw new ArgumentException($"[Bad Reqeust] Player {playerName} is not the current user!!!");
            }

            var (row, col) = BasicExtensions.GetRowCol(index);

            if (game.Grid[row, col] != 0)
            {
                throw new ArgumentException($"[Bad Reqeust] cell with index '{index}' at row '{row}' and colr '{col}' is already marked!!!");
            }
        }
    }
}
