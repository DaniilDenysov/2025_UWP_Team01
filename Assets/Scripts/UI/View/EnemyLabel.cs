using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerDeffence.AI.Data;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDeffence.UI.Labels
{
    public class EnemyLabel : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text nameDisplay;
        [SerializeField] private TMP_Text amountDisplay;
        public void Construct(EnemySO enemySO, uint amount)
        {
            iconImage.sprite = enemySO.EnemyIcon;
            nameDisplay.text = enemySO.EnemyName;
            amountDisplay.text = $"{amount}";
        }
    }
}
