using System.Collections;
using System.Collections.Generic;
using TowerDeffence.AI.Data;
using TowerDeffence.Interfaces;
using UnityEngine;

namespace TowerDeffence.AI
{
    public abstract class AttackController : MonoBehaviour, IEnemyState
    {
        [SerializeField] public AttackSO attackSO;
        private float lastAttackTime = 0;

        public abstract GameObject GetClosestTarget();
        public abstract void DoAttack();

        public virtual void Enter(StateMachineContext context)
        {
            context.Owner.GetComponent<EnemyMovement>().StopAgent();
        }

        public virtual void Execute(StateMachineContext context)
        {
            var target = context.Owner.currentTarget;
            if (target == null)
            {
                context.Owner.RequestStateChange(context.MovementState);
                return;
            }

            transform.LookAt(target.transform);

            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance > attackSO.Range)
            {
                context.Owner.RequestStateChange(context.MovementState);
                return;
            }

            if (Time.time > lastAttackTime + (1f / attackSO.Rate))
            {
                context.Animator.SetTrigger("Attack");
                DoAttack();
                lastAttackTime = Time.time;
            }
        }

        public virtual void Exit(StateMachineContext context)
        {
        }
    }
}