using System;
using TowerDeffence.AI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private LayerMask buildingLayer;

    private Building currentPreview;
    private Building prefabToPlace;

    private bool _isValidPosition;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    public void StartPlacing(Building prefab)
    {
        CancelPlacing();

        prefabToPlace = prefab;
        currentPreview = Instantiate(prefabToPlace);
        currentPreview.SetPreviewMode(true);
    }

    public void UpdatePlacingPosition()
    {
        if (currentPreview == null) return;
        
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Vector3 placePos;
        bool isValidPosition = true;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, ~buildingLayer))
        {
            placePos = hit.point;
            GameObject hitObject = hit.transform.gameObject;
            
            if (IsLayerInMask(placementLayer, hitObject.layer))
            {
                placePos = new Vector3(hitObject.transform.position.x, 
                    placePos.y, hitObject.transform.position.z);
            }
            else
            {
                isValidPosition = false;
            }
        }
        else
        {
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (!groundPlane.Raycast(ray, out float enter)) return;
            placePos = ray.GetPoint(enter);
            isValidPosition = false;
        }

        currentPreview.transform.position = placePos;

        Collider previewCollider = currentPreview.GetComponentInChildren<Collider>();
        
        bool hasOverlap = IsBlockedByBuilding(currentPreview.transform.position, previewCollider);
        _isValidPosition = !hasOverlap && isValidPosition;
        
        currentPreview.SetPreviewValid(_isValidPosition);
    }

    public void FinalizePlacement()
    {
        if (currentPreview == null) return;

        Collider previewCollider = currentPreview.GetComponentInChildren<Collider>();
        if (!_isValidPosition)
        {
            Debug.Log("Cannot place building here — something's in the way.");
            CancelPlacing();
            return;
        }

        currentPreview.SetPreviewMode(false);
        currentPreview.Place();
        
        currentPreview = null;
        prefabToPlace = null;
    }

    public void CancelPlacing()
    {
        if (currentPreview != null)
            Destroy(currentPreview.gameObject);

        currentPreview = null;
        prefabToPlace = null;
    }

    private bool IsBlockedByBuilding(Vector3 position, Collider previewCollider)
    {
        Vector3 size = previewCollider.bounds.size;
        Collider[] overlapping = Physics.OverlapBox(position, size / 2, Quaternion.identity, buildingLayer);

        foreach (var col in overlapping)
        {
            if (!col.transform.gameObject.Equals(previewCollider.transform.gameObject))
                return true;
        }

        return false;
    }
    
    public bool IsLayerInMask(LayerMask mask, int layerId)
    {
        return ((1 << layerId) & mask) != 0;
    }
}