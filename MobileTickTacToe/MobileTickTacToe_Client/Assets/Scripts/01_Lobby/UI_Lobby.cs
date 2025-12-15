using NetworkShared.Packets.ClientToServer;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTickTacToe_Client.Login
{
    class UI_Lobby : UI_Base
    {
        Dictionary<string, int> enumNumbers = new Dictionary<string, int>();

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
            RequestServerStatus();
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
