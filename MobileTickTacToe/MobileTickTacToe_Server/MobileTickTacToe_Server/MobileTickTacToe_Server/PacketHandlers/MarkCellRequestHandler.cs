using MobileTickTacToe_Server.Game;
using MobileTickTacToe_Server.Utils;
using NetworkShared;
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

        public MarkCellRequestHandler(UsersManager usersManager, GameManager gameManager, NetworkServer server)
        {
            _usersManager = usersManager;
            _gameManager = gameManager;
            _server = server;   
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_MarkCellRequest)packet;
            var connection = _usersManager.GetConnection(connectionId);
            var userId = connection.User.Id;
            MobileTickTacToe_Server.Game.Game game = _gameManager.FindGame(userId);

            // 1) Validate game
            Validate(msg.Index, userId, game);

            // 2) Get current game and invoke game.MarkCell(), got outcome
            var result = game.MarkCell(msg.Index);
            var rmsg = new Net_OnMarkCell
            {
                PlayerName = userId,
                Index = msg.Index,
                Outcome = result.Outcome,
                WinLineType = result.WinLineType
            };

            var oppeonentId = game.GetOpponent(userId);
            var opponentConnection = _usersManager.GetConnection(oppeonentId);

            _server.SendClient(connection.ConnectionId, rmsg);
            _server.SendClient(opponentConnection.ConnectionId, rmsg);

            // TODO
            // 
            // 3) Do each action with following outcome
            // -- None : Switch Current Player
            // -- Win : Increase Player Score and add a win
            // -- Draw : Do Nothing
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
