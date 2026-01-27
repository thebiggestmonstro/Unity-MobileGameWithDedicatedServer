
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;
using Assets.Scripts.Game;
using MobileTickTacToe_Client.PacketHandlers;
using NetworkShared.Packets.ServerToClient;
using NetworkShared.Models;
using System.Collections;
using UnityEngine.UI;
using NetworkShared.Packets.ClientToServer;

namespace MobileTickTacToe_Client.Game
{
    class UI_Game : UI_Base
    {
        Dictionary<string, int> enumNumbers = new Dictionary<string, int>();

        public enum GameObjects_Btn
        {
            Btn_Surrender,
        }

        public enum GameObjects_Text
        {
            Txt_XScore,
            Txt_XUserName,
            Txt_YScore,
            Txt_YUserName,
            Txt_Timer
        }

        private Transform _turn;
        private Transform _endRoundPanel;
        private TextMeshProUGUI _xUserName;
        private TextMeshProUGUI _xUserScore;
        private TextMeshProUGUI _yUserName;
        private TextMeshProUGUI _yUserScore;

        private int _xScore = 0;
        private int _yScore = 0;

        private Button _surrenderBtn;

        private void Awake()
        {
            GenerateEnumsSerialNumber<UI_Game>(enumNumbers);

            Bind<GameObject>(typeof(GameObjects_Btn));
            Bind<GameObject>(typeof(GameObjects_Text));

            _xUserName = GetObject(enumNumbers[GetEnumFullName(GameObjects_Text.Txt_XUserName)]).GetOrAddComponent<TextMeshProUGUI>();
            _xUserScore = GetObject(enumNumbers[GetEnumFullName(GameObjects_Text.Txt_XScore)]).GetOrAddComponent<TextMeshProUGUI>();
            _yUserName = GetObject(enumNumbers[GetEnumFullName(GameObjects_Text.Txt_YUserName)]).GetOrAddComponent<TextMeshProUGUI>();
            _yUserScore = GetObject(enumNumbers[GetEnumFullName(GameObjects_Text.Txt_YScore)]).GetOrAddComponent<TextMeshProUGUI>();
            _turn = transform.Find("Turn");
            _endRoundPanel = transform.Find("EndRound");

            _surrenderBtn = GetObject(enumNumbers[GetEnumFullName(GameObjects_Btn.Btn_Surrender)]).GetOrAddComponent<Button>();
            _surrenderBtn.GetComponent<Button>().onClick.AddListener(Surrender);

            OnMarkCellHandler.OnMarkCell += HandleMarkCell;
            OnNewRoundHandler.OnNewRound += HandleNewRound;
            OnSurrenderHandler.OnSurrender += HandleSurrender;
            OnQuitGameHandler.OnQuitGame += HandleOpponentLeft;

            InitHeader();
        }

        private void OnDestroy()
        {
            _surrenderBtn.GetComponent<Button>().onClick.RemoveListener(Surrender);
            OnMarkCellHandler.OnMarkCell -= HandleMarkCell;
            OnNewRoundHandler.OnNewRound -= HandleNewRound;
            OnSurrenderHandler.OnSurrender -= HandleSurrender;
            OnQuitGameHandler.OnQuitGame -= HandleOpponentLeft;
        }

        private void InitHeader()
        {
            var game = GameManager.Instance.ActiveGame;
            _xUserName.text = "[x] " + game.XUserName;
            _yUserName.text = "[y] " + game.YUserName;
        }

        private void HandleMarkCell(Net_OnMarkCell msg)
        {
            if (msg.Outcome != MarkOutcome.None)
            {
                var isDraw = msg.Outcome == MarkOutcome.Draw;
                StartCoroutine(EndRoundCoroutine(msg.PlayerName, isDraw));
                return;
            }

            StopCoroutine(ShowTurn());
            StartCoroutine(ShowTurn());
        }

        private void HandleNewRound()
        {
            StopCoroutine(ShowTurn());
            StartCoroutine(ShowTurn());
        }

        private IEnumerator EndRoundCoroutine(string playerName, bool isDraw)
        {
            var waitTime = isDraw ? 1.5f : 2f;
            yield return new WaitForSeconds(waitTime);
            DisplayEndRoundUI(playerName, isDraw);
        }

        void DisplayEndRoundUI(string playerName, bool isDraw)
        {
            _endRoundPanel.gameObject.SetActive(true);
            _endRoundPanel.GetOrAddComponent<UI_EndRound>().Init(playerName, isDraw);

            if (isDraw)
            {
                return;
            }

            var playerType = GameManager.Instance.ActiveGame.GetPlayerType(playerName);
            if (playerType == MarkType.X)
            {
                _xScore++;
                _xUserScore.text = _xScore.ToString();
            }
            else if (playerType == MarkType.Y)
            {
                _yScore++;
                _yUserScore.text = _yScore.ToString();
            }
        }

        private IEnumerator ShowTurn()
        {
            _turn.gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            _turn.gameObject.SetActive(true);
        }

        private void Surrender()
        {
            var msg = new Net_SurrenderRequest();
            NetworkClient.Instance.SendServer(msg);
        }

        private void HandleSurrender(Net_OnSurrender msg)
        {
            DisplayEndRoundUI(msg.WinnerName, false);
        }

        private void HandleOpponentLeft(Net_OnQuitGame msg)
        { 
            if(!_endRoundPanel.gameObject.activeSelf)
            {
                _endRoundPanel.gameObject.SetActive(true);
                _endRoundPanel.GetOrAddComponent<UI_EndRound>().HandleOpponentLeft(msg);
            }
        }
    }
}
