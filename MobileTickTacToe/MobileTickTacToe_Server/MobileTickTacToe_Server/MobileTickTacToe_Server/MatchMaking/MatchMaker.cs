
using Microsoft.Extensions.Logging;
using MobileTickTacToe_Server.Game;
using NetworkShared.Packets.ServerToClient;
using TickTackToeWithDedicated_Server;

namespace MobileTickTacToe_Server.MatchMaking
{
    public class MatchMaker
    {
        private ILogger<MatchMaker> _logger;
        private GameManager _gameManager;
        private NetworkServer _server;
        private List<MatchMakingRequest> _requestPool = new List<MatchMakingRequest>();

        public MatchMaker(ILogger<MatchMaker> logger, GameManager gameManaer, NetworkServer server)
        {
            _logger = logger;
            _gameManager = gameManaer;
            _server = server;
        }

        public void RegisterPlayer(ServerConnection connection)
        {
            if (_requestPool.Any(x => x.Connection.User.Id == connection.User.Id))
            {
                _logger.LogWarning($"{connection.User.Id} is already registerd!!! So ignoring...");
                return;
            }

            var request = new MatchMakingRequest()
            { 
                Connection = connection,
                SearchStartTime = DateTime.UtcNow,
            };

            _requestPool.Add(request);
            _logger.LogInformation($"{request.Connection.User.Id} has been registered in MatchMakingRequest Pool");

            DoMatchMaking();
        }

        public void TryUnregisterPlayer(string username)
        {
            var request = _requestPool.FirstOrDefault(r => r.Connection.User.Id == username);
            if (request != null)
            {
                _logger.LogInformation($"Remove {request.Connection.User.Id} from MatchMakingRquest Pool");
                _requestPool.Remove(request);
            }
        }

        private void DoMatchMaking()
        {
            var matchMakingRequests = new List<MatchMakingRequest>();

            foreach (var request in _requestPool)
            {
                var match = _requestPool.FirstOrDefault(x => !x.MatchFound && x.Connection.ConnectionId != request.Connection.ConnectionId);

                if (match == null)
                {
                    continue;
                }

                request.MatchFound = true;
                match.MatchFound = true;
                matchMakingRequests.Add(request);
                matchMakingRequests.Add(match);

                var xUser = request.Connection.User.Id;
                var yUser = match.Connection.User.Id;
                var gameId = _gameManager.RegisterGame(xUser, yUser);
                request.Connection.GameId = gameId;
                match.Connection.GameId = gameId;

                var msg = new Net_OnStartGame
                { 
                    GameId = gameId,    
                    XUserName = xUser,
                    YUserName = yUser,
                };

                var p1 = request.Connection.ConnectionId;
                var p2 = match.Connection.ConnectionId;
                _server.SendClient(p1, msg);
                _server.SendClient(p2, msg);

                _logger.LogInformation($"Matched Player {xUser} and  Player {yUser}!!!");
            }

            foreach (var request in matchMakingRequests)
            {
                _requestPool.Remove(request);
            }
        }
    }
}
