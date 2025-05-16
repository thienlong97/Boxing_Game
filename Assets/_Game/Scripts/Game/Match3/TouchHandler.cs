using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3
{
    public class LineHandler
    {
        private LineRenderer lineRenderer;

        public LineHandler(LineRenderer _lineRenderer)
        {
            lineRenderer = _lineRenderer;
        }

        public void UpdateLineRenderer(List<Cell> selectedCells)
        {
            if (selectedCells == null || lineRenderer == null) return;

            if (selectedCells.Count == 0)
            {
                lineRenderer.positionCount = 0;
                return;
            }

            lineRenderer.positionCount = selectedCells.Count;
            for (int i = 0; i < selectedCells.Count; i++)
            {
                Vector3 _point = selectedCells[i].transform.position;
                _point.z -= 0.01f;
                lineRenderer.SetPosition(i,_point);
            }

            lineRenderer.material.color = selectedCells[0].CellConfig.colorHighlight;
        }

        public void ClearLine()
        {
            lineRenderer.positionCount = 0;
        }
    }

    [RequireComponent(typeof(LineRenderer))]
    public class TouchHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [Header("References")]
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private GridGroup gridGroup;
        private List<Cell> selectedCells = new List<Cell>();
        private bool isDragging = false;
        private LineHandler lineHandler;

        public void Initialized()
        {
            lineHandler = new LineHandler(lineRenderer);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isDragging = true;
            UpdateSelectedCell(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            lineHandler.ClearLine();
            DoSelectedCells();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging) return;

            UpdateSelectedCell(eventData);
        }

        private void DoSelectedCells()
        {
            if (selectedCells.Count >= 3)
            {
                // combo
                int _count = selectedCells.Count;
                CellConfig.CellType _type = selectedCells[0].CellConfig.cellType;
                Match3Manager.onMatch.Invoke(_type, _count);

                foreach (var cell in selectedCells)
                {
                    cell.ClearCell();
                }

                // Sort Grid
                gridGroup.SortGrid();
            }
            else
            {
                for (int i = 0; i < selectedCells.Count; i++)
                    selectedCells[i].SetHighlight(false);
            }

            isDragging = false;
            selectedCells.Clear();
            lineHandler.ClearLine();
        }

        private void UpdateSelectedCell(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (var result in results)
            {
                Cell cell = result.gameObject.GetComponent<Cell>();
                if (cell == null) continue;

                if (selectedCells.Contains(cell))
                {
                    for(int i = selectedCells.Count - 1; i >= 0; i--)
                    {
                        Cell _curCell = selectedCells[i];
                        if (_curCell != cell)
                        {
                            _curCell.SetHighlight(false);
                            selectedCells.Remove(_curCell);
                            lineHandler.UpdateLineRenderer(selectedCells);
                            i = selectedCells.Count;
                        }
                        else break;

                    }
                    continue;
                }

                if (selectedCells.Count == 0 ||
                    (cell.IsSameColor(selectedCells[0]) && IsNeighbor(cell)))
                {
                    selectedCells.Add(cell);
                    cell.SetHighlight(true);
                    lineHandler.UpdateLineRenderer(selectedCells);
                }
            }
        }

        private bool IsNeighbor(Cell targetCell)
        {
            if (selectedCells.Count == 0) return true;
            Cell lastCell = selectedCells[selectedCells.Count - 1];
            Vector2Int lastPos = lastCell.GridPosition;
            Vector2Int targetPos = targetCell.GridPosition;
            int dx = Mathf.Abs(lastPos.x - targetPos.x);
            int dy = Mathf.Abs(lastPos.y - targetPos.y);
            return (dx <= 1 && dy <= 1);
        }
    }
}
