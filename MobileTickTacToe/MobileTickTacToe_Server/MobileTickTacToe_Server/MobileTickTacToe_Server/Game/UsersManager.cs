using LiteNetLib;
using MobileTickTacToe_Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            // 기존에 존재하던 사용자
            if (dbUser != null)
            {
                // 비밀번호가 불일치
                if (dbUser.Password != password)
                {
                    return false;
                }
            }

            // 새로운 사용자
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

            // false를 리턴하지 않으므로 해당 사용자는 서버에 성공적으로 접속함
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
    }
}
