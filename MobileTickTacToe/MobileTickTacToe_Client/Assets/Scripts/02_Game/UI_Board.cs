using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MobileTickTacToe_Client.Game 
{ 
    public class UI_Board : MonoBehaviour
    {
        [SerializeField]
        GameObject _boardCellPrefab;

        private Dictionary<int, UI_BoardCell> _cells;

        private void Start()
        {
            ResetBoard();
        }

        private void ResetBoard()
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            _cells = new Dictionary<int, UI_BoardCell>();

            for (int i = 0; i < 9; i++)
            {
                var cell = Instantiate(_boardCellPrefab, transform).GetOrAddComponent<UI_BoardCell>();
                cell.Init((byte)i);
                _cells.Add(i, cell);
            }
        }
    }
}