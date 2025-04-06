using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDeffence.AI.Data
{
    [CreateAssetMenu(fileName = "EnemySO",menuName = "TowerDeffence/Enemies/Create new enemy")]
    public class EnemySO : ScriptableObject
    {
        [SerializeField] private Sprite enemyIcon;
        public Sprite EnemyIcon
        {
            get => enemyIcon;
        }
        [SerializeField] private string enemyName;
        public string EnemyName
        {
            get => enemyName;
        }
        [SerializeField] private EnemyMovement prefab;
        public EnemyMovement Prefab
        {
            get => prefab;
        }
    }
}
