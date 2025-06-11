using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private static List<GameObject> towerPopUpMenus = new List<GameObject>();

    private void Start()
    {
        if (target == null) return;
        towerPopUpMenus.Add(target);
    }
    public void Toggle()
    {
        if (target.activeSelf)
            target.SetActive(false);
        else
        {
            target.SetActive(true);
        }
    }
 
}
