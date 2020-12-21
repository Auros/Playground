﻿using System;
using Zenject;
using UnityEngine;
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
        private readonly JojoPodiumManager _jojoPodiumManager;
        private readonly PodiumViewController _podiumViewController;
        private readonly DenyahBackNoteManager _denyahBackNoteManager;

        public IKoGame? ActiveMode { get; private set; }

        public KoGameManager([InjectOptional] List<IKoGame> koGames, KoPodiumFactory.Fact podiumFactory, JojoPodiumManager jojoPodiumManager, PodiumViewController podiumViewController, DenyahBackNoteManager denyahBackNoteManager)
        {
            _koGames = koGames;
            _podium = podiumFactory.Create();
            _podium.transform.localPosition += new Vector3(0f, 0f, -1.5f);
            _podium.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0));
            if (_koGames == null)
            {
                _koGames = new List<IKoGame>();
            }
            _jojoPodiumManager = jojoPodiumManager;
            _podiumViewController = podiumViewController;
            _denyahBackNoteManager = denyahBackNoteManager;
        }

        public void Initialize()
        {
            _podiumViewController.modes.Add("discard");
            _podiumViewController.modes.Add(new BlockHunt.BlockHuntGame());
            _podium.SetViewController(_podiumViewController);

            _jojoPodiumManager.Init(_podium, 180f, 130f);
            _podiumViewController.ModeSelected += ModeSelected;
        }

        private void ModeSelected(IKoGame? ko)
        {
            if (ko is null)
            {
                _jojoPodiumManager.Return();
                _denyahBackNoteManager.Revert(true);
                return;
            }
            _jojoPodiumManager.MoveOutOfTheWay();
            _denyahBackNoteManager.Yeet(2.5f, 5, 15);
        }

        public void Dispose()
        {
            _podiumViewController.ModeSelected -= ModeSelected;
        }
    }
}