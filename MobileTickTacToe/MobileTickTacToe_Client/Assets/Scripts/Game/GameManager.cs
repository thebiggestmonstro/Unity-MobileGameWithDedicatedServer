using System;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class GameManager : MonoBehaviour
    {
        public class Game
        {
            public Guid? Id { get; set; }
            public string XUserName { get; set; }
            public string YUserName { get; set; }
            public string CurrentUser { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
        }

        private Game _activeGame;
        public Game ActiveGame
        {
            get
            {
                return _activeGame;
            }
        }

        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public bool InputEnabled { get; set; }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void RegisterGame(Guid gameId, string xUserName, string yUSerName)
        {
            _activeGame = new Game
            {
                Id = gameId,
                XUserName = xUserName,
                YUserName = yUSerName,
                StartTime = DateTime.Now,
                CurrentUser = xUserName
            };

            InputEnabled = true;
        }
    }
}