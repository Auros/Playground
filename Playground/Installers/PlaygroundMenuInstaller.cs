using System;
using Zenject;
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

            // Stuff
            Container.Bind(typeof(IKoGameManager), typeof(IInitializable), typeof(IDisposable)).To<KoGameManager>().AsSingle();

            // UI
        }
    }
}