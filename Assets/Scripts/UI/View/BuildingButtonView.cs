using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace TowerDeffence.UI.View
{
    public class BuildingButtonView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Building _buildPrefab;
        private BuildingPlacer buildingPlacer;
        
        [Header("Colors")]
        public Color normalColor = Color.white;
        public Color selectedColor = Color.green;

        private Button button;
        private Image buttonImage;

        private void Awake()
        {
            buttonImage = gameObject.GetComponent<Image>();
        }

        private void Start()
        {
            buildingPlacer = FindObjectOfType<BuildingPlacer>();
        }

        public void SetSelected(bool isSelected)
        {
            buttonImage.color = isSelected ? selectedColor : normalColor;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_buildPrefab != null && buildingPlacer != null)
            {
                buildingPlacer.StartPlacing(_buildPrefab);
                SetSelected(true);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (buildingPlacer != null)
                buildingPlacer.UpdatePlacingPosition();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (buildingPlacer != null)
            {
                buildingPlacer.FinalizePlacement();
                SetSelected(false);
            }
        }
    }
}