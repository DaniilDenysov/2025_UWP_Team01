using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TowerDeffence.AI;
using TowerDeffence.Interfaces;
using UnityEngine;

public class Building : MonoBehaviour, IPrototype
{
    [SerializeField] private int price;
    public static List<Building> AvailableBuidings = new List<Building>();

    private Renderer[] renderers;
    
    private Collider previewCollider;
    private Vector3 boundsExtents;
    protected EconomyManager _economyManager;

    protected bool isPreviewMode;
    
    private void Awake()
    {
        previewCollider = GetComponentInChildren<Collider>();
        boundsExtents = previewCollider.bounds.extents;
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void Start()
    {
        _economyManager = EconomyManager.Instance;
    }

    private void OnEnable()
    {
        AvailableBuidings.Add(this);
    }

    private void OnDisable()
    {
        AvailableBuidings.Remove(this);
    }

    private void OnDestroy()
    {
        AvailableBuidings.Remove(this);
    }

    public virtual bool Place()
    {
        if (_economyManager.CanAfford(price))
        {
            _economyManager.Reduce(price);
            return true;
        }

        return false;
    }

    public virtual void Remove()
    {
        Destroy(this);
    }

    public virtual void Sell()
    {
        _economyManager.Add(price);
        Remove();
    }

    public GameObject Copy()
    {
        throw new System.NotImplementedException();
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
}
