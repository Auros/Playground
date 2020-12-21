using Zenject;
using SiraUtil.Tools;
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

        [UIAction("mode-selected")]
        protected void ModeSelected(object value)
        {
            if (value is IKoGame koGame)
            {
                Test = koGame.Name;
                return;
            }
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
    }
}