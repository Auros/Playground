using Zenject;
using UnityEngine;

namespace Playground.Components
{
    public class KoBlock : MonoBehaviour
    {
        private Material _material = null!;
        private SpriteRenderer _noteArrowGlowRenderer = null!;

        protected void Awake()
        {
            var matBlockController = GetComponent<MaterialPropertyBlockController>();
            var renderer = matBlockController.renderers[0];
            _material = renderer.material;

            _noteArrowGlowRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();

            /*
            var v = Random.insideUnitSphere * 5;
            v.y = Mathf.Abs(v.y);
            gameObject.transform.position = v;
            Color = new Color(Random.Range(5f, 10f), Random.Range(5f, 10f), Random.Range(5f, 10f));*/
        }

        public Color Color
        {
            get => _material.color;
            set => _material.color = value;
        }

        public Color ArrowGlowColor
        {
            get => _noteArrowGlowRenderer.color;
            set => _noteArrowGlowRenderer.color = value;
        }


        [Inject]
        public void Construct()
        {

        }

        public class Pool : MonoMemoryPool<KoBlock> { /* Pool Initialization */ }
    }
}
