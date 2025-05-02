using TowerDeffence.AI;
using TowerDeffence.HealthSystem;
using TowerDeffence.ObjectPools;

namespace TowerDeffence.Buildings
{
    public class UnitObjectPool : ObjectPoolWrapper<EnemyMovement>
    {

        public override void OnTakenFromPool(EnemyMovement obj)
        {
            base.OnTakenFromPool(obj);
            if (obj.TryGetComponent(out HealthPresenter healthPresenter))
            {
                healthPresenter.ResetHealth();
            }
        }
    }
}