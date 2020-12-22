using Zenject;
using Playground.Components;

namespace Playground.Factories
{
    public class KoGunFactory : IFactory<KoGun>
    {
        private readonly DiContainer _container;

        public KoGunFactory(DiContainer container)
        {
            _container = container;
        }

        public KoGun Create()
        {
            KoGun gun = _container.InstantiateComponentOnNewGameObject<KoGun>($"{nameof(KoGun)} (Factorized)");
            return gun;
        }

        internal class Fact : PlaceholderFactory<KoGun> { /* Placeholder */ }
    }
}