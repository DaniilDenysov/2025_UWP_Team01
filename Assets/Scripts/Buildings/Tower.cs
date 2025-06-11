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
        [SerializeField] private AudioClipSO shootSFX;
        [SerializeField, Range(1f, 100f)] private float upgradeRate;  
        [SerializeField, Range(1f, 100f)] private float upgradeRange;  
        [SerializeField, Range(1f, 100f)] private float upgradeDamage;
        [SerializeField] List<Material> materialsToUpgread;
        private float fireCountdown = 0f;
        private Transform target;
        private IAttackStrategyHandler<EnemyMovement> strategySelector;
        private IObjectPool<Projectile> objectPool;
        public float rate { get; set; }
        public float damage { get; set; }
        public float range { get; set; }

        [Inject]
        private void Construct(IObjectPool<Projectile> objectPool)
        {
            this.objectPool = objectPool;
        }

        private void Start()
        {
            strategySelector = new ClosestEnemy() { myPosition = transform };
            rate = _towerSO.FireRate;
            range = _towerSO.Range;
            damage = _towerSO.Damage;
        }

        private void Update()
        {
            if (CanShoot())
            {
                FindTarget();

                if (target != null && fireCountdown <= 0f)
                {
                    Shoot();
                    fireCountdown = 1f / rate;
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
                if (Vector3.Distance(gameObject.transform.position, nearestEnemy.transform.position) < range)
                {
                    target = nearestEnemy.transform;
                }
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
            projGO.Damage = (uint)damage;

            SFXManager.Instance.PlayOneShot(shootSFX, transform.position);
            if (projGO != null)
            {
                projGO.Seek(target);
            }
        }

        private bool CanShoot()
        {
            return !isPreviewMode;
        }


        public virtual void UpgradeRange()
        {
            commandContainer.ExecuteCommand(new UpgradeCommand(this, Updatable.Range, upgradeRange, materialsToUpgread));
        }

        public virtual void UpgradeRate()
        {
            commandContainer.ExecuteCommand(new UpgradeCommand(this, Updatable.Rate, upgradeRate, materialsToUpgread));
        }

        public virtual void UpgradeDamage()
        {
            commandContainer.ExecuteCommand(new UpgradeCommand(this, Updatable.Damage, upgradeDamage, materialsToUpgread));
        }
    }
}