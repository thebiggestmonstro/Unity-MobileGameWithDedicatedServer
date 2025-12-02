using LiteNetLib;
using MobileTickTacToe_Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Server.Game
{
    public class ServerConnection
    {
        public int ConnectionId { get; set; }
        public User User { get; set; }

        public NetPeer Peer {get; set;}
    
        public Guid? GameId { get; set; }
    }
}
