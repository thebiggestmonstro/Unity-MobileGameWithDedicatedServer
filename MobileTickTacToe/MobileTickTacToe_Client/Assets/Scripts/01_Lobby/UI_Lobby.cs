using MobileTickTacToe_Client.PacketHandlers;
using NetworkShared.Packets.ClientToServer;
using System.Collections.Generic;
using UnityEngine;
using NetworkShared.Packets.ServerToClient;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MobileTickTacToe_Client.Login
{
    class UI_Lobby : UI_Base
    {
        Dictionary<string, int> enumNumbers = new Dictionary<string, int>();
        private Transform _topPlayersContainer;
        private TextMeshProUGUI _totalPlayers;
        private Button _findOpponentButton;
        private Button _logoutButton;

        private Transform _waitingUI;
        private Button _cancleMatchingButton;

        [SerializeField]
        GameObject _playerRowPrefab;

        public enum GameObjects_Btn
        {
            Btn_FindOpponent,
            Btn_Logout,
        }

        public enum GameObjects_Vlg
        {
            Vlg_PlayerList,
        }

        public enum GameObjects_Text
        {
            Txt_TotalPlayers,
        }

        private void Awake()
        {
            GenerateEnumsSerialNumber<UI_Lobby>(enumNumbers);

            Bind<GameObject>(typeof(GameObjects_Btn));
            Bind<GameObject>(typeof(GameObjects_Vlg));
            Bind<GameObject>(typeof(GameObjects_Text));
        }

        private void Start()
        {
            _waitingUI = transform.Find("Waiting");
            _cancleMatchingButton = _waitingUI.Find("Btn_CancleMatching").GetOrAddComponent<Button>();
            _cancleMatchingButton.onClick.AddListener(CancelFindOpponent);

            _topPlayersContainer = GetObject(enumNumbers[GetEnumFullName(GameObjects_Vlg.Vlg_PlayerList)]).transform;
            _totalPlayers = GetObject(enumNumbers[GetEnumFullName(GameObjects_Text.Txt_TotalPlayers)]).GetOrAddComponent<TextMeshProUGUI>();

            _findOpponentButton = GetObject(enumNumbers[GetEnumFullName(GameObjects_Btn.Btn_FindOpponent)]).GetOrAddComponent<Button>();
            _findOpponentButton.onClick.AddListener(FindOpponent);
            _logoutButton = GetObject(enumNumbers[GetEnumFullName(GameObjects_Btn.Btn_Logout)]).GetOrAddComponent<Button>();
            _logoutButton.onClick.AddListener(Logout);

            OnServerStatusRequestHandler.OnServerStatus += Refresh;
            RequestServerStatus();
        }

        private void OnDestroy()
        {
            OnServerStatusRequestHandler.OnServerStatus -= Refresh;
        }

        private void Refresh(Net_OnServerStatus msg)
        {
            while (_topPlayersContainer.childCount > 0)
            {
                DestroyImmediate(_topPlayersContainer.GetChild(0).gameObject);
            }

            _totalPlayers.text = $"{msg.PlayersCount} player online";

            for (int i = 0; i < msg.TopPlayers.Length; i++)
            {
                var player = msg.TopPlayers[i];
                var instance = Instantiate(_playerRowPrefab, _topPlayersContainer).GetOrAddComponent<UI_PlayerRow>();
                instance.Init(player);
            }
        }

        private void RequestServerStatus()
        {
            var msg = new Net_ServerStatusRequest();
            NetworkClient.Instance.SendServer(msg);
        }

        private void Logout()
        {
            NetworkClient.Instance.Disconnect();
            SceneManager.LoadScene("00_Login");
        }

        private void FindOpponent()
        {
            LeanTween.cancelAll();
            LeanTween.reset();
            _findOpponentButton.gameObject.SetActive(false);
            _waitingUI.gameObject.SetActive(true);

            var msg = new Net_FindOpponentRequest();
            NetworkClient.Instance.SendServer(msg);
        }

        private void CancelFindOpponent()
        {
            _findOpponentButton.gameObject.SetActive(true);
            _waitingUI.gameObject.SetActive(false);

            var msg = new Net_CancleFindOpponentRequest();
            NetworkClient.Instance.SendServer(msg);
        }
    }
}
