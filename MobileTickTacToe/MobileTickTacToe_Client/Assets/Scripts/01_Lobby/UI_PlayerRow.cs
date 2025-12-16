using NetworkShared.Packets.ServerToClient;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace MobileTickTacToe_Client.Login
{
    public class UI_PlayerRow : MonoBehaviour
    {
        [SerializeField]
        Image Img_PlayerStatusOnline;
        [SerializeField]
        Image Img_PlayerStatusOffline;
        [SerializeField]
        TextMeshProUGUI Txt_RowUserName;
        [SerializeField]
        TextMeshProUGUI Txt_RowUserScore;

        public void Init(PlayersNetDto player)
        {
            if (player.IsOnline)
            {
                Img_PlayerStatusOnline.gameObject.SetActive(true);
            }
            else
            {
                Img_PlayerStatusOffline.gameObject.SetActive(true);
            }

            Txt_RowUserName.text = player.UserName;
            Txt_RowUserScore.text = player.Score.ToString();
        }
    }
}
