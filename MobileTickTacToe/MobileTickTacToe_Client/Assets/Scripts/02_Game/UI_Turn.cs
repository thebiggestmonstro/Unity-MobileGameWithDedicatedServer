using Assets.Scripts.Game;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace MobileTickTacToe_Client.Game
{
    public class UI_Turn : MonoBehaviour
    {
        [SerializeField]
        Color _xUserNameColor;
        [SerializeField]
        Color _yUserNameColor;
        private TextMeshProUGUI _playerText;

        private void Awake()
        {
            _playerText = transform.Find("Txt_PlayerTurn").GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            var myColor = GameManager.Instance.MyType == NetworkShared.Models.MarkType.X ? _xUserNameColor : _yUserNameColor;
            var opponentColor = GameManager.Instance.OpponentType == NetworkShared.Models.MarkType.X ? _xUserNameColor : _yUserNameColor;

            if (GameManager.Instance.IsMyTurn)
            {
                _playerText.text = "your";
                _playerText.color = myColor;
                var rt = _playerText.GetOrAddComponent<RectTransform>();
                rt.sizeDelta = new Vector2(82f, rt.sizeDelta.y);
            }
            else
            {
                _playerText.text = "enemy";
                _playerText.color = opponentColor;
                var rt = _playerText.GetOrAddComponent<RectTransform>();
                rt.sizeDelta = new Vector2(120f, rt.sizeDelta.y);
            }

            LeanTween.scale(gameObject, new Vector3(1.1f, 1.1f, 1.1f), 0.3f)
                .setEase(LeanTweenType.easeOutBounce)
                .setOnComplete(() =>
                {
                    LeanTween.scale(gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.3f)
                    .setEase(LeanTweenType.easeOutBounce);
                });
        }
    }
}