using System.Collections;
using System.Linq;
using TowerDeffence.Interfaces;
using TowerDeffence.Utilities;
using UnityEngine;

namespace TowerDeffence.AI
{
    public class RangeAttack : AttackController
    {
        private GameObject target;

        private IEnumerator Start()
        {
            while (true)
            {
                DoAttack();
                yield return new WaitForSeconds(attackSO.Rate);
            }
        }

        public override void DoAttack()
        {
            if (target == null || target != null && !target.activeInHierarchy)
            {
                var enemy = GetClosestEnemy();
                if (enemy == null)
                {
                    DebugUtility.PrintLine("No enemies found!");
                    return;
                }
                if (Vector3.Distance(enemy.transform.position, transform.position) > attackSO.Range) return;
                target = enemy;
                if (enemy.TryGetComponent(out IDamagable damagable))
                {
                    //TODO: [DD] add more complex logic with launching projectile
                    damagable.DoDamage(attackSO.Damage);
                }
            }
            else
            {
                if (target.TryGetComponent(out IDamagable damagable))
                {
                    //TODO: [DD] add more complex logic with launching projectile
                    damagable.DoDamage(attackSO.Damage);
                }
            }
        }

        public override GameObject GetClosestEnemy()
        {
            if (Building.AvailableBuidings == null || !Building.AvailableBuidings.Any())
                return null;

            return Building.AvailableBuidings
                           .OrderBy(b => Vector3.Distance(b.transform.position, transform.position))
                           .FirstOrDefault()?.gameObject;
        }
    }
}
