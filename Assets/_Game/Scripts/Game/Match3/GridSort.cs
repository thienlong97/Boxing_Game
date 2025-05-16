using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
    public class GridSort
    {
        private GridGroup gridGroup;
        private GridLayoutGroup gridLayout;
        private Match3LevelConfig dataConfig;
        private List<Cell> listCell;

        public GridSort(GridGroup gridGroup, GridLayoutGroup gridLayout, Match3LevelConfig dataConfig, List<Cell> listCell)
        {
            this.gridGroup = gridGroup;
            this.gridLayout = gridLayout;
            this.dataConfig = dataConfig;
            this.listCell = listCell;
        }

        public void SortGridWithAnimation()
        {
            gridLayout.enabled = false;
            int width = dataConfig.gridWidth;
            int height = dataConfig.gridHeight;
            float animationDuration = 0.8f;

            for (int x = 0; x < width; x++)
            {
                List<Cell> columnCells = new List<Cell>();

                for (int y = 0; y < height; y++)
                {
                    Cell cell = listCell.FirstOrDefault(c => c.GridPosition.x == x && c.GridPosition.y == y);
                    if (cell != null && cell.gameObject.activeSelf)
                    {
                        columnCells.Add(cell);
                    }
                }

                columnCells.Sort((a, b) => b.GridPosition.y.CompareTo(a.GridPosition.y));

                for (int i = 0; i < columnCells.Count; i++)
                {
                    Vector2Int newPos = new Vector2Int(x, height - 1 - i);
                    Vector2 targetPos = gridGroup.GetCellPosition(newPos.x, newPos.y);
                    columnCells[i].SetNewGridPosition(newPos);

                    RectTransform rectTransform = columnCells[i].GetComponent<RectTransform>();
                    rectTransform.DOAnchorPos(targetPos, animationDuration).SetEase(Ease.OutBounce);
                }

                int missingCount = height - columnCells.Count;
                for (int i = 0; i < missingCount; i++)
                {
                    int spawnY = height + i;
                    Vector2Int targetPos = new Vector2Int(x, height - 1 - columnCells.Count - i);
                    SpawnNewFallingCell(x, spawnY, targetPos);
                }
            }
        }

        public void SpawnNewFallingCell(int x, int startY, Vector2Int targetPos)
        {
            Cell cell = gridGroup.SpawnCell(targetPos);
            RectTransform rectTransform = cell.GetComponent<RectTransform>();
            Vector2 endPos = gridGroup.GetCellPosition(targetPos.x, targetPos.y);
            Vector2 startPos = endPos + Vector2.up * (rectTransform.rect.height * 2);
            rectTransform.anchoredPosition = startPos;
            rectTransform.DOAnchorPos(endPos, 0.8f).SetEase(Ease.OutBounce);
            cell.SetNewGridPosition(targetPos);
        }
    }
}