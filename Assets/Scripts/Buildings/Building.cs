using System.Collections.Generic;
using DesignPatterns.Singleton.Command;
using TowerDeffence.AI;
using TowerDeffence.Interfaces;
using TowerDeffence.ObjectPools;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class Building : MonoBehaviour, IPrototype<Building>, IPlacable
{
    [SerializeField] private int price;
    [SerializeField] public UnityEvent OnTowerPlaced;

    public static List<Building> AvailableBuidings = new List<Building>();
    private Renderer[] renderers;
    
    private Collider previewCollider;
    private Vector3 boundsExtents;
    protected EconomyManager _economyManager;
    
    private IObjectPool<Building> objectPool;
    [SerializeField] private UnityEvent<bool> _onInteracted;
    protected CommandContainer commandContainer;
    
    private bool _isPositionValid;
    protected bool isPreviewMode;

    private BuildingPlacer _buildingPlacer;
    private bool isSelected = false;
    
    private void Awake()
    {
        previewCollider = GetComponentInChildren<Collider>();
        boundsExtents = previewCollider.bounds.extents;
        renderers = GetComponentsInChildren<Renderer>();
    }
    
    [Inject]
    private void Construct(CommandContainer commandContainer)
    {
        this.commandContainer = commandContainer;
    }

    [Inject]
    private void Construct(IObjectPool<Building> objectPool)
    {
        this.objectPool = objectPool;
    }

    [Inject]
    private void Construct(EconomyManager economyManager)
    {
        Debug.Log("CONSTRUCTED!");
        _economyManager = economyManager;
    }

    private void Start()
    {
        OnTowerPlaced.Invoke();
    }

    private void OnEnable()
    {
        AvailableBuidings.Add(this);
    }

    private void OnDisable()
    {
        isSelected = false;        
        _onInteracted.Invoke(isSelected);
        AvailableBuidings.Remove(this);
    }

    public virtual bool Place()
    {
        if (_economyManager.CanAfford(price) && !IsBlockedByBuilding())
        {
            _economyManager.Reduce(price);
            return true;
        }

        return false;
    }

    public virtual void Withdraw()
    {
        _economyManager.Add(price);
    }

    public virtual void Remove()
    {
        Withdraw();
        objectPool.ReleaseObject(this);
    }

    public virtual void Sell()
    {
        commandContainer.ExecuteCommand(new SellBuildingCommand(this));
    }


    public void SetPreviewMode(bool isPreview)
    {
        isPreviewMode = isPreview;
        
        foreach (var col in GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
            col.isTrigger = isPreview;
        }
    
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in renderer.materials)
            {
                if (isPreview)
                {
                    SetMaterialTransparent(mat);
                    mat.color = new Color(0f, 1f, 0f, 0.5f);
                }
                else
                {
                    SetMaterialOpaque(mat);
                    mat.color = Color.white;
                }
            }
        }
    }
    
    public void SetPreviewValid(bool isValid)
    {
        Color validColor = new Color(0f, 1f, 0f, 0.5f);
        Color invalidColor = new Color(1f, 0f, 0f, 0.5f);

        foreach (var renderer in renderers)
        {
            foreach (var mat in renderer.materials)
            {
                mat.color = isValid ? validColor : invalidColor;
            }
        }
    }
    
    private void SetMaterialTransparent(Material mat)
    {
        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
    }

    private void SetMaterialOpaque(Material mat)
    {
        mat.SetFloat("_Mode", 0);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = -1;
    }

    public static IReadOnlyList<Building> getAllBuildings()
    {
        return AvailableBuidings.AsReadOnly();
    }

    public void SubscribeToPlacing(BuildingPlacer buildingPlacer)
    {
        _buildingPlacer = buildingPlacer;
        
        _buildingPlacer.OnUpdatePlacement += OnPlacing;
        _buildingPlacer.OnEndedPlacement += OnEndedPlace;
    }

    public void UnsubscribeToPlacing()
    {
        if (_buildingPlacer == null) return; 
        
        _buildingPlacer.OnUpdatePlacement -= OnPlacing;
        _buildingPlacer.OnEndedPlacement -= OnEndedPlace;
        
        _buildingPlacer = null;
    }

    public void OnStartedPlace()
    {
        SetPreviewMode(true);
    }

    public void OnPlacing(Vector3 position, bool isValid)
    {
        transform.position = position;
        _isPositionValid = isValid && !IsBlockedByBuilding();
        SetPreviewValid(isValid && !IsBlockedByBuilding());
    }

    public void OnEndedPlace()
    {
        if (!_isPositionValid)
        {
            Debug.Log("Cannot place building here — something's in the way.");
            Destroy(gameObject);
        }
        
        SetPreviewMode(false);
        commandContainer.ExecuteCommand(new PlaceBuildingCommand(this));
        UnsubscribeToPlacing();
    }

    private bool IsBlockedByBuilding()
    {
        Vector3 size = previewCollider.bounds.size;
        Collider[] overlapping = 
            Physics.OverlapBox(transform.position, size / 2, Quaternion.identity, gameObject.layer);

        foreach (var col in overlapping)
        {
            if (!col.transform.gameObject.Equals(previewCollider.transform.gameObject))
                return true;
        }

        return false;
    }

    private void OnDestroy()
    {
        UnsubscribeToPlacing();
        AvailableBuidings.Remove(this);
    } 
       
    public Building Copy()
    {
        return (Building) MemberwiseClone();
    }
        
    void OnMouseDown()
    {
        isSelected = !isSelected;
        _onInteracted.Invoke(isSelected);
    }
}
