using Zenject;

namespace Playground.BlockHunt
{
    internal class DuckFactory : IFactory<Duck> 
    {
        private readonly DiContainer _container;

        public DuckFactory(DiContainer container)
        {
            _container = container;
        }

        public Duck Create()
        {
            Duck duck = _container.InstantiateComponentOnNewGameObject<Duck>();
            duck.gameObject.name = $"{nameof(Duck)} (Factorized)";
            duck.gameObject.SetActive(false);
            return duck;
        }
    }
}