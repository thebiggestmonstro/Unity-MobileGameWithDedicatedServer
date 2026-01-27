using Assets.Scripts.Game;
using MobileTickTacToe_Client.PacketHandlers;
using NetworkShared.Packets.ClientToServer;
using NetworkShared.Packets.ServerToClient;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        private float _originalPanelHeight;
        private bool _opponentLeft = false;

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

            _btnPlayAgain.GetOrAddComponent<Button>().onClick.AddListener(RequestPlayAgain);
            _btnAccept.GetOrAddComponent<Button>().onClick.AddListener(Accept);
            _btnQuit.GetOrAddComponent<Button>().onClick.AddListener(Quit);

            OnPlayAgainHandler.OnPlayAgain += HandlePlayAgainRequest;
            OnNewRoundHandler.OnNewRound += ResetUI;
            OnQuitGameHandler.OnQuitGame += HandleOpponentLeft;

            var rectTransform = _imgPopupPanel.GetComponent<RectTransform>();
            _originalPanelHeight = rectTransform.sizeDelta.y;

            LeanTween.scale(_imgPopupPanel.gameObject, new Vector3(1.0f, 1.0f, 1.0f), 1.0f).setEase(LeanTweenType.easeOutBounce);
        }

        private void OnDisable()
        {
            _btnPlayAgain.GetOrAddComponent<Button>().onClick.RemoveListener(RequestPlayAgain);
            _btnAccept.GetOrAddComponent<Button>().onClick.RemoveListener(Accept);
            _btnQuit.GetOrAddComponent<Button>().onClick.RemoveListener(Quit);

            OnPlayAgainHandler.OnPlayAgain -= HandlePlayAgainRequest;
            OnNewRoundHandler.OnNewRound -= ResetUI;
            OnQuitGameHandler.OnQuitGame -= HandleOpponentLeft;

            var rectTransform = _imgPopupPanel.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, _originalPanelHeight);
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

        private void RequestPlayAgain()
        {
            _btnPlayAgain.gameObject.SetActive(false);
            _txtWatingForOpponent.gameObject.SetActive(true);

            var msg = new Net_PlayAgainRequest();
            NetworkClient.Instance.SendServer(msg);
        }

        private void Accept()
        {
            _btnAccept.GetComponent<Button>().interactable = false;
            var msg = new Net_AcceptPlayAgainRequest();
            NetworkClient.Instance.SendServer(msg);
        }

        private void HandlePlayAgainRequest()
        {
            _btnPlayAgain.gameObject.SetActive(false);
            _btnAccept.gameObject.SetActive(true);
            _txtPlayAgain.gameObject.SetActive(true);

            var rectTransform = _imgPopupPanel.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + 75f);
        }

        private void ResetUI()
        {
            _btnPlayAgain.gameObject.SetActive(true);
            _txtWatingForOpponent.gameObject.SetActive(false);
            _btnAccept.GetComponent<Button>().interactable = true;
            _btnAccept.gameObject.SetActive(false);
            _txtPlayAgain.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        private void Quit()
        {
            if (_opponentLeft)
            {
                SceneManager.LoadScene("01_Lobby");
                return;
            }

            _btnQuit.GetComponent<Button>().interactable = false;
            var msg = new Net_QuitGameRequest();
            NetworkClient.Instance.SendServer(msg);
        }

        public void HandleOpponentLeft(Net_OnQuitGame msg)
        {
            _btnPlayAgain.gameObject.SetActive(false);
            _txtOpponentLeft.gameObject.SetActive(true);
            _txtWatingForOpponent.gameObject.SetActive(false);
            _txtPlayAgain.gameObject.SetActive(false);
            var rectTransform = _imgPopupPanel.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, _originalPanelHeight);
            _btnAccept.gameObject.SetActive(false);
            _opponentLeft = true;
        }
    }
}