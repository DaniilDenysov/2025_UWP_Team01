using System;
using System.Collections.Generic;
using TowerDeffence.AI;
using TowerDeffence.Buildings;
using UnityEngine;

namespace Buildings
{
    public class AreaProjectile : Projectile
    {
        private List<EnemyMovement> enemies = new List<EnemyMovement>();
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out EnemyMovement enemyMovement))
            {
                enemies.Add(enemyMovement);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out EnemyMovement enemyMovement))
            {
                enemies.Remove(enemyMovement);
            }
        }

        protected override void OnCollisionEnter(Collision other)
        {
            foreach (var enemyMovement in enemies)
            {
                enemyObjectPool.ReleaseObject(enemyMovement); 
                onKilled.Invoke();
            }
            
            projectileObjectPool.ReleaseObject(this);
        }
    }
}