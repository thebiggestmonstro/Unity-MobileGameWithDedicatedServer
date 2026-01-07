using LiteNetLib;
using MobileTickTacToe_Server.Data;
using NetworkShared.Packets.ServerToClient;
using TickTackToeWithDedicated_Server;

namespace MobileTickTacToe_Server.Game
{
    public class UsersManager
    {
        private readonly IUserRepository _usersRepository;
        private Dictionary<int, ServerConnection> _connections;
        private NetworkServer _server;

        public UsersManager(IUserRepository userRepository, NetworkServer server)
        { 
            _connections = new Dictionary<int, ServerConnection>();
            _usersRepository = userRepository;
            _server = server;
        }

        public void AddConnection(NetPeer peer)
        {
            _connections.Add(peer.Id, new ServerConnection
            {
                ConnectionId = peer.Id,
                Peer = peer
            });
        }

        public bool LoginOrRegister(int connectionId, string userName, string password)
        {
            var dbUser = _usersRepository.Get(userName);

            if (dbUser != null)
            {
                if (dbUser.Password != password)
                {
                    return false;
                }
            }

            if (dbUser == null)
            {
                var newUser = new User
                {
                    Id = userName,
                    Password = password,
                    IsOnline = true,
                    Score = 0,
                };

                _usersRepository.Add(newUser);
                dbUser = newUser;
            }

            if (_connections.ContainsKey(connectionId))
            {
                dbUser.IsOnline = true;
                _connections[connectionId].User = dbUser;
            }

            return true;
        }

        public void Disconnect(int peerId)
        {
            var connection = GetConnection(peerId);

            if (connection.User != null)
            {
                var userId = connection.User.Id;
                _usersRepository.SetOffline(userId);

                NotifyOtherPlayers(peerId);
            }

            _connections.Remove(peerId);
        }

        public ServerConnection GetConnection(int peerId)
        {
            return _connections[peerId];
        }

        public ServerConnection GetConnection(string userId)
        {
            return _connections.FirstOrDefault(x => x.Value.User.Id == userId).Value;
        }

        public int[] GetOtherConnectionIds(int excluededConnectionId)
        {
            return _connections.Keys.Where(k => k != excluededConnectionId).ToArray();
        }

        public PlayersNetDto[] GetTopPlayers()
        {
            return _usersRepository.GetQuery()
                .OrderByDescending(x => x.Score)
                .Select(u => new PlayersNetDto
                {
                    UserName = u.Id,
                    Score = u.Score,
                    IsOnline = u.IsOnline,
                })
                .Take(9)
                .ToArray();
        }

        private void NotifyOtherPlayers(int excluededConnectionId)
        {
            var rmsg = new Net_OnServerStatus()
            {
                PlayersCount = _usersRepository.GetTotalCount(),
                TopPlayers = GetTopPlayers(),
            };

            var otherIds = GetOtherConnectionIds(excluededConnectionId);

            foreach (var connectId in otherIds)
            {
                _server.SendClient(connectId, rmsg);
            }
        }

        public void IncreaseScore(string userId)
        {
            var user = _usersRepository.Get(userId);
            user.Score += 10;
            _usersRepository.Update(user);
        }
    }
}
