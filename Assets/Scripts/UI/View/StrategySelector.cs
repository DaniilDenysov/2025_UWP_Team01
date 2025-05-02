using Buildings;
using TowerDeffence.AI;
using TowerDeffence.Buildings.Strategies;
using UnityEngine;

namespace TowerDeffence.UI
{
    public class StrategySelector : MonoBehaviour
    {
        [SerializeReference, SubclassSelector] private AttackStrategy attackStrategy;

        public AttackStrategy AttackStrategy
        {
            get => attackStrategy;
        }

        [SerializeField] private Tower tower;

        public void OnSelected()
        {
            if (attackStrategy is IAttackStrategyHandler<EnemyMovement> typed)
            {
                tower.SetStrategySelector(typed);
            }
        }
    }
}
