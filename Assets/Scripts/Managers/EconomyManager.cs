using TowerDeffence.UI.Presenter.Variables;
using UnityEngine;

namespace TowerDeffence.AI
{
    public class EconomyManager : IntegerPresenter
    {
        [SerializeField] private int _enemyPrice;

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