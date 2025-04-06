using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerDeffence.AI;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDeffence.UI.View
{
    public class WaveProgressView : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentWaveNameDisplay;
        [SerializeField] private Slider currentWaveProgressDisplay;
        private Coroutine currentTimer;

        private void OnEnable ()
        {
            WaveManager.OnNewWaveStarted += OnNewWaveStarted;
        }

        private void OnDisable()
        {
            WaveManager.OnNewWaveStarted -= OnNewWaveStarted;
        }

        private void OnNewWaveStarted(string name, float duration)
        {
            currentWaveNameDisplay.text = name;
            if (currentTimer != null) StopCoroutine(currentTimer);
            currentTimer = StartCoroutine(StartTimer(duration));
        }

        private IEnumerator StartTimer(float duration)
        {
            float startTime = Time.time;
            float endTime = startTime + duration;

            while (Time.time < endTime)
            {
                float elapsed = Time.time - startTime;
                float currentTime = elapsed / duration;
                currentWaveProgressDisplay.value = Mathf.Lerp(currentWaveProgressDisplay.maxValue, currentWaveProgressDisplay.minValue, currentTime);
                yield return null;
            }
            currentWaveProgressDisplay.value = currentWaveProgressDisplay.minValue;
        }

    }
}
