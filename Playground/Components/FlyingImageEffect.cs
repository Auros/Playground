using BeatSaberMarkupLanguage;
using HMUI;

namespace Playground.Components
{
    internal class FlyingImageEffect : FlyingObjectEffect
    {
        private ImageView _imageView = null!;

        protected void Awake()
        {
            _imageView = gameObject.AddComponent<ImageView>();
            _imageView.SetImage("Playground.Resources.AmeSpin.gif");
            //_imageView.material = Utilities.ImageResources.NoGlowMat;
        }

        protected override void ManualUpdate(float t)
        {

        }
    }
}
