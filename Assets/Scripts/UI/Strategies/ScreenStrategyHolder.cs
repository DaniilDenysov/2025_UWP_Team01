using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDeffence.UI.Strategies
{
    public class ScreenStrategyHolder : MonoBehaviour
    {
        [SerializeReference, SubclassSelector] private ScreenStrategy screenStrategy;

        public void SetActive(bool state)
        {
            screenStrategy.SetActive(state);
        }
    }
}
