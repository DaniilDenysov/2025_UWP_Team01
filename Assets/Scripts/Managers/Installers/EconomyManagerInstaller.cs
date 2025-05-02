using TowerDeffence.AI;
using UnityEngine;
using Zenject;

namespace TowerDeffence.Managers.Installers
{
    public class EconomyManagerInstaller : MonoInstaller
    {
        [SerializeField] private EconomyManager economyManager;

        public override void InstallBindings()
        {
            Container.Bind<EconomyManager>().To<EconomyManager>().FromInstance(economyManager).AsSingle().NonLazy();
        }
    }
}
