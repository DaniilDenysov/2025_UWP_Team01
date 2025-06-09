using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public IEnemyState CurrentState { get; private set; }
    public event Action<IEnemyState> OnStateChanged;

    public void Initialize(IEnemyState startingState)
    {
        CurrentState = startingState;
        CurrentState?.Enter(null);
        OnStateChanged?.Invoke(CurrentState);
    }

    public void ChangeState(IEnemyState newState)
    {
        CurrentState?.Exit(null);
        CurrentState = newState;
        CurrentState?.Enter(null);
        OnStateChanged?.Invoke(CurrentState);
    }
}
