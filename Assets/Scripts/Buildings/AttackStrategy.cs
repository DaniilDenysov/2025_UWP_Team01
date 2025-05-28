using System.Collections;
using System.Collections.Generic;
using TowerDeffence.Utilities;
using UnityEngine;
using System.Linq;
using TowerDeffence.AI;
using TowerDeffence.HealthSystem;

namespace TowerDeffence.Buildings.Strategies
{
    [System.Serializable]
    public abstract class AttackStrategy
    {
        [SerializeField] public Transform myPosition;
    }

    public interface IAttackStrategyHandler
    {
        Object GetEnemy(List<Object> enemies);
    }


    public interface IAttackStrategyHandler<T> : IAttackStrategyHandler
    {
        public T GetEnemy(List<T> enemies);
    }

    [System.Serializable]
    public class LowestHealthOpponent : AttackStrategy, IAttackStrategyHandler<HealthPresenter>
    {
        public HealthPresenter GetEnemy(List<HealthPresenter> enemies)
        {
            if (enemies == null)
            {
                DebugUtility.PrintError("Null list is prohibited!");
                return null;
            }
            return enemies.OrderByDescending(obj =>
            {
                if (obj != null)
                {
                    return obj.GetCurrentHealthPoints();
                }
                return float.MinValue;
            }).LastOrDefault();
        }

        public Object GetEnemy(List<Object> enemies)
        {
            return GetEnemy(enemies);
        }
    }

    [System.Serializable]
    public class ClosestEnemy : AttackStrategy, IAttackStrategyHandler<EnemyMovement>
    {
        public EnemyMovement GetEnemy(List<EnemyMovement> enemies)
        {
            if (enemies == null)
            {
                DebugUtility.PrintError("Null list is prohibited!");
                return null;
            }
            return enemies.OrderByDescending(obj =>
            {
                if (obj != null)
                {
                    return Vector3.Distance(myPosition.position,obj.transform.position);
                }
                return float.MinValue;

            }).LastOrDefault();
        }

        public Object GetEnemy(List<Object> enemies)
        {
            return GetEnemy(enemies);
        }
    }

    [System.Serializable]
    public class ClosestTower : AttackStrategy, IAttackStrategyHandler<Building>
    {
        public Building GetEnemy(List<Building> buildings)
        {
            if (buildings == null)
            {
                DebugUtility.PrintError("Null list is prohibited!");
                return null;
            }
            return buildings.OrderByDescending(obj =>
            {
                if (obj != null)
                {
                    return Vector3.Distance(myPosition.position, obj.transform.position);
                }
                return float.MinValue;

            }).LastOrDefault();
        }

        public Object GetEnemy(List<Object> enemies)
        {
            return GetEnemy(enemies);
        }
    }
}
