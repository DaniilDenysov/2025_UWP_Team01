using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 lowerbound;
    [SerializeField] private Vector2 upperbound;
    private Vector2 _inputVector;

    private void Update()
    {
        HandleMovement();
    }

    public void OnMove(Vector2 inputVector)
    {
        _inputVector = inputVector;
    }

    public void HandleMovement()
    {
        if (_inputVector.magnitude == 0) return;
        
        Vector2 movementVector = _inputVector.normalized * speed;
        transform.position += processVector(movementVector);
    }

    public Vector3 processVector(Vector2 vector)
    {
        Vector3 result = new Vector3(vector.y, 0, -vector.x);
        
        if ((transform.position.x + result.x < lowerbound.x && result.x < 0) ||
            (transform.position.x + result.x > upperbound.x && result.x > 0))
        {
            result.x = 0;
        }
        
        if ((transform.position.z + result.z < lowerbound.y && result.z < 0) ||
            (transform.position.z + result.z > upperbound.y && result.z > 0))
        {
            result.z = 0;
        }

        return result;
    }
}
