using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDeffence.UI.Model.Varialbes
{
    public abstract class VariableSO<T> : ScriptableObject
    {
        [SerializeField] protected T defaultValue;
        protected T currentValue;
        public T Value
        {
            set
            {
                if (currentValue.Equals(value)) return;
                OnValueChanged?.Invoke(currentValue, value);
                currentValue = value;
            }
            get => currentValue;
        }
        public event Action<T,T> OnValueChanged;

        protected virtual void ResetValue()
        {
            currentValue = defaultValue;
        }

        private void OnEnable()
        {
            ResetValue();
        }

        private void OnDisable()
        {
            ResetValue();
        }
    }
}
