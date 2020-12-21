using Zenject;
using Playground.Factories;
using Playground.Components;

namespace Playground.Installers
{
    internal class PlaygroundMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindMemoryPool<KoBlock, KoBlock.Pool>().WithInitialSize(10).FromFactory<KoBlockFactory>();
        }
    }
}