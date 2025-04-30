using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDeffence.Interfaces
{
    public interface IDamagable
    {
        public bool DoDamage(uint damage);
        public uint GetCurrentHealthPoints();
    }
}
