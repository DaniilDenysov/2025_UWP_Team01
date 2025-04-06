using System;
using System.Collections.Generic;
using TMPro;
using TowerDeffence.AI;
using TowerDeffence.UI.Labels;
using UnityEngine;

namespace TowerDeffence.UI.View
{
    [Serializable]
    public class NextWavePreviewDisplay
    {
        public TMP_Text WaveName;
        public Transform Container;
        public List<EnemyLabel> Contents = new List<EnemyLabel>();
    }

    public class WaveView : View<WaveInfo>
    {
        [SerializeField] private EnemyLabel enemyLabelPrefab;
        [SerializeField] private NextWavePreviewDisplay nextWavePreviewDisplay;

        public override void UpdateView(WaveInfo model)
        {
            nextWavePreviewDisplay.WaveName.text = $"{model.Name}";
            ClearContainer();
            foreach (var enemy in model.Enemies)
            {
                EnemyLabel enemyLabel = Instantiate(enemyLabelPrefab,nextWavePreviewDisplay.Container);
                enemyLabel.Construct(enemy.EnemyData,enemy.Amount);
                nextWavePreviewDisplay.Contents.Add(enemyLabel);
            }
        }

        public void ClearContainer()
        {
            foreach (var label in nextWavePreviewDisplay.Contents)
            {
                Destroy(label.gameObject);
            }
            nextWavePreviewDisplay.Contents.Clear();
        }
    }
}
