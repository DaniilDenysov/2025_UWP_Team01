using System;
using TMPro;
using UnityEngine;

namespace TowerDeffence.UI.View.Variables
{
    public abstract class VariableView<T> : MonoBehaviour
    {
        [SerializeField] protected TMP_Text display;
        [SerializeField,TextArea] protected string prefix;
        public event Action<T> OnVariableAssigned;
        public event Action<T> OnVariableAdded;

        public virtual void UpdateView(T value)
        {
            display.text = $"{prefix}{value}";
        }

        public virtual void AssignValue(T value)
        {
            OnVariableAssigned?.Invoke(value);
        }

        public virtual void AddValue(T value)
        {
            OnVariableAdded?.Invoke(value);
        }
    }
}
