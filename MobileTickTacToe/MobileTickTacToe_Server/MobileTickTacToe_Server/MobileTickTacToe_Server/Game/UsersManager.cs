using LiteNetLib;
using MobileTickTacToe_Server.Data;
using NetworkShared.Packets.ServerToClient;

namespace MobileTickTacToe_Server.Game
{
    public class UsersManager
    {
        private readonly IUserRepository _userRepository;
        private Dictionary<int, ServerConnection> _connections;

        public UsersManager(IUserRepository userRepository)
        { 
            _connections = new Dictionary<int, ServerConnection>();
            _userRepository = userRepository;
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
            var dbUser = _userRepository.Get(userName);

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

                _userRepository.Add(newUser);
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
                _userRepository.SetOffline(userId);
            }

            _connections.Remove(peerId);
        }

        public ServerConnection GetConnection(int peerId)
        {
            return _connections[peerId];
        }

        public int[] GetOtherConnectionIds(int excluededConnectionId)
        {
            return _connections.Keys.Where(k => k != excluededConnectionId).ToArray();
        }

        public PlayersNetDto[] GetTopPlayers()
        {
            return _userRepository.GetQuery()
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
    }
}
