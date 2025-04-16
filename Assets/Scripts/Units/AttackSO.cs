using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDeffence.AI.Data
{
    [CreateAssetMenu(fileName = "AttackSO", menuName = "TowerDeffence/Enemies/Create attackSO")]
    public class AttackSO : ScriptableObject
    {
        [SerializeField, Range(0, 100)] protected uint damage;
        public uint Damage
        {
            get => damage;
        }
        [SerializeField, Range(0, 100)] protected uint range;
        public uint Range
        {
            get => range;
        }
        [SerializeField, Range(0, 100)] protected uint rate;
        public uint Rate
        {
            get => rate;
        }
    }
}
