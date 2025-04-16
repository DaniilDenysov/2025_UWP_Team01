using System.Collections;
using System.Collections.Generic;
using TowerDeffence.AI.Data;
using TowerDeffence.Interfaces;
using UnityEngine;

namespace TowerDeffence.AI
{
    public abstract class AttackController : MonoBehaviour
    {
        [SerializeField] protected AttackSO attackSO;

        public abstract GameObject GetClosestEnemy();

        public abstract void DoAttack();
    }
}
