using System.Collections;
using System.Collections.Generic;
using TowerDeffence.HealthSystem;
using TowerDeffence.UI.View;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystemView : View<Health>
{
    [SerializeField] private Slider slider;

    public override void UpdateView(Health model)
    {
        //TODO: [DD] add proper health bar
        slider.value = model.Current;
        Debug.Log($"Current HP:{model.Current}");
    }
}
