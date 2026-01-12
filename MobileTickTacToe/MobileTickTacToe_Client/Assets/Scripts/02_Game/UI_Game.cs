
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;
using Assets.Scripts.Game;
using MobileTickTacToe_Client.PacketHandlers;
using NetworkShared.Packets.ServerToClient;
using NetworkShared.Models;
using System.Collections;

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
            Txt_Timer
        }

        private Transform _turn;
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
            _turn = transform.Find("Turn");

            OnMarkCellHandler.OnMarkCell += HandleMarkCell;

            InitHeader();
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
                Debug.Log("Showing End Round Screen!");
                return;
            }

            StopCoroutine(ShowTurn());
            StartCoroutine(ShowTurn());
        }

        private IEnumerator ShowTurn()
        {
            _turn.gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            _turn.gameObject.SetActive(true);
        }
    }
}
