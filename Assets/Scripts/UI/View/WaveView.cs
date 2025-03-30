using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerDeffence.AI;
using UnityEngine;

namespace TowerDeffence.UI.View
{
    public class WaveView : View<WaveInfo>
    {
        [SerializeField] private TMP_Text currentWaveNameDisplay;


        public override void UpdateView(WaveInfo model)
        {
            currentWaveNameDisplay.text = $"Incoming wave:{model.Name} Duration:{model.Duration}";
        }
    }
}
