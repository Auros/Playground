using Zenject;
using Playground.Components;

namespace Playground.Factories
{
    public class KoBulletFactory : IFactory<KoBullet>
    {
        private readonly DiContainer _container;

        public KoBulletFactory(DiContainer container)
        {
            _container = container;
        }

        public KoBullet Create()
        {
            KoBullet bullet = _container.InstantiateComponentOnNewGameObject<KoBullet>($"{nameof(KoBullet)} (Factorized)");
            return bullet;
        }
    }
}