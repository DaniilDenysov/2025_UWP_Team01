using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public interface IEnemyState
{
    void Enter(StateMachineContext context);
    void Execute(StateMachineContext context);
    void Exit(StateMachineContext context);
}