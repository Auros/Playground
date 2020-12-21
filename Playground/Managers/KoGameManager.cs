using System;
using Zenject;
using Playground.UI;
using Playground.Factories;
using Playground.Interfaces;
using Playground.Components;
using System.Collections.Generic;

namespace Playground.Managers
{
    internal class KoGameManager : IKoGameManager, IInitializable, IDisposable
    {
        private readonly KoPodium _podium;
        private readonly List<IKoGame> _koGames;
        private readonly PodiumViewController _podiumViewController;

        public IKoGame? ActiveMode { get; private set; }

        public KoGameManager([InjectOptional] List<IKoGame> koGames, KoPodiumFactory.Fact podiumFactory, PodiumViewController podiumViewController)
        {
            _koGames = koGames;
            _podium = podiumFactory.Create();
            if (_koGames == null)
            {
                _koGames = new List<IKoGame>();
            }
            _podiumViewController = podiumViewController;
        }

        public void Initialize()
        {
            _podiumViewController.modes.Add("discard");
            _podium.SetViewController(_podiumViewController);
        }

        public void Dispose()
        {

        }
    }
}