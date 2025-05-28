using System;
using UnityEngine;

namespace TowerDeffence.AI
{
    public interface IPlacable
    {
        public void OnStartedPlace();
        public void SubscribeToPlacing(BuildingPlacer buildingPlacer);
        public void UnsubscribeToPlacing();
    }
}