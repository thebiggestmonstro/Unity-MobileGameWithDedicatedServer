using UnityEngine;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace MobileTickTacToe_Client.Game
{
    public class UI_EndRound : MonoBehaviour
    {
        [SerializeField]
        Color _winColor;
        [SerializeField]
        Color _looseColor;
        [SerializeField]
        Color _drawColor;
        [SerializeField]
        string _winText = "YOU WIN!";
        [SerializeField]
        string _looseText = "YOU LOOSE!";
        [SerializeField]
        string _drawText = "DRAW!";

        private Transform _imgDark;
        private Transform _imgPopupPanel;
        #region Subwidget of Img_PopupPanel 
        private Image _imgWinLoose;
        private TextMeshProUGUI _txtWinLoose;
        private Transform _txtOpponentLeft;
        private Transform _txtWatingForOpponent;
        private Transform _txtPlayAgain;
        private Transform _btnPlayAgain;
        private Transform _btnAccept;
        private Transform _btnQuit;
        #endregion

        private void OnEnable()
        {
            _imgPopupPanel = transform.Find("Img_PopupPanel");
            _btnPlayAgain = _imgPopupPanel.Find("Btn_PlayAgain");
            _btnAccept = _imgPopupPanel.Find("Btn_Accept");
            _btnQuit = _imgPopupPanel.Find("Btn_Quit");

            _txtWinLoose = _imgPopupPanel.Find("Txt_WinLoose").GetOrAddComponent<TextMeshProUGUI>();
            _imgWinLoose = _imgPopupPanel.Find("Img_WinLoose").GetOrAddComponent<Image>();
            _txtOpponentLeft = _imgPopupPanel.Find("Txt_OpponentLeft");
            _txtPlayAgain = _imgPopupPanel.Find("Txt_PlayAgain");
            _txtWatingForOpponent = _imgPopupPanel.Find("Txt_WaitingForOpponent");
        }


        public void Init(string playerName, bool isDraw)
        {
            if (isDraw)
            {
                _imgWinLoose.color = _drawColor;
                _txtWinLoose.text = _drawText;
                return;
            }

            var isWin = GameManager.Instance.MyUserName == playerName;

            if (isWin)
            {
                _imgWinLoose.color = _winColor;
                _txtWinLoose.text = _winText;
            }
            else 
            {
                _imgWinLoose.color = _looseColor;
                _txtWinLoose.text = _looseText;
            }
        }
    }
}