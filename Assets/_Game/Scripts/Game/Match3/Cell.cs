using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
    public class Cell : MonoBehaviour
    {
        [Header("References")]
        public Image dotImg;
        public Image iconImg;
        public Image highlightImg;
        private RectTransform rect;

        private CellConfig cellConfig;
        public CellConfig CellConfig => cellConfig;

        private Vector2Int gridPosition;
        public Vector2Int GridPosition => gridPosition;

        public void Setup(CellConfig _cellConfig, Vector2Int _position)
        {
            cellConfig = _cellConfig;
            dotImg.color = cellConfig.colorType;
            iconImg.sprite = cellConfig.iconSprite;
            highlightImg.color = cellConfig.colorHighlight;
            SetNewGridPosition(_position);
            SetHighlight(false);
            gameObject.SetActive(true);    
        }

        public void SetNewGridPosition(Vector2Int _position)
        {
            gridPosition = _position;

        }

        public void ClearCell()
        {
            gameObject.SetActive(false);
        }

        public void SetHighlight(bool _isActive)
        {
            highlightImg.enabled = _isActive;
        }

        public bool IsSameColor(Cell other)
        {
            return cellConfig.colorType == other.cellConfig.colorType;
        }

        public bool IsPointInside(Vector2 point)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(rect, point);
        }
    }
}

