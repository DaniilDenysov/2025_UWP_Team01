using System.Collections;
using System.Collections.Generic;
using TowerDeffence.Buildings;
using UnityEngine;

public class UpgradeCommand : ICommand
{
    private Tower building;
    private Updatable update;
    private float value;

    public UpgradeCommand(Tower building, Updatable update, float value)
    {
        this.building = building;
        this.update = update;
        this.value = value;
    }
    public bool Execute()
    {
        switch (update)
        {
            case Updatable.Range:
                building.range += value;
                break;
            case Updatable.Rate:
                building.rate += value;
                break;
            case Updatable.Damage:
                building.damage += value;
                break;
            default:
                return false;
        }
        Debug.Log("Upgreaded");
        return true;
    }

    public void Undo()
    {
        switch (update)
        {
            case Updatable.Range:
                building.range -= value;
                break;
            case Updatable.Rate:
                building.rate -= value;
                break;
            case Updatable.Damage:
                building.damage -= value;
                break;
        }
        Debug.Log("Deupgreaded");
    }
}

public enum Updatable
{
    Range,
    Rate,
    Damage
}
