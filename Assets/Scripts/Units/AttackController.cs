using System.Collections;
using System.Collections.Generic;
using TowerDeffence.AI.Data;
using TowerDeffence.Interfaces;
using UnityEngine;

namespace TowerDeffence.AI
{
    public abstract class AttackController : MonoBehaviour, IEnemyState
    {
        [SerializeField] protected AttackSO attackSO;
        public uint Damage { get => attackSO.Damage; }
        public uint Range { get => attackSO.Range; }
        public uint Rate { get => attackSO.Rate; }
        public abstract GameObject GetClosestEnemy();

        public abstract void DoAttack();

        public abstract void Enter(StateMachineContext context);
        public abstract void Execute(StateMachineContext context);
        public abstract void Exit(StateMachineContext context);
    }
}
