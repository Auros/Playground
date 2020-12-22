using System;
using Zenject;
using SiraUtil;
using Playground.UI;
using Playground.Managers;
using Playground.BlockHunt;
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
            Container.BindMemoryPool<KoBullet, KoBullet.Pool>().WithInitialSize(5).FromFactory<KoBulletFactory>();
            Container.BindMemoryPool<KoBlock, KoBlock.Pool>().WithInitialSize(10).FromFactory<KoBlockFactory>();
            Container.BindMemoryPool<Duck, Duck.Pool>().WithInitialSize(2).FromFactory<DuckFactory>();
            Container.BindFactory<KoPodium, KoPodiumFactory.Fact>().FromFactory<KoPodiumFactory>();
            Container.BindFactory<KoGun, KoGunFactory.Fact>().FromFactory<KoGunFactory>();

            // Stuff
            Container.Bind<JojoPodiumManager>().AsSingle();
            Container.Bind<DenyahBackNoteManager>().AsSingle();
            Container.Bind(typeof(IKoGameManager), typeof(IInitializable), typeof(IDisposable)).To<KoGameManager>().AsSingle();

            // UI
            Container.Bind<PodiumViewController>().FromNewComponentAsViewController().AsSingle();

            // Gamemodes
            Container.BindInterfacesTo<BlockHuntGame>().AsSingle();
            Container.Bind<ShootingManager>().AsSingle();

            Container.BindInterfacesTo<Dummy>().AsSingle();
        }
        private class Dummy : IKoGame
        {
            public string Name => "Dummy";

            public bool Playing => throw new NotImplementedException();

            public bool Activating => throw new NotImplementedException();

            public bool Deactivating => throw new NotImplementedException();

            public void Begin()
            {
                throw new NotImplementedException();
            }

            public void Create()
            {
                throw new NotImplementedException();
            }

            public void Destroy()
            {
                throw new NotImplementedException();
            }

            public void Stop()
            {
                throw new NotImplementedException();
            }
        }
    }

}