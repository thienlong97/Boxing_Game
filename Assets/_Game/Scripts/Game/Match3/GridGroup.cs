using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
    public class GridGroup : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GridLayoutGroup gridLayout;
        private List<Cell> listAllCell = new List<Cell>();

        private List<Cell> listCell = new();
        public List<Cell> ListCell => listCell;

        private Match3LevelConfig dataConfig;
        private GridSort gridSort;

        public void Initialized()
        {
            listAllCell = GetComponentsInChildren<Cell>().ToList();
        }

        public void GenerateGrid()
        {
            gridLayout.enabled = true;
            Transform gridParent = gridLayout.transform;
            dataConfig = Match3LevelSO.Instance.ListMath3LevelConfig[0];

            listCell.Clear();
            foreach (var cell in listAllCell)
                cell.gameObject.SetActive(false);

            // setup grid layout
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = dataConfig.gridWidth;

            // calculate cell size
            RectTransform gridRect = gridLayout.GetComponent<RectTransform>();
            float cellWidth = gridRect.rect.width / dataConfig.gridWidth;
            float cellHeight = gridRect.rect.height / dataConfig.gridHeight;
            gridLayout.cellSize = new Vector2(cellWidth, cellWidth);

            // resize gridRect
            float spacing = gridLayout.spacing.x;
            float totalWidth = cellWidth * dataConfig.gridWidth + spacing * (dataConfig.gridWidth - 1);
            float totalHeight = cellWidth * dataConfig.gridHeight + spacing * (dataConfig.gridHeight - 1);
            gridRect.sizeDelta = new Vector2(totalWidth, totalHeight);

            for (int y = 0; y < dataConfig.gridHeight; y++)
            {
                for (int x = 0; x < dataConfig.gridWidth; x++)
                {
                    Vector2Int _position = new Vector2Int(x, y);
                    SpawnCell(_position);
                }
            }

            gridSort = new GridSort(this, gridLayout, dataConfig, ListCell);
        }

        public void SortGrid()
        {
            gridSort.SortGridWithAnimation();
        }

        public Cell SpawnCell(Vector2Int _position)
        {
            Cell cell = GetNewCell();
            if (!cell) Debug.LogError("GetNewCell == null");
            int _randomColor = Random.Range(0, dataConfig.colorMax);
            CellConfig _newCellConfig = Match3LevelSO.Instance.GetRandomNewCellConfig;
            cell.Setup(_newCellConfig, _position);
            listCell.Add(cell);
            return cell;
        }
        
        private Cell GetNewCell()
        {
            foreach (var cell in listAllCell)
            {
                if (cell.gameObject.activeSelf) continue;
                return cell;
            }
            return null;
        }
        public Vector2 GetCellPosition(int x, int y)
        {
            RectTransform gridRect = gridLayout.GetComponent<RectTransform>();
            float cellWidth = gridLayout.cellSize.x;
            float cellHeight = gridLayout.cellSize.y;
            Vector2 gridPos = Vector2.zero;
            float posX = gridPos.x + x * (cellWidth + gridLayout.spacing.x) + (0.5f * cellWidth);
            float posY = gridPos.y - y * (cellHeight + gridLayout.spacing.y) - (0.5f * cellHeight);
            return new Vector2(posX, posY);
        }
    }

}
