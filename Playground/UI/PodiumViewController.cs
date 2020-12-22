using System;
using UnityEngine;
using UnityEngine.UI;
using Playground.Interfaces;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace Playground.UI
{
    [ViewDefinition("Playground.Views.podium-view.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\podium-view.bsml")]
    internal class PodiumViewController : BSMLAutomaticViewController
    {
        public event Action<IKoGame?>? ModeSelected;
        public event Action<IKoGame>? PlayRequested;
        public event Action<IKoGame>? StopRequested;

        [UIValue("-")]
        protected object value = null!;

        [UIValue("modes")]
        public List<object> modes = new List<object>();

        private string _test = "None";
        [UIValue("test")]
        public string Test
        {
            get => _test;
            set
            {
                _test = value;
                NotifyPropertyChanged();
            }
        }

        private string _actionText = "Play";
        [UIValue("action-text")]
        public string ActionText
        {
            get => _actionText;
            set
            {
                _actionText = value;
                NotifyPropertyChanged();
            }
        }

        [UIComponent("play-stop-button")]
        protected Button playStopButton = null!;

        [UIComponent("dropdown")]
        protected RectTransform dropdownRect = null!;

        [UIAction("mode-selected")]
        protected void Selected(object value)
        {
            ModeSelected?.Invoke(value is IKoGame game ? game! : null);
            if (value is IKoGame koGame)
            {
                Test = koGame.Name;
                ActionText = "Play";
                playStopButton.interactable = true;
                return;
            }
            playStopButton.interactable = false;
            ActionText = "Play";
            Test = "None";
        }

        [UIAction("mode-format")]
        protected string Format(object value)
        {
            if (value is IKoGame koGame)
            {
                return koGame.Name;
            }
            return "None";
        }

        [UIAction("play")]
        protected void Play()
        {
            if (value is IKoGame koGame)
            {
                if (koGame.Playing)
                {
                    ActionText = "Play";
                    StopRequested?.Invoke(koGame);
                }
                else
                {
                    ActionText = "Stop";
                    PlayRequested?.Invoke(koGame);
                }
            }
        }
    }
}