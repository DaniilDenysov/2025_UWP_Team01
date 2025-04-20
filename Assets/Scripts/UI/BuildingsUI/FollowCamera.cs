using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Camera objectToFollow;

    private void Start()
    {
       objectToFollow = Camera.main;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - objectToFollow.transform.position);        
    }
}
