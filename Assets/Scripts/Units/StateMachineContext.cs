using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineContext
{
    public EnemyStateMachine StateMachine { get; }
    public Animator Animator { get; }
    public EnemyController Owner { get; }
    public IEnemyState MovementState { get; }
    public IEnemyState AttackState { get; }
    public IEnemyState DieState { get; }

    public StateMachineContext(EnemyStateMachine stateMachine, Animator animator, EnemyController owner, IEnemyState movementState, IEnemyState attackState, IEnemyState dieState)
    {
        StateMachine = stateMachine;
        Animator = animator;
        Owner = owner;
        MovementState = movementState;
        AttackState = attackState;
        DieState = dieState;
    }
}
