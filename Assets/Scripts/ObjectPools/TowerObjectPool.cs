using TowerDeffence.HealthSystem;
using TowerDeffence.ObjectPools;

namespace TowerDeffence.Buildings
{
    public class TowerObjectPool : ObjectPoolWrapper<Building>
    {
        public override void OnTakenFromPool(Building obj)
        {
            base.OnTakenFromPool(obj);
            if (obj.TryGetComponent(out HealthPresenter healthPresenter))
            {
               healthPresenter.ResetHealth();
            }
        }
    }   
}
