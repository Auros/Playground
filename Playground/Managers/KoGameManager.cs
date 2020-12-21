﻿using System;
using Zenject;
using Playground.Interfaces;
using System.Collections.Generic;

namespace Playground.Managers
{
    internal class KoGameManager : IKoGameManager, IInitializable, IDisposable
    {
        private readonly List<IKoGame> _koGames;

        public IKoGame? ActiveMode { get; private set; }

        public KoGameManager([InjectOptional] List<IKoGame> koGames)
        {
            _koGames = koGames;
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