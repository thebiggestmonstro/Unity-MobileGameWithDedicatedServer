using Assets.Scripts.Game;
using NetworkShared.Packets.ClientToServer;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
                Debug.Log("Not My Turn!!!");
                return;
            }

            _button.interactable = false;
            GameManager.Instance.InputEnabled = false;

            Debug.Log("Sending MarkCellRequset to Server");

            var msg = new Net_MarkCellRequest();
            NetworkClient.Instance.SendServer(msg);
        }
    } 
}
