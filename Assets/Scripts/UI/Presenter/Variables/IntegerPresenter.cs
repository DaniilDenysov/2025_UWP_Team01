using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDeffence.UI.Presenter.Variables
{
    public class IntegerPresenter : VariablePresenter<int>
    {
        protected override void OnViewVariableAdded(int addedValue)
        {
            if (model == null) return;
            model.Value = model.Value + addedValue;
            view.UpdateView(model.Value);
        }

        protected override void OnViewVariableAssigned(int newValue)
        {
            if (model == null) return;
            model.Value = newValue;
            view.UpdateView(model.Value);
        }
    }
}
