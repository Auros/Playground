using UnityEngine;
using SiraUtil.Tools;
using Playground.Factories;
using Playground.Components;

namespace Playground.BlockHunt
{
    internal class ShootingManager
    {
        private readonly KoGun _koGun;
        private readonly SiraLog _siraLog;

        public ShootingManager(SiraLog siraLog, KoGunFactory.Fact koGunFactory)
        {
            _siraLog = siraLog;
            _koGun = koGunFactory.Create();
            _koGun.transform.localScale *= 0.2f;
            _koGun.Mask = "DuckBody";

            _koGun.onHit = HitNote;
        }

        public void HitNote(GameObject obj)
        {
            var bird = obj.transform.parent.gameObject.GetComponent<Duck>();
            if (bird != null)
            {
                _siraLog.Info("Hit");
                bird.Deinit();
            }
        }
    }
}
