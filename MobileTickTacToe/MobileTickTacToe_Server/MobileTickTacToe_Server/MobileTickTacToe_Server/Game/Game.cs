using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Server.Game
{
    public class Game
    {
        public Game(string xUser, string yUser)
        {
            Id = Guid.NewGuid();
            GameStartTime = DateTime.UtcNow;
            CurrentRoundStartTime = DateTime.UtcNow;
            XUserName = xUser;
            YUserName = yUser;
            Round = 1;
            CurrentUserName = xUser;
        }

        public Guid Id { get; set; }
        public ushort Round { get; set; }
        public DateTime GameStartTime { get; set; }
        public DateTime CurrentRoundStartTime { get; set; }
        public string XUserName { get; set; }
        public ushort XUserWinCount { get; set; }
        public bool XWantRematch { get; set; }

        public string YUserName { get; set; }
        public ushort YUserWinCount { get; set; }
        public bool YWantRematch { get; set; }

        public string CurrentUserName { get; set; }
    }
}
