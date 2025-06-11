using System.Collections;
using System.Collections.Generic;
using TowerDeffence.HealthSystem;
using UnityEngine;

public class TownHallPresenter : HealthPresenter
{
    [SerializeField] BackgroundMusicChanger changer;

    private void Start()
    {
        model.UpdateModel();
        model.OnHealthThreshold += HandleHpThreshold;
    }

    private void OnDestroy()
    {
        if (model != null)
        {
            model.OnHealthThreshold -= HandleHpThreshold;
        }
    }

    private void HandleHpThreshold(int index)
    {
        if (!changer) return;
        changer.ChangeBackgroundMusic(index);
    }
}
