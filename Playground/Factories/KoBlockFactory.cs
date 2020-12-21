using Zenject;
using UnityEngine;
using Playground.Components;

namespace Playground.Factories
{
    public class KoBlockFactory : IFactory<KoBlock>
    {
        private readonly DiContainer _container;
        private readonly GameObject _blockPrefab;

        public KoBlockFactory(DiContainer container, [Inject(Id = "playground.block.prefab")] GameObject blockPrefab)
        {
            _container = container;
            _blockPrefab = blockPrefab;
        }

        public KoBlock Create()
        {
            GameObject gameObject = _container.InstantiatePrefab(_blockPrefab, Vector3.zero, Quaternion.identity, null);
            KoBlock koBlock = _container.InstantiateComponent<KoBlock>(gameObject);
            koBlock.transform.localRotation = Quaternion.identity;
            gameObject.name = $"{nameof(KoBlock)} (Factorized)";
            koBlock.transform.localPosition = Vector3.zero;
            koBlock.gameObject.SetActive(false);
            gameObject.SetActive(false);
            return koBlock;
        }

        //internal class Fact : PlaceholderFactory<KoBlock> { /* Placeholder */ }
    }
}