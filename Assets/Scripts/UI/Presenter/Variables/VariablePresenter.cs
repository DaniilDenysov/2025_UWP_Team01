using TowerDeffence.UI.Model.Varialbes;
using TowerDeffence.UI.View.Variables;
using TowerDeffence.Utilities;
using UnityEngine;

namespace TowerDeffence.UI.Presenter.Variables
{
    public abstract class VariablePresenter<T> : MonoBehaviour
    {
        [SerializeField] protected VariableSO<T> model;
        [SerializeField] protected VariableView<T> view;

        protected void Start()
        {
            view.UpdateView(model.Value);
        }

        protected virtual void OnEnable()
        {
            if (model != null)
            {
                model.OnValueChanged += OnVariableUpdated;
            }
            else
            {
                DebugUtility.PrintError("Model is null!");
            }
            if (view != null)
            {
                view.OnVariableAssigned += OnViewVariableAssigned;
                view.OnVariableAdded += OnViewVariableAdded;
            }
            else
            {
                DebugUtility.PrintError("View is null!");
            }
        }

        protected virtual void OnDisable()
        {
            if (model != null)
            {
                model.OnValueChanged -= OnVariableUpdated;
            }
            else
            {
                DebugUtility.PrintError("Model is null!");
            }
            if (view != null)
            {
                view.OnVariableAssigned -= OnViewVariableAssigned;
                view.OnVariableAdded -= OnViewVariableAdded;
            }
            else
            {
                DebugUtility.PrintError("View is null!");
            }
        }

        protected  abstract void OnViewVariableAssigned(T newValue);

        protected abstract void OnViewVariableAdded(T addedValue);

        protected virtual void OnVariableUpdated(T oldValue, T newValue)
        {
            if (view == null) return;
            view.UpdateView(newValue);
        }
    }
}
