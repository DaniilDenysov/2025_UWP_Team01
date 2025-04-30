using TowerDeffence.Interfaces;
using TowerDeffence.UI.Model;
using TowerDeffence.UI.Presenter;
using UnityEngine;


namespace TowerDeffence.HealthSystem
{
    [System.Serializable]
    public class Health
    {
        public int Current;
        public int Max;
    }

    [System.Serializable]
    public class HealthModel : IModel<Health>, IDamagable
    {
        [SerializeField] private Health health;

        public System.Action<Health> OnModelUpdated 
        { 
            get; 
            set; 
        }

        public bool DoDamage(uint damage)
        {
            bool isDead = health.Current - damage <= 0;
            health.Current = (int)Mathf.Clamp(health.Current - damage, 0, health.Max);
            OnModelUpdated?.Invoke(health);
            return isDead;
        }

        public uint GetCurrentHealthPoints()
        {
            return (uint)health?.Current;
        }

        public void UpdateModel()
        {
            OnModelUpdated?.Invoke(health);
        }
    }

    public class HealthPresenter : Presenter<HealthSystemView, HealthModel, Health>, IDamagable
    {
        private void Start()
        {
            model.UpdateModel();
        }

        public bool DoDamage(uint damage)
        {
           return model.DoDamage(damage);
        }

        public uint GetCurrentHealthPoints()
        {
            return model.GetCurrentHealthPoints();
        }
    }
}