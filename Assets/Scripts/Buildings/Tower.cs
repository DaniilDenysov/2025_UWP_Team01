using System.Collections.Generic;
using TowerDeffence.AI;
using TowerDeffence.Buildings.Strategies;
using TowerDeffence.ObjectPools;
using TowerDeffence.UI;
using TowerDeffence.Utilities;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace TowerDeffence.Buildings
{
    public class Tower : Building
    {
        [SerializeField] private TowerSO _towerSO;
        [SerializeField] private Transform firePoint;
        private float fireCountdown = 0f;
        private Transform target;
        private IAttackStrategyHandler<EnemyMovement> strategySelector;
        private IObjectPool<Projectile> objectPool;

        [Inject]
        private void Construct(IObjectPool<Projectile> objectPool)
        {
            this.objectPool = objectPool;
        }

        private void Start()
        {
            strategySelector = new ClosestEnemy() { myPosition = transform };
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

        public void SetStrategySelector(IAttackStrategyHandler<EnemyMovement> strategySelector)
        {
            this.strategySelector = strategySelector;
            DebugUtility.PrintLine("Strategy changed!");
        }

        private void FindTarget()
        {
            EnemyMovement nearestEnemy = strategySelector?.GetEnemy(new List<EnemyMovement>(EnemyMovement.AvailableEnemies));

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
            Projectile projGO = objectPool.GetObject(_towerSO.ProjectilePrefab);
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