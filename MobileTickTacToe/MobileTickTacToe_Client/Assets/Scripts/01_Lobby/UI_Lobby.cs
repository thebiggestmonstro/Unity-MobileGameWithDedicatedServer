using MobileTickTacToe_Client.PacketHandlers;
using NetworkShared.Packets.ClientToServer;
using System.Collections.Generic;
using UnityEngine;
using NetworkShared.Packets.ServerToClient;
using Unity.VisualScripting;
using TMPro;

namespace MobileTickTacToe_Client.Login
{
    class UI_Lobby : UI_Base
    {
        Dictionary<string, int> enumNumbers = new Dictionary<string, int>();
        private Transform _topPlayersContainer;
        private TextMeshProUGUI _totalPlayers;

        [SerializeField]
        GameObject _playerRowPrefab;

        public enum GameObjects_Btn
        {
            Btn_FindOpponent,
            Btn_Logout
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
            _topPlayersContainer = GetObject(enumNumbers[GetEnumFullName(GameObjects_Vlg.Vlg_PlayerList)]).transform;
            _totalPlayers = GetObject(enumNumbers[GetEnumFullName(GameObjects_Text.Txt_TotalPlayers)]).GetOrAddComponent<TextMeshProUGUI>();
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

        // FindOpppnent()

        // CancelFindOpponent()

        // Logout()

        // RefreshUI()
    }
}
