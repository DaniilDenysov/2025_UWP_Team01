using System;
using TowerDeffence.UI.Model.Varialbes;
using TowerDeffence.UI.Presenter;
using TowerDeffence.UI.Presenter.Variables;
using TowerDeffence.UI.View.Variables;
using Unity.VisualScripting;
using UnityEngine;

namespace TowerDeffence.AI
{
    public class EconomyManager : IntegerPresenter
    {
        [SerializeField] private int _enemyPrice;
        public static EconomyManager Instance;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void OnKill()
        {
            Add(_enemyPrice);
        }

        public void Add(int amount)
        {
            OnViewVariableAdded(amount);
        }
        
        public void Reduce(int amount)
        {
            if (model.Value > amount)
            {
                OnViewVariableAdded(-amount);
            }
            else
            {
                OnViewVariableAdded(0);
            }
        }

        public bool CanAfford(int amount)
        {
            return model.Value > amount;
        }
    }
}