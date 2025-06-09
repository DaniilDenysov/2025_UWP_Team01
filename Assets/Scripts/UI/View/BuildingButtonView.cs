using System;
using TMPro;
using TowerDeffence.AI;
using TowerDeffence.ObjectPools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Zenject;
using Image = UnityEngine.UI.Image;

namespace TowerDeffence.UI.View
{
    public class BuildingButtonView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Building _buildPrefab;
        private BuildingPlacer _buildingPlacer;
        
        [Header("Colors")]
        public Color normalColor = Color.white;
        public Color selectedColor = Color.green;
        private IObjectPool<Building> objectPool;
        private Button button;
        private Image buttonImage;

        private void Awake()
        {
            buttonImage = gameObject.GetComponent<Image>();
        }

        private void Start()
        {
            _buildingPlacer = FindObjectOfType<BuildingPlacer>();
        }

        public void SetSelected(bool isSelected)
        {
            buttonImage.color = isSelected ? selectedColor : normalColor;
        }

        [Inject]
        private void Construct(IObjectPool<Building> objectPool)
        {
            this.objectPool = objectPool;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_buildPrefab != null && _buildingPlacer != null)
            {
                IPlacable newBuildingInstance = objectPool.GetObject(_buildPrefab);
                _buildingPlacer.StartPlacing(newBuildingInstance);
                SetSelected(true);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_buildingPlacer != null)
                _buildingPlacer.UpdatePlacingPosition();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_buildingPlacer != null)
            {
                _buildingPlacer.FinalizePlacement();
                SetSelected(false);
            }
        }
    }
}