using System;
using Zenject;
using UnityEngine;
using Playground.Components;

namespace Playground.BlockHunt
{
    internal class Duck : MonoBehaviour
    {
        public event Action<Duck>? DidEscape;
        public float EscapeHeight { get; set; } = 20f;
        public float Speed { get; set; } = 5f;

        private bool _active;
        private KoBlock? _body;
        private KoBlock.Pool _koBlockPool = null!;

        private float _cycleTime = 0f;
        private readonly float _cycleLength = 0.02f;

        private bool _goingRight = true;
        private int _decisions = 0;

        [Inject]
        protected void Construct(KoBlock.Pool koBlockPool)
        {
            _koBlockPool = koBlockPool;
        }

        public void Init(Vector3 startPos)
        {
            if (!_active)
            {
                _body = _koBlockPool.Spawn();
                _body.transform.SetParent(transform, false);

                _body.transform.localScale = Vector3.one;
                _body.transform.localPosition = Vector3.zero; 
                _body.transform.localRotation = Quaternion.identity;

                transform.localPosition = startPos;
                _body.Color = new Color(10f, 5f, 0f, 1f);
                _active = true;
                _cycleTime = 0;
            }
        }

        protected void Update()
        {
            if (_cycleTime >= _cycleLength)
            {
                if (_active)
                {
                    if (transform.localPosition.y >= EscapeHeight)
                    {
                        DidEscape?.Invoke(this);
                        Deinit();
                        return;
                    }
                    var ran = UnityEngine.Random.Range(0, 50);
                    if (_decisions > ran)
                    {
                        _decisions = 0;
                        _goingRight = ran % 2 == 0;
                    }
                    else
                    {
                        _decisions++;
                    }
                    transform.localPosition += new Vector3(_goingRight ? 0.1f : -0.1f, 0.1f, 0);
                }
                _cycleTime = 0;
            }
            _cycleTime += Time.deltaTime;
        }

        public void Deinit()
        {
            if (_active)
            {
                _koBlockPool.Despawn(_body!);
                _active = false;
            }
        }

        public class Pool : MonoMemoryPool<Duck> { /* Pool Initialization */ }
    }
}