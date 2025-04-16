using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour
{
    [SerializeField] private UnityEvent<Vector2> onMove;
    
    public void OnMove(InputValue value)
    {
        onMove.Invoke(value.Get<Vector2>());
    }
}
