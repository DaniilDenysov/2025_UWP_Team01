using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour
{
    [SerializeField] private UnityEvent<Vector2> onMove;
    [SerializeField] private UnityEvent onUndo;
    [SerializeField] private UnityEvent onRedo;
    
    public void OnMove(InputValue value)
    {
        onMove.Invoke(value.Get<Vector2>());
    }
    
    public void OnUndo(InputValue value)
    {
        onUndo.Invoke();
    }
    
    public void OnRedo(InputValue value)
    {
        onRedo.Invoke();
    }
}
