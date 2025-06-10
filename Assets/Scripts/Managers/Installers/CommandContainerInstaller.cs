using UnityEngine;
using Zenject;

namespace TowerDeffence.Managers.Installers
{
    public class CommandContainerInstaller : MonoInstaller
    {
        [SerializeField] private CommandContainer commandContainer;

        public override void InstallBindings()
        {
            Container.Bind<CommandContainer>().To<CommandContainer>().FromInstance(commandContainer).AsSingle().NonLazy();
        }
    }
}