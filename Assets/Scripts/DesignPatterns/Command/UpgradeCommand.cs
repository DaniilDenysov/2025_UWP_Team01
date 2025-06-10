using System.Collections;
using System.Collections.Generic;
using TowerDeffence.Buildings;
using UnityEngine;

public class UpgradeCommand : ICommand
{
    private Tower building;
    private Updatable update;
    private float value;
    private List<Material> materials;
    private static int index = 0;

    public UpgradeCommand(Tower building, Updatable update, float value, List<Material> materials)
    {
        this.building = building;
        this.update = update;
        this.value = value;
        this.materials = materials;
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
        if (building.gameObject.TryGetComponent<MeshRenderer>(out var renderer))
        {
            index++;
            renderer.SetMaterials(new List<Material>() { materials[index] });
            
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
        if (building.gameObject.TryGetComponent<MeshRenderer>(out var renderer))
        {
            index--;
            renderer.SetMaterials(new List<Material>() { materials[index] });
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
