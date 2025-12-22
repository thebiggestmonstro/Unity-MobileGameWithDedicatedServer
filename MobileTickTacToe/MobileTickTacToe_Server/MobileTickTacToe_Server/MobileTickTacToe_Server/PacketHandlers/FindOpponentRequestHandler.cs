using MobileTickTacToe_Server.Game;
using MobileTickTacToe_Server.MatchMaking;
using NetworkShared;
using NetworkShared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Server.PacketHandlers
{
    [HandlerRegister(PacketType.FindOpponentRequest)]
    public class FindOpponentRequestHandler : IPacketHandler
    {
        private readonly UsersManager _usersManager;
        private readonly MatchMaker _matchMaker;

        public FindOpponentRequestHandler(UsersManager usersManager, MatchMaker matchMaker)
        { 
            _usersManager = usersManager;
            _matchMaker = matchMaker;
        }

        public void Handle(INetPacket packet, int connectionId)
        {
            var connection = _usersManager.GetConnection(connectionId);
            _matchMaker.RegisterPlayer(connection);
        }
    }
}
