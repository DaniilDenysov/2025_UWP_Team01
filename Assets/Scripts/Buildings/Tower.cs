using TowerDeffence.AI;
using TowerDeffence.ObjectPools;
using UnityEngine;
using Zenject;

namespace Buildings
{
    public class Tower : Building
    {
        [SerializeField] private TowerSO _towerSO;
        [SerializeField] private Transform firePoint;
        private float fireCountdown = 0f;
        private Transform target;

        private ObjectPoolWrapper<Projectile> objectPool;

        [Inject]
        private void Construct(ObjectPoolWrapper<Projectile> objectPool)
        {
            this.objectPool = objectPool;
        }

        private void Update()
        {
            if (CanShoot())
            {
                FindTarget();

                if (target != null && fireCountdown <= 0f)
                {
                    Shoot();
                    fireCountdown = 1f / _towerSO.FireRate;
                }

                fireCountdown -= Time.deltaTime;
            }
        }

        private void FindTarget()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance && distanceToEnemy <= _towerSO.Range)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null)
            {
                target = nearestEnemy.transform;
            }
            else
            {
                target = null;
            }
        }

        private void Shoot()
        {
            Projectile projGO = objectPool.Get(_towerSO.ProjectilePrefab);
            projGO.transform.position = firePoint.position;
            projGO.transform.rotation = firePoint.rotation;
            projGO.onKilled += _economyManager.OnKill;

            if (projGO != null)
            {
                projGO.Seek(target);
            }
        }

        private bool CanShoot()
        {
            return !isPreviewMode;
        }

    }
}