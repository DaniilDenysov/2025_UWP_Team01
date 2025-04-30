using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerDeffence.AI.Data;
using TowerDeffence.UI.Model;
using TowerDeffence.UI.Presenter;
using TowerDeffence.UI.View;
using TowerDeffence.Utilities;
using UnityEngine;

namespace TowerDeffence.AI
{
    [System.Serializable]
    public class Wave : IModel<WaveInfo>
    {
        public string Name;
        public List<WaveEnemy> Enemies = new List<WaveEnemy>();
        public float Duration;

        public System.Action<WaveInfo> OnModelUpdated { get; set; }

        public void UpdateModel()
        {
            OnModelUpdated?.Invoke(CreateSnapshot());
        }

        private WaveInfo CreateSnapshot()
        {
            return new WaveInfo() {Name = Name, Enemies = Enemies,Duration = Duration };
        }
    }


    public struct WaveInfo
    {
        public string Name;
        public IReadOnlyCollection<WaveEnemy> Enemies;
        public float Duration;
    }


    [System.Serializable]
    public class WaveEnemy
    {
        public EnemySO EnemyData;
        public uint Amount;
    }

    public class WaveManager : Presenter<View<WaveInfo>,Wave,WaveInfo>
    {
        public static Action OnLastWaveFinished;
        public static Action<string, float> OnNewWaveStarted;
        [SerializeField] private Transform target, spawnPoint;
        [SerializeField] private bool randomizeDelay;
        [SerializeField, Range(0, 100)] private float spawnDelay;
        [SerializeField] private List<Wave> waves = new List<Wave>();

        protected override void OnEnable()
        {
            if (2 <= waves.Count)
            {
                model = waves[1];
            }
            else
            {
                model = waves[0];
            }
            base.OnEnable();
        }

        public void ChangeModel(Wave wave)
        {
            base.OnDisable();
            model = wave;
            base.OnEnable();
        }

        private IEnumerator Start()
        {
            for (int i = 0; i < waves.Count; i++)
            {
                int totalEnemyCount = (int)waves[i].Enemies.Sum((w) => w.Amount);
                float waveDuration = waves[i].Duration / totalEnemyCount;
                OnNewWaveStarted?.Invoke(waves[i].Name, waves[i].Duration);
                if (i+1 < waves.Count)
                {
                    ChangeModel(waves[i + 1]);
                    model.UpdateModel();
                }
                else
                {
                    ChangeModel(waves[i]);
                    model.UpdateModel();
                }

                while (totalEnemyCount > 0)
                {
                    totalEnemyCount--;
                    WaveEnemy waveEnemy = GetRandomEnemy();
                    if (waveEnemy == null) break;
                    waveEnemy.Amount--;
                    //TODO: [DD] refacctor to pool
                    EnemyMovement instance = Instantiate(waveEnemy.EnemyData.Prefab,spawnPoint.position, Quaternion.identity);
                    instance.MoveTo(target.position);
                    yield return new WaitForSeconds(waveDuration + GetDealy());
                }
            }
            OnLastWaveFinished?.Invoke();
        }

        //TODO: [DD] account for delay in the duration calculation
        private float GetDealy()
        {
            return randomizeDelay ? UnityEngine.Random.Range(0, spawnDelay) : spawnDelay;
        }

        private WaveEnemy GetRandomEnemy()
        {
            List<WaveEnemy> availableEnemies = waves
            .SelectMany(w => w.Enemies)
            .Where(e => e.Amount > 0).ToList();
            if (availableEnemies.Count == 0) return null;
            return availableEnemies[UnityEngine.Random.Range(0, availableEnemies.Count)];
        }
    }
}
