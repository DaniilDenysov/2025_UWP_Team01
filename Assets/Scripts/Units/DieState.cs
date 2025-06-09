using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : IEnemyState
{
    public void Enter(StateMachineContext context)
    {
        context.Animator.SetTrigger("Die");
        context.Owner.StartCoroutine(context.Owner.DestroyAfterAnimation(context.Animator, "Die"));
    }

    public void Execute(StateMachineContext context)
    {
    }

    public void Exit(StateMachineContext context)
    {
    }
}
