using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Server.Utils
{
    public static class BasicExtensions
    {
        public static (byte, byte) GetRowCol(byte index)
        {
            var row = (byte)(index / 3);
            var col = (byte)(index % 3);

            return (row, col);
        }
    }
}
