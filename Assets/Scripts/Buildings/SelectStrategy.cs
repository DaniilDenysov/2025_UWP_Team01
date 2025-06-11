using System.Collections;
using System.Collections.Generic;
using TowerDeffence.Buildings.Strategies;
using TowerDeffence.UI;
using UnityEngine;

public class SelectStrategy : MonoBehaviour
{
    [SerializeField] private StrategySelector strategy;

    public void HandleInputData(int val)
    {
        switch (val)
        {
            case 0:
                strategy.OnSelected(new ClosestEnemy());
                break;

            case 1:
                strategy.OnSelected(new LowestHealthOpponent());
                break;
        }
    }
}
