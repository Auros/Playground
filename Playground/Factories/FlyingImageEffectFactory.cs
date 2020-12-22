using Zenject;
using SiraUtil;
using UnityEngine;
using IPA.Utilities;

namespace Playground.Factories
{
    public class FlyingImageEffectFactory : IFactory<FlyingSpriteEffect> 
    {
        private readonly DiContainer _container;
        private readonly SpriteRenderer _rendererTemplate;

        public FlyingImageEffectFactory(DiContainer container, FlickeringNeonSign flickeringNeonSign)
        {
            _container = container;

            _rendererTemplate = flickeringNeonSign.transform.parent.Find("BatLogo").GetComponent<SpriteRenderer>();
        }

        public FlyingSpriteEffect Create()
        {
            var flyingImage = _container.InstantiateComponentOnNewGameObject<FlyingSpriteEffect>($"{nameof(FlyingSpriteEffect)}");
            var renderer = new GameObject("Image").Clone(_rendererTemplate);
            //var renderer = _container.InstantiateComponentOnNewGameObject<SpriteRenderer>("Image");
            renderer.transform.SetParent(flyingImage.transform);

            flyingImage.SetField("_spriteRenderer", renderer);
            return flyingImage;
        }
    }
}