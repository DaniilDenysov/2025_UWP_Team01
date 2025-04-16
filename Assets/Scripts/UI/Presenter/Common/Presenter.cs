using System.Collections;
using System.Collections.Generic;
using TowerDeffence.UI.Model;
using TowerDeffence.UI.View;
using UnityEngine;

namespace TowerDeffence.UI.Presenter
{
    public abstract class Presenter<V, M, T> : MonoBehaviour where M : IModel<T> where V : View<T>
    {
        [SerializeField] protected M model;
        [SerializeField] protected V view;

        protected virtual void OnEnable()
        {
            view.OnStateChanged += OnViewUpdated;
            model.OnModelUpdated += OnModelUpdated; 
        }

        protected virtual void OnDisable()
        {
            view.OnStateChanged -= OnViewUpdated;
            model.OnModelUpdated -= OnModelUpdated;
        }

        protected virtual void OnViewUpdated()
        {
            model.UpdateModel();
        }
        protected virtual void OnModelUpdated(T data)
        {
            view.UpdateView(data);
        }
    }
}