using System;
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
        
        [SerializeField, Range(0f, 1f)] float[] thresholds;
        public Action<int> OnHealthThreshold;
        private int thresholdsIndex = 0;

        public System.Action<Health> OnModelUpdated 
        { 
            get; 
            set; 
        }

        public void ResetHealth()
        {
            health.Current = health.Max;
        }

        public bool DoDamage(uint damage)
        {
            bool isDead = health.Current - damage <= 0;
            health.Current = (int)Mathf.Clamp(health.Current - damage, 0, health.Max);
            OnModelUpdated?.Invoke(health);
            CheckHealthThresholds();
            return isDead;
        }

        public uint GetCurrentHealthPoints()
        {
            return (uint)health?.Current;
        }

        private void CheckHealthThresholds()
        {
            float healthPercentage = (float)health.Current / health.Max;

            if (thresholdsIndex < thresholds.Length && healthPercentage <= thresholds[thresholdsIndex])
            {
                OnHealthThreshold?.Invoke(thresholdsIndex);
                thresholdsIndex++;
            }
            
        }

        public void UpdateModel()
        {
            OnModelUpdated?.Invoke(health);
        }
    }

    public class HealthPresenter : Presenter<HealthSystemView, HealthModel, Health>, IDamagable
    {
        [SerializeField] BackgroundMusicChanger changer;

        private void Start()
        {
            model.UpdateModel();
            model.OnHealthThreshold += HandleHpThreshold;
        }

        private void OnDestroy()
        {
            if (model != null)
            {
                model.OnHealthThreshold -= HandleHpThreshold;
            }
        }

        private void HandleHpThreshold(int index)
        {
            if (!changer) return;
            changer.ChangeBackgroundMusic(index);
        }


        public void ResetHealth()
        {
            model.ResetHealth();
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