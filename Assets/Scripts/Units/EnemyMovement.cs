using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace TowerDeffence.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour
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

        public void MoveTo(Vector3 position)
        {
            agent.SetDestination(position);
        }
    }
}
