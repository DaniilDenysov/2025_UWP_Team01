using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TowerDeffence.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;


        public void MoveTo(Vector3 position)
        {
            agent.SetDestination(position);
        }
    }
}
