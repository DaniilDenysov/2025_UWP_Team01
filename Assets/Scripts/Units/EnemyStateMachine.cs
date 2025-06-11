using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public IEnemyState CurrentState { get; private set; }
    public event Action<IEnemyState> OnStateChanged;
    private StateMachineContext context; 
    public void SetContext(StateMachineContext context)
    {
        this.context = context;
    }

    public void Initialize(IEnemyState startingState)
    {
        CurrentState = startingState;
        Debug.Log($"[StateMachine] Initialized with state: {startingState.GetType().Name}");
        CurrentState?.Enter(context);
        OnStateChanged?.Invoke(CurrentState);
    }

    public void ChangeState(IEnemyState newState)
    {
        if (newState == CurrentState) return;

        Debug.Log($"[StateMachine] Transitioning from <{CurrentState?.GetType().Name}> to <{newState.GetType().Name}>");

        CurrentState?.Exit(context);
        CurrentState = newState;
        CurrentState?.Enter(context);
        OnStateChanged?.Invoke(CurrentState);
    }
}
