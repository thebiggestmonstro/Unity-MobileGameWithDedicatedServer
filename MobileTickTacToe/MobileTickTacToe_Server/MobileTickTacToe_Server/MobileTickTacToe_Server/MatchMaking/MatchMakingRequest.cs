using MobileTickTacToe_Server.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Server.MatchMaking
{
    public class MatchMakingRequest
    {
        public ServerConnection Connection { get; set; }
        public DateTime SearchStartTime { get; set; }
        public bool MatchFound { get; set; }
    }
}
