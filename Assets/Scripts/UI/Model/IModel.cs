using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDeffence.UI.Model
{
    public interface IModel<T>
    {
        Action<T> OnModelUpdated { get; set; }
        void UpdateModel();
    }
}
