using System;
using Zenject;
using UnityEngine;
using SiraUtil.Tools;

namespace Playground.Components
{
    public class KoBullet : MonoBehaviour
    {
        public Color Color { get; set; }
        public string? NameMask { get; set; }
        public float DespawnDistance { get; set; } = 100f;

        public event Action<KoBullet, GameObject>? DidHit;
        public event Action<KoBullet>? DidDespawn;

        private float? _speed = null;

        private Vector3 _origin;
        private SiraLog _siraLog = null!;
        private KoBlock? _bulletBody = null!;
        private KoBlock.Pool _koBlockPool = null!;

        [Inject]
        protected void Construct(SiraLog siraLog, KoBlock.Pool koBlockPool)
        {
            _siraLog = siraLog;
            _koBlockPool = koBlockPool;
        }

        public void Launch(Vector3 pos, Quaternion rot, float speed)
        {
            _siraLog.Info("Launching Bullet");
            _origin = pos;
            _speed = speed;
            transform.position = pos;
            transform.rotation = rot;
            _bulletBody = _koBlockPool.Spawn();
            _bulletBody.Color = Color * 10f;
            _bulletBody.transform.SetParent(transform);
            _bulletBody.transform.localPosition = Vector3.zero;
            _bulletBody.transform.localRotation = Quaternion.identity;
            _bulletBody.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        }

        protected void Update()
        {
            if (!_speed.HasValue)
            {
                return;
            }

            gameObject.transform.Translate(0, 0, _speed.Value * Time.deltaTime, Space.Self); //.localPosition += Vector3.forward * _speed.Value * Time.deltaTime;
        }

        protected void FixedUpdate()
        {
            if (!_speed.HasValue || _bulletBody == null)
            {
                return;
            }

            if (Vector3.Distance(_origin, transform.position) >= DespawnDistance)
            {
                Deinit();
                DidDespawn?.Invoke(this);
                return;
            }

            Collider[] hits = Physics.OverlapBox(gameObject.transform.localPosition, _bulletBody!.transform.localScale * 2f, Quaternion.identity);

            bool doMask = NameMask != null;
            foreach (var hit in hits)
            {
                if (_bulletBody == null || hit.gameObject == _bulletBody!.gameObject || (doMask && hit.gameObject.name != NameMask))
                {
                    continue;
                }
                DidHit?.Invoke(this, hit.gameObject);
            }
        }

        public void Deinit()
        {
            _speed = null;
            if (_bulletBody != null)
            {
                _koBlockPool.Despawn(_bulletBody);
                _bulletBody = null;
            }
        }

        public class Pool : MonoMemoryPool<KoBullet> { /* Pool Initialization */ }
    }
}