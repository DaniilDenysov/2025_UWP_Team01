using System;
using System.Threading;
using System.Threading.Tasks;
using TowerDeffence.AI;
using TowerDeffence.HealthSystem;
using TowerDeffence.Utilities;
using UnityEngine;

namespace TowerDeffence.Managers
{
    [System.Serializable]
    public abstract class ConditionStrategy
    {
        protected Action<string> OnConditionFulfilled;
        protected bool fulfilled;

        public abstract void OnEnable();

        public abstract void OnDisable();

        public void ConditionFulfilled(string reason)
        {
            if (fulfilled) return;
            OnConditionFulfilled?.Invoke(reason);
            fulfilled = true;
            OnDisable();
        }

        public void Subscribe(Action<string> action)
        {
            OnConditionFulfilled += action;
        }
        public void Unsubscribe(Action<string> action)
        {
            OnConditionFulfilled -= action;
        }
    }

    [System.Serializable]
    public class WinCondition : ConditionStrategy
    {
        private CancellationTokenSource cancellationTokenSource;

        public override void OnDisable()
        {
            if (!(cancellationTokenSource.IsCancellationRequested || cancellationTokenSource == null))
            {
                cancellationTokenSource?.Cancel();
                cancellationTokenSource?.Dispose();
            }
            WaveManager.OnLastWaveFinished -= OnLastWaveFinished;
        }

        public void OnLastWaveFinished()
        {
            if (fulfilled) return;
            WaitForCondition(cancellationTokenSource.Token);
        }

        public async void WaitForCondition(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (EnemyMovement.AvailableEnemies.Count == 0)
                    {
                        ConditionFulfilled("Last enemy destroyed!");
                        return;
                    }

                    await Task.Delay(100, cancellationToken);
                }
            }
            catch (TaskCanceledException)
            {

            }
        }

        public override void OnEnable()
        {
            cancellationTokenSource = new CancellationTokenSource();
            WaveManager.OnLastWaveFinished += OnLastWaveFinished;
        }
    }

    [System.Serializable]
    public class LoseCondition : ConditionStrategy
    {
        [SerializeField] private HealthPresenter townHall;

        public override void OnDisable()
        {
            townHall.OnModelUpdatedAction -= OnTownHallDamaged;
        }

        public override void OnEnable()
        {
            townHall.OnModelUpdatedAction += OnTownHallDamaged;
        }

        public void OnTownHallDamaged(Health health)
        {
            if (fulfilled) return;
            if (health == null) return;
            if (health.Current <= 0)
            {
                ConditionFulfilled("Last enemy destroyed!");
            }
        }
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeReference, SerializeField, SubclassSelector] private ConditionStrategy winCondiotion;
        [SerializeReference, SerializeField, SubclassSelector] private ConditionStrategy loseCondiotion;

        private void OnEnable()
        {
            winCondiotion?.OnEnable();
            winCondiotion?.Subscribe(OnWinConditionFullfilled);
            loseCondiotion?.OnEnable();
            loseCondiotion?.Subscribe(OnLoseConditionFullfilled);
        }

        private void OnDisable()
        {
            winCondiotion?.OnDisable();
            winCondiotion?.Unsubscribe(OnWinConditionFullfilled);
            loseCondiotion?.OnDisable();
            loseCondiotion?.Unsubscribe(OnLoseConditionFullfilled);
        }

        private void OnLoseConditionFullfilled(string reason)
        {
            DebugUtility.PrintLine("Lose condition fullfilled!");
        }

        private void OnWinConditionFullfilled(string reason)
        {
            DebugUtility.PrintLine("Win condition fullfilled!");
        }
    }
}
