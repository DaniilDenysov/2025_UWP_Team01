using Buildings;

namespace TowerDeffence.ObjectPools
{
    public class ProjectileObjectPool : ObjectPoolWrapper<Projectile>
    {
        public override void OnReturnedToPool(Projectile obj)
        {
            base.OnReturnedToPool(obj);
            obj.OnObjectReturnedToPool();
        }
    }
}
