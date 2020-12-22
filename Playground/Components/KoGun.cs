using Zenject;
using UnityEngine;
using SiraUtil.Tools;
using System;

namespace Playground.Components
{
    public class KoGun : MonoBehaviour
    {
        private SiraLog _siraLog = null!;
        private KoBlock _barrelTip = null!;
        private KoBlock.Pool _koBlockPool = null!;
        private KoBullet.Pool _koBulletPool = null!;

        public string? Mask { get; set; }

        public Action<GameObject>? onHit;

        [Inject]
        protected void Construct(SiraLog siraLog, KoBlock.Pool koBlockPool, KoBullet.Pool koBulletPool)
        {
            _siraLog = siraLog;
            _koBlockPool = koBlockPool;
            _koBulletPool = koBulletPool;

            var barrel1 = _koBlockPool.Spawn();
            var barrel2 = _koBlockPool.Spawn();
            var barrel3 = _koBlockPool.Spawn();
            var barrel4 = _koBlockPool.Spawn();

            var trigger = _koBlockPool.Spawn();
            var handle1 = _koBlockPool.Spawn();
            var handle2 = _koBlockPool.Spawn();

            barrel1.transform.SetParent(transform);
            barrel2.transform.SetParent(transform);
            barrel3.transform.SetParent(transform);
            barrel4.transform.SetParent(transform);

            trigger.transform.SetParent(transform);
            handle1.transform.SetParent(transform);
            handle2.transform.SetParent(transform);

            barrel1.transform.localPosition += new Vector3(0f, 0f, 1.5f);
            barrel2.transform.localPosition += new Vector3(0f, 0f, 1.0f);
            barrel3.transform.localPosition += new Vector3(0f, 0f, 0.5f);

            trigger.transform.localPosition += new Vector3(0f, -0.45f, 0.45f);
            trigger.transform.localScale *= 0.5f;

            handle1.transform.localPosition += new Vector3(0f, -0.5f, 0f);
            handle2.transform.localPosition += new Vector3(0f, -1f, 0f);

            _barrelTip = barrel1;
        }

        public void Shoot()
        {
            var bullet = _koBulletPool.Spawn();
            bullet.Color = Color.red;

            bullet.NameMask = Mask;
            bullet.Launch(_barrelTip.transform.position, _barrelTip.transform.rotation, 25f);
            bullet.DidDespawn += Bullet_DidDespawn;
            bullet.DidHit += Bullet_DidHit;
        }

        private void Bullet_DidHit(KoBullet bullet, GameObject go)
        {
            bullet.Deinit();
            onHit?.Invoke(go);
            Bullet_DidDespawn(bullet);
        }

        private void Bullet_DidDespawn(KoBullet bullet)
        {
            bullet.DidDespawn -= Bullet_DidDespawn;
            bullet.DidHit -= Bullet_DidHit;
            _koBulletPool.Despawn(bullet);
        }
    }
}