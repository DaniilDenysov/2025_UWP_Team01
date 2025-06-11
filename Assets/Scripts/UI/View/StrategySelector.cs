using TowerDeffence.AI;
using TowerDeffence.Buildings;
using TowerDeffence.Buildings.Strategies;
using UnityEngine;

namespace TowerDeffence.UI
{
    public class StrategySelector : MonoBehaviour
    {
        [SerializeReference, SubclassSelector] private AttackStrategy attackStrategy;

        public AttackStrategy AttackStrategy => attackStrategy;


        [SerializeField] private Tower tower;

        public void OnSelected(AttackStrategy sellectedAttackStrategy)
        {
            if (sellectedAttackStrategy is IAttackStrategyHandler<EnemyMovement> typed)
            {
                attackStrategy = sellectedAttackStrategy;
                tower.SetStrategySelector(typed);
            }
        }
    }
}
