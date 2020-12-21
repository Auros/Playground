using Zenject;
using Playground.Components;

namespace Playground.Factories
{
    public class KoPodiumFactory : IFactory<KoPodium>
    {
        private readonly DiContainer _container;

        public KoPodiumFactory(DiContainer container)
        {
            _container = container;
        }

        public KoPodium Create()
        {
            KoPodium podium = _container.InstantiateComponentOnNewGameObject<KoPodium>($"{nameof(KoPodium)} (Factorized)");
            return podium;
        }

        internal class Fact : PlaceholderFactory<KoPodium> { /* Placeholder */ }
    }
}