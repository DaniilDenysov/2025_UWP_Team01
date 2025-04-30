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
    public abstract class AttackStrategy<T> where T : Object
    {
        protected Transform myPosition;
        public AttackStrategy(Transform myPosition)
        {
            this.myPosition = myPosition;
        }
        public abstract T GetEnemy(List<T> enemies);
    }

    [System.Serializable]
    public class LowestHealthOpponent : AttackStrategy<HealthPresenter>
    {
        public LowestHealthOpponent(Transform myPosition) : base(myPosition)
        {

        }

        public override HealthPresenter GetEnemy(List<HealthPresenter> enemies)
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
    }

    [System.Serializable]
    public class ClosestEnemy : AttackStrategy<EnemyMovement>
    {
        public ClosestEnemy(Transform myPosition) : base(myPosition)
        {

        }

        public override EnemyMovement GetEnemy(List<EnemyMovement> enemies)
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
    }

    [System.Serializable]
    public class ClosestTower : AttackStrategy<Building>
    {
        public ClosestTower(Transform myPosition) : base(myPosition)
        {

        }

        public override Building GetEnemy(List<Building> buildings)
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
    }
}
