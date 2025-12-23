using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Server.Game
{
    public class GameManager
    {
        private List<Game> _games;

        public GameManager()
        {
            _games = new List<Game>();
        }

        public Guid RegisterGame(string xUser, string yUser)
        {
            var newGame = new Game(xUser, yUser);
            _games.Add(newGame);
            return newGame.Id;
        }

        public Game FindGame(string username)
        {
            return _games.FirstOrDefault(g => g.XUserName == username || g.YUserName == username);
        }

        public Game CloseGame(string username)
        {
            var game = FindGame(username);
            _games.Remove(game);
            return game;
        }

        public bool GameExist(string username)
        { 
            return _games.Any(g => g.XUserName == username || g.YUserName == username);
        }

        public int GetGameCount()
        {
            return _games.Count;
        }
    }
}
