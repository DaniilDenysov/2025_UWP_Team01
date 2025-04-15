using System;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        public float force = 50000f;
        private Rigidbody rb;
        public Action onKilled;

        public void Seek(Transform target)
        {
            if (target != null)
            {
                rb = GetComponent<Rigidbody>();
                Vector3 flatDirection = (target.position - transform.position);
                rb.AddForce(flatDirection.normalized * force);
            }
        }

        void Start()
        {
            Destroy(gameObject, 5f);
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Destroy(other.gameObject);
                onKilled.Invoke();
                Destroy(gameObject);
            }
        }
    }
}