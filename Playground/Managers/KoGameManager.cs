using System;
using Zenject;
using Playground.Factories;
using Playground.Interfaces;
using Playground.Components;
using System.Collections.Generic;

namespace Playground.Managers
{
    internal class KoGameManager : IKoGameManager, IInitializable, IDisposable
    {
        private KoPodium _podium = null!;
        private readonly List<IKoGame> _koGames;

        public IKoGame? ActiveMode { get; private set; }

        public KoGameManager([InjectOptional] List<IKoGame> koGames, KoPodiumFactory.Fact podiumFactory)
        {
            _koGames = koGames;
            _podium = podiumFactory.Create();
            if (_koGames == null)
            {
                _koGames = new List<IKoGame>();
            }
        }

        public void Initialize()
        {
            
        }

        public void Dispose()
        {

        }
    }
}