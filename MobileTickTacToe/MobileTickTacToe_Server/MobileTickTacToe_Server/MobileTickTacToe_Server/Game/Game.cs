using MobileTickTacToe_Server.Utils;
using NetworkShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Server.Game
{
    public class Game
    {
        private const int GRID_SIZE = 3;

        public Game(string xUser, string yUser)
        {
            Id = Guid.NewGuid();
            GameStartTime = DateTime.UtcNow;
            CurrentRoundStartTime = DateTime.UtcNow;
            XUserName = xUser;
            YUserName = yUser;
            Round = 1;
            Grid = new MarkType[GRID_SIZE, GRID_SIZE];
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

        public MarkType[,] Grid { get; }

        public string GetOpponent(string otherUserId)
        {
            if (otherUserId == XUserName)
            {
                return YUserName;
            }

            return XUserName;
        }

        public MarkResult MarkCell(byte index)
        {
            var (row, col) = BasicExtensions.GetRowCol(index);
            Grid[row, col] = GetPlayerType(CurrentUserName);

            var (isWin, lineType) = CheckWin(row, col);
            var draw = CheckDraw();

            var result = new MarkResult();

            if (isWin)
            {
                result.Outcome = MarkOutcome.Win;
                result.WinLineType = lineType;
            }
            else if (draw)
            {
                result.Outcome = MarkOutcome.Draw;
            }

            return result;
        }

        private MarkType GetPlayerType(string userId)
        {
            if (userId == XUserName)
            {
                return MarkType.X;
            }

            return MarkType.Y;
        }

        private (bool isWin, WinLineType lineType) CheckWin(byte row, byte col)
        {
            var type = Grid[row, col];

            // check row
            for (int i = 0; i < GRID_SIZE; i++)
            {
                if (Grid[row, i] != type)
                {
                    break;
                }

                if (i == GRID_SIZE - 1)
                {
                    return (true, ResolveLineTypeRow(row));
                }
            }

            // check col
            for (int i = 0; i < GRID_SIZE; i++)
            {
                if(Grid[i, col] != type)
                {
                    break;
                }

                if (i == GRID_SIZE - 1)
                {
                    return (true, ResolveLineTypeCol(col));
                }
            }

            // check diagonal
            if (row == col)
            {
                for (int i = 0; i < GRID_SIZE; i++)
                {
                    if (Grid[i, i] != type)
                    {
                        break;
                    }

                    if (i == GRID_SIZE - 1)
                    {
                        return (true, WinLineType.Diagonal);
                    }
                }
            }

            // check anti-diagonal
            if (row + col == GRID_SIZE - 1)
            {
                for (int i = 0; i < GRID_SIZE; i++)
                {
                    if (Grid[i, (GRID_SIZE - 1) - i] != type)
                    {
                        break;
                    }

                    if (i == GRID_SIZE - 1)
                    {
                        return (true, WinLineType.AntiDiagonal);
                    }
                }
            }

            return (false, WinLineType.None);
        }

        private bool CheckDraw()
        {
            for (int row = 0; row < GRID_SIZE; row++)
            {
                for (int col = 0; col < GRID_SIZE; col++)
                {
                    if (Grid[row, col] == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private WinLineType ResolveLineTypeRow(byte row)
        {
            return (WinLineType)(row + 6);
        }

        private WinLineType ResolveLineTypeCol(byte col)
        {
            return (WinLineType)(col + 3);
        }

        public void SwitchCurrentPlayer()
        {
            CurrentUserName = GetOpponent(CurrentUserName);
        }

        public void AddWin(string winnerId)
        {
            var winnerType = GetPlayerType(winnerId);
            if (winnerType == MarkType.X)
            {
                XUserWinCount++;
            }
            else
            {
                YUserWinCount++;
            }
        }

        public void SetRematchReadiness(string userId)
        {
            var playerType = GetPlayerType(userId);
            if (playerType == MarkType.X)
            {
                XWantRematch = true;
            }
            else
            {
                YWantRematch = true;
            }
        }
    }

    public struct MarkResult
    { 
        public MarkOutcome Outcome { get; set; }
        public WinLineType WinLineType { get; set; }
    }
}
