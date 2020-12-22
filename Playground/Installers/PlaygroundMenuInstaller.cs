﻿using System;
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
            Container.BindMemoryPool<KoBlock, KoBlock.Pool>().WithInitialSize(10).FromFactory<KoBlockFactory>();
            Container.BindFactory<KoPodium, KoPodiumFactory.Fact>().FromFactory<KoPodiumFactory>();
            Container.BindMemoryPool<Duck, Duck.Pool>().WithInitialSize(2).FromFactory<DuckFactory>();

            // Stuff
            Container.Bind<JojoPodiumManager>().AsSingle();
            Container.Bind<DenyahBackNoteManager>().AsSingle();
            Container.Bind(typeof(IKoGameManager), typeof(IInitializable), typeof(IDisposable)).To<KoGameManager>().AsSingle();

            // UI
            Container.Bind<PodiumViewController>().FromNewComponentAsViewController().AsSingle();

            // Gamemodes
            Container.BindInterfacesTo<BlockHuntGame>().AsSingle();
            Container.BindInterfacesTo<Dummy>().AsSingle();
        }
        private class Dummy : IKoGame
        {
            public string Name => "Dummy";

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