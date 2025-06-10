using UnityEngine;

namespace TowerDeffence.Buildings
{
    [CreateAssetMenu(fileName = "create new tower", menuName = "TowerDeffence/Buildings/Tower")]
    public class TowerSO : ScriptableObject
    {
        [SerializeField] private Projectile projectilePrefab;
        public Projectile ProjectilePrefab { get => projectilePrefab; }
        [SerializeField] private float fireRate = 1f;
        public float FireRate { get => fireRate; }
        [SerializeField, Range(1,100)] private float range = 30f;
        public float Range { get => range; }

        [SerializeField, Range(1,100)] private float damage = 1f;
        public float Damage { get => damage; }
    }
}