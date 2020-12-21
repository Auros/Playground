using System;
using Zenject;
using SiraUtil;
using Playground.UI;
using Playground.Managers;
using Playground.Factories;
using Playground.Components;
using Playground.Interfaces;

namespace Playground.Installers
{
    internal class PlaygroundMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            // Factories
            Container.BindMemoryPool<KoBlock, KoBlock.Pool>().WithInitialSize(10).FromFactory<KoBlockFactory>();
            Container.BindFactory<KoPodium, KoPodiumFactory.Fact>().FromFactory<KoPodiumFactory>();

            // Stuff
            Container.Bind(typeof(IKoGameManager), typeof(IInitializable), typeof(IDisposable)).To<KoGameManager>().AsSingle();

            // UI
            Container.Bind<PodiumViewController>().FromNewComponentAsViewController().AsSingle();
        }
    }
}