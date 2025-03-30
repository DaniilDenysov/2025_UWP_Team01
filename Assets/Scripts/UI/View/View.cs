using System;
using System.Collections;
using System.Collections.Generic;
using TowerDeffence.UI.Model;
using UnityEngine;

namespace TowerDeffence.UI.View
{
    public abstract class View<T> : MonoBehaviour
    {
        public event Action OnStateChanged;

        public abstract void UpdateView(T model);
    }
}
