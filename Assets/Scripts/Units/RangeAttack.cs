using System.Collections;
using System.Linq;
using TowerDeffence.Interfaces;
using TowerDeffence.Utilities;
using UnityEngine;

namespace TowerDeffence.AI
{
    public class RangeAttack : AttackController
    {
        private StateMachineContext context;

        public override GameObject GetClosestTarget()
        {
            if (Building.AvailableBuidings == null || !Building.AvailableBuidings.Any())
                return null;

            return Building.AvailableBuidings.FirstOrDefault(b => b.IsTarget)?.gameObject;
        }

        public override void DoAttack()
        {
            var targetTransform = context.Owner.currentTarget;
            if (targetTransform == null) return;

            Debug.Log($"[{name}] Attacking {targetTransform.name} with a ranged attack!");

            if (targetTransform.TryGetComponent(out IDamagable damagable))
            {
                damagable.DoDamage(attackSO.Damage);
            }
        }

        public override void Enter(StateMachineContext context)
        {
            base.Enter(context);
            this.context = context;
        }

        public override void Execute(StateMachineContext context)
        {
            base.Execute(context);
        }

        public override void Exit(StateMachineContext context)
        {
            base.Exit(context);
            this.context = null;
        }
    }
}