using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace TowerDeffence.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour, IEnemyState
    {
        private static HashSet<EnemyMovement> avaialableEnemies = new HashSet<EnemyMovement>();
        public static IReadOnlyCollection<EnemyMovement> AvailableEnemies => avaialableEnemies;
        [SerializeField] private NavMeshAgent agent;

        private void OnEnable()
        {
            avaialableEnemies.Add(this);
        }

        private void OnDisable()
        {
            avaialableEnemies.Remove(this);
        }

        private void OnDestroy()
        {
            avaialableEnemies.Remove(this);
        }

        public void StopAgent()
        {
            agent.isStopped = true;
        }

        public void StartAgent()
        {
            agent.isStopped = false;
        }

        public void Warp(Vector3 position)
        {
            agent.Warp(position);
        }

        public void MoveTo(Vector3 position)
        {
            agent.SetDestination(position);
        }
        
        public void Enter(StateMachineContext context)
        {
            context.Animator.SetBool("IsMoving", true);
            StartAgent();
        }

        public void Execute(StateMachineContext context)
        {
            if(context.Owner.Target != null)
            {
                MoveTo(context.Owner.Target.position);
                float distance = Vector3.Distance(transform.position, context.Owner.Target.position);
                if (distance <= context.Owner.AttackData.Range)
                {
                    context.StateMachine.ChangeState(context.AttackState);
                }
            }
        }

        public void Exit(StateMachineContext context)
        {
            context.Animator.SetBool("IsMoving", false);
            StopAgent();
        }
    }
}
