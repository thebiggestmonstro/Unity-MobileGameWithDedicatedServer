
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;
using static MobileTickTacToe_Client.Login.UI_Lobby;
using Assets.Scripts.Game;

namespace MobileTickTacToe_Client.Game
{
    class UI_Game : UI_Base
    {
        Dictionary<string, int> enumNumbers = new Dictionary<string, int>();

        public enum GameObjects_Btn
        {
            Btn_Logout
        }

        public enum GameObjects_Text
        {
            Txt_XScore,
            Txt_XUserName,
            Txt_YScore,
            Txt_YUserName,
            Txt_PlayerTurn,
            Txt_Timer
        }

        private TextMeshProUGUI _xUserName;
        private TextMeshProUGUI _xUserScore;
        private TextMeshProUGUI _yUserName;
        private TextMeshProUGUI _yUserScore;

        private void Awake()
        {
            GenerateEnumsSerialNumber<UI_Game>(enumNumbers);

            Bind<GameObject>(typeof(GameObjects_Btn));
            Bind<GameObject>(typeof(GameObjects_Text));

            _xUserName = GetObject(enumNumbers[GetEnumFullName(GameObjects_Text.Txt_XUserName)]).GetOrAddComponent<TextMeshProUGUI>();
            _xUserScore = GetObject(enumNumbers[GetEnumFullName(GameObjects_Text.Txt_XScore)]).GetOrAddComponent<TextMeshProUGUI>();
            _yUserName = GetObject(enumNumbers[GetEnumFullName(GameObjects_Text.Txt_YUserName)]).GetOrAddComponent<TextMeshProUGUI>();
            _yUserScore = GetObject(enumNumbers[GetEnumFullName(GameObjects_Text.Txt_YScore)]).GetOrAddComponent<TextMeshProUGUI>();

            InitHeader();
        }

        private void InitHeader()
        {
            var game = GameManager.Instance.ActiveGame;
            _xUserName.text = "[x] " + game.XUserName;
            _yUserName.text = "[y] " + game.YUserName;
        }
    }
}
