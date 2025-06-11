using System.Collections;
using System.Collections.Generic;
using TowerDeffence.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace TowerDeffence.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour, IEnemyState
    {
        private static HashSet<EnemyMovement> avaialableEnemies = new HashSet<EnemyMovement>();
        public static IReadOnlyCollection<EnemyMovement> AvailableEnemies => avaialableEnemies;

        private NavMeshAgent agent;
        private Animator animator;

        public void Initialize() { agent = GetComponent<NavMeshAgent>(); animator = GetComponent<Animator>(); }
        private void OnEnable() { avaialableEnemies.Add(this); }
        private void OnDisable() { avaialableEnemies.Remove(this); }
        public void StopAgent() { if (agent != null && agent.isActiveAndEnabled) agent.isStopped = true; }
        public void StartAgent() { if (agent != null && agent.isActiveAndEnabled) agent.isStopped = false; }
        public void MoveTo(Vector3 position) { if (agent != null && agent.isActiveAndEnabled) agent.SetDestination(position); }
        public void Warp(Vector3 position) { if (agent != null && agent.isActiveAndEnabled) agent.Warp(position); }

        public void Enter(StateMachineContext context)
        {
            StartAgent();
            if (animator != null) animator.SetBool("IsMoving", true);
        }

        public void Execute(StateMachineContext context)
        {
            var target = context.Owner.currentTarget;

            if (target != null)
            {
                MoveTo(target.transform.position);
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance <= context.Owner.AttackData.Range)
                {
                    context.Owner.RequestStateChange(context.AttackState);
                }
            }
            else
            {
                DebugUtility.PrintLine("Stopped agent!");
                StopAgent();
            }
        }

        public void Exit(StateMachineContext context)
        {
            StopAgent();
            DebugUtility.PrintLine("Stopped agent!");
            if (animator != null) animator.SetBool("IsMoving", false);
        }
    }
}