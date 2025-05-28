using System;
using TowerDeffence.AI;
using TowerDeffence.ObjectPools;
using TowerDeffence.Utilities;
using UnityEngine;
using Zenject;

namespace Buildings
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        public float force = 50000f;
        private Rigidbody rb;
        public Action onKilled;
        private ObjectPoolWrapper<EnemyMovement> enemyObjectPool;
        private ObjectPoolWrapper<Projectile> projectileObjectPool;

        [Inject]
        private void Construct(ObjectPoolWrapper<EnemyMovement> enemyObjectPool, ObjectPoolWrapper<Projectile> projectileObjectPool)
        {
            this.enemyObjectPool = enemyObjectPool;
            this.projectileObjectPool = projectileObjectPool;
        }

        public void OnObjectReturnedToPool()
        {
            rb.velocity = Vector3.zero;
        }

        public void Seek(Transform target)
        {
            if (target != null)
            {
                rb = GetComponent<Rigidbody>();
                Vector3 flatDirection = (target.position - transform.position);
                rb.AddForce(flatDirection.normalized * force);
            }
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out EnemyMovement enemyMovement))
            {
                enemyObjectPool.Release(enemyMovement); 
                onKilled.Invoke();
                projectileObjectPool.Release(this);
            }
        }
    }
}