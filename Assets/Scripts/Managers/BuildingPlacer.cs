using System;
using TowerDeffence.AI;
using TowerDeffence.ObjectPools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zenject;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private LayerMask buildingLayer;
    [SerializeField] private Grid grid;
    private Building currentPreview;
    private Building prefabToPlace;
    private bool _isValidPosition;
    public Action<Vector3, bool> OnUpdatePlacement;
    public Action OnEndedPlacement;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    public void StartPlacing(IPlacable prefab)
    {
        OnEndedPlacement?.Invoke();

        prefab.OnStartedPlace();
        prefab.SubscribeToPlacing(this);
    }

    public void UpdatePlacingPosition() {
        if (OnUpdatePlacement == null) return;
        
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Vector3 placePos;
        bool isValidPosition = true;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, ~buildingLayer))
        {
            Vector3Int cell = grid.WorldToCell(hit.point);
            placePos = grid.CellToWorld(cell);
            if (!IsLayerInMask(placementLayer, hit.collider.gameObject.layer)) 
                isValidPosition = false;
        }
        else
        {
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (!groundPlane.Raycast(ray, out float enter)) return;
            placePos = ray.GetPoint(enter);
            isValidPosition = false;
        }
        
        OnUpdatePlacement.Invoke(placePos, isValidPosition);
    }

    public void FinalizePlacement()
    {
        if (OnEndedPlacement == null) return;
        OnEndedPlacement?.Invoke();
    }
    
    public bool IsLayerInMask(LayerMask mask, int layerId)
    {
        return ((1 << layerId) & mask) != 0;
    }
}