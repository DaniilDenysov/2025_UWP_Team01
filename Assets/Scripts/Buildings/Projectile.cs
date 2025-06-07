using System;
using TowerDeffence.AI;
using TowerDeffence.ObjectPools;
using TowerDeffence.Utilities;
using UnityEngine;
using Zenject;

namespace TowerDeffence.Buildings
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        private Transform rotateTowards;
        [SerializeField] private Transform model;
        public float force = 50000f;
        private Rigidbody rb;
        public Action onKilled;
        private ObjectPoolWrapper<EnemyMovement> enemyObjectPool;
        private ObjectPoolWrapper<Projectile> projectileObjectPool;
        private Quaternion originalRotation;


        private void Start()
        {
            originalRotation = model.transform.rotation;
        }

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

        private void Update()
        {
            if (!rotateTowards) return;

            Vector3 direction = (rotateTowards.position - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                lookRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);

                Quaternion finalRotation = lookRotation * originalRotation;

                model.transform.rotation = Quaternion.Slerp(model.transform.rotation, finalRotation, Time.deltaTime * 10f);
            }
        }




        public void Seek(Transform target)
        {
            if (target != null)
            {
                rotateTowards = target;
                rb = GetComponent<Rigidbody>();
                Vector3 flatDirection = (target.position - transform.position);
                rb.AddForce(flatDirection.normalized * force);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out EnemyMovement enemyMovement))
            {
                enemyObjectPool.Release(enemyMovement); 
                onKilled.Invoke();
            }
            projectileObjectPool.Release(this);
        }
    }
}