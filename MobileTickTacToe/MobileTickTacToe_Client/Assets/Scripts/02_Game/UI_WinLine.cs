using NetworkShared.Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using MobileTickTacToe_Client.PacketHandlers;
using NetworkShared.Packets.ServerToClient;
using Assets.Scripts.Game;
using System.Collections;

namespace MobileTickTacToe_Client.Game
{
    public class LineConfig
    { 
        public float X { get; set; }
        public float Y { get; set; }
        public float Height { get; set; }
        public float ZRotation { get; set; }
    }

    public class UI_WinLine : MonoBehaviour
    {
        [SerializeField]
        Color _xColor;
        [SerializeField]
        Color _yColor;

        private Image _image;
        private RectTransform _rectTransform;
        private Dictionary<WinLineType, LineConfig> _lineConfigs;

        private void Start()
        {
            _image = gameObject.GetOrAddComponent<Image>();
            _rectTransform = gameObject.GetOrAddComponent<RectTransform>();

            OnMarkCellHandler.OnMarkCell += HandleMarkCell;
            OnNewRoundHandler.OnNewRound += ResetLine;

            _lineConfigs = InitLineConfigs();
        }

        private void OnDestroy()
        {
            OnMarkCellHandler.OnMarkCell -= HandleMarkCell;
        }

        private void HandleMarkCell(Net_OnMarkCell msg)
        {
            if (msg.Outcome == MarkOutcome.Win)
            {
                _image.enabled = true;
                var config = _lineConfigs[msg.WinLineType];
                SetupLine(config, msg.PlayerName);

                StopCoroutine(AnimateLine());
                StartCoroutine(AnimateLine());
            }
        }

        IEnumerator AnimateLine()
        {
            yield return new WaitForSeconds(0.5f);

            while (_image.fillAmount < 1f)
            {
                _image.fillAmount += 2f * Time.deltaTime;
                yield return null;
            }
        }

        private void SetupLine(LineConfig config, string playerId = default)
        {
            Color color = _xColor;

            if (!string.IsNullOrEmpty(playerId))
            {
                var type = GameManager.Instance.ActiveGame.GetPlayerType(playerId);
                color = type == MarkType.X ? _xColor : _yColor;
            }

            _rectTransform.localPosition = new Vector2(config.X, config.Y);
            _rectTransform.localRotation = Quaternion.Euler(0, 0, config.ZRotation);
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, config.Height);
            _image.color = color;
        }

        private Dictionary<WinLineType, LineConfig> InitLineConfigs()
        {
            return new Dictionary<WinLineType, LineConfig>
            {
                { WinLineType.None, new LineConfig() },
                { WinLineType.Diagonal, new LineConfig() { Height = 370f,  ZRotation = 45f, X = 0f, Y = 0f } },
                { WinLineType.AntiDiagonal, new LineConfig() { Height = 370f,  ZRotation = -45f, X = 0f, Y = 0f } },
                { WinLineType.ColLeft, new LineConfig() { Height = 290f,  ZRotation = 0f, X = -94f, Y = 0f } },
                { WinLineType.ColMid, new LineConfig() {  Height = 290f,  ZRotation = 0f, X = 0f, Y = 0f } },
                { WinLineType.ColRight, new LineConfig() {  Height = 290f,  ZRotation = 0f, X = 94f, Y = 0f } },
                { WinLineType.RowTop, new LineConfig() {  Height = 290f,  ZRotation = 90f, X = 0, Y = 94f } },
                { WinLineType.RowMiddle, new LineConfig() {  Height = 290f,  ZRotation = 90f, X = 0, Y = 0f } },
                { WinLineType.RowBottom, new LineConfig() {  Height = 290f,  ZRotation = 90f, X = 0, Y = -94f } },
            };
        }

        private void ResetLine()
        {
            _image.enabled = false;
            _image.fillAmount = 0;
            SetupLine(_lineConfigs[WinLineType.ColMid]);
        }
    }
}