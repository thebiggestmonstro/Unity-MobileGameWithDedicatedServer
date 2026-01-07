using Assets.Scripts.Game;
using NetworkShared.Packets.ClientToServer;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using NetworkShared.Models;

namespace MobileTickTacToe_Client.Game 
{ 
    public class UI_BoardCell : MonoBehaviour
    {
        private Button _button;
        private byte _index;
        private byte _row;
        private byte _col;

        private Transform _oImg;
        private Transform _xImg;

        private void Start()
        {
            _oImg = transform.Find("Img_BtnO");
            _xImg = transform.Find("Img_BtnX");
        }

        public void Init(byte index)
        {
            _index = index;
            _button = gameObject.GetOrAddComponent<Button>();
            _button.onClick.AddListener(CellClicked);
            _row = (byte)(index / 3);
            _col = (byte)(index % 3);
        }

        private void CellClicked()
        {
            if (!GameManager.Instance.IsMyTurn || !GameManager.Instance.InputEnabled)
            {
                Debug.Log("Not my turn!");
                return;
            }

            _button.interactable = false;
            GameManager.Instance.InputEnabled = false;

            Debug.Log("Sending MarkCellRequest to server!");

            var msg = new Net_MarkCellRequest
            {
                Index = _index,
            };

            NetworkClient.Instance.SendServer(msg);
        }

        public void UpdateUI(string player)
        {
            var playerType = GameManager.Instance.ActiveGame.GetPlayerType(player);

            if (playerType == MarkType.X)
            {
                _xImg.gameObject.SetActive(true);
                LeanTween.scale(_xImg.gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.5f).setEase(LeanTweenType.easeOutBounce);
            }
            else
            {
                _oImg.gameObject.SetActive(true);
                LeanTween.scale(_oImg.gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.5f).setEase(LeanTweenType.easeOutBounce);
            }

            _button.interactable = false;
        }
    } 
}
