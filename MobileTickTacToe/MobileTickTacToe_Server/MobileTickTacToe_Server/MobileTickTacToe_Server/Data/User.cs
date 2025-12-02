using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Server.Data
{
    public class User
    {
        public string Id { get; set; }

        public string Password { get; set; }

        public ushort Score { get; set; }

        public bool IsOnline { get; set; }
    }
}
