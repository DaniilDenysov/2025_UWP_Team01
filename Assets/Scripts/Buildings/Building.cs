using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IPrototype
{
    public static List<Building> Available = new List<Building>();
    private Renderer[] renderers;
    
    private Collider previewCollider;
    private Vector3 boundsExtents;
    
    private void Awake()
    {
        previewCollider = GetComponentInChildren<Collider>();
        boundsExtents = previewCollider.bounds.extents;
        renderers = GetComponentsInChildren<Renderer>();
    }
    
    public void Place(Vector3 position)
    {
        
    }

    public GameObject Copy()
    {
        throw new System.NotImplementedException();
    }
    
    public void SetPreviewMode(bool isPreview)
    {
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
        Color validColor = new Color(0f, 1f, 0f, 0.5f); // green
        Color invalidColor = new Color(1f, 0f, 0f, 0.5f); // red

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
}
