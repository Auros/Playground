using Zenject;
using UnityEngine;
using SiraUtil.Tools;
using Playground.Factories;
using Playground.Components;
using IPA.Utilities;

namespace Playground.BlockHunt
{
    internal class ShootingManager : ITickable
    {
        private bool _shootingEnabled;
        private bool _didRecentlyShoot;

        private bool _didFix;
        private readonly KoGun _koGun;
        private readonly SiraLog _siraLog;
        private readonly IVRPlatformHelper _vrPlatformHelper;
        private readonly MenuPlayerController _menuPlayerController;
        private readonly VRControllersInputManager _vrControllersInputManager;
        private readonly FireworkItemController.Pool _fireworkItemControllerPool;

        public ShootingManager(SiraLog siraLog, IVRPlatformHelper vrPlatformHelper, MenuPlayerController menuPlayerController, VRControllersInputManager vrControllersInputManager, FireworkItemController.Pool fireworkItemControllerPool, KoGunFactory.Fact koGunFactory)
        {
            _siraLog = siraLog;
            _vrPlatformHelper = vrPlatformHelper;
            _menuPlayerController = menuPlayerController;
            _vrControllersInputManager = vrControllersInputManager;
            _fireworkItemControllerPool = fireworkItemControllerPool;

            _koGun = koGunFactory.Create();
            _koGun.transform.localScale *= 0.2f;
            _koGun.Mask = "DuckBody";

            _koGun.onHit = HitNote;

            _koGun.transform.SetParent(_menuPlayerController.rightController.transform);

            

            Disable();
        }

        public void Enable()
        {
            _shootingEnabled = true;
            _koGun.gameObject.SetActive(true);

            if (!_didFix)
            {
                var enumerator = _fireworkItemControllerPool.InactiveItems.GetEnumerator();
                enumerator.MoveNext();
                var firework = enumerator.Current;
                var particleRenderer = firework.GetField<ParticleSystem, FireworkItemController>("_particleSystem").GetComponent<ParticleSystemRenderer>();
                particleRenderer.sharedMaterial.renderQueue = 5000;
                _didFix = true;
            }
        }

        public void Disable()
        {
            _shootingEnabled = false;
            _koGun.gameObject.SetActive(false);
        }

        public void HitNote(GameObject hit)
        {
            if (_shootingEnabled)
            {
                var duck = hit.transform.parent.gameObject.GetComponent<Duck>();
                if (duck != null)
                {
                    var pos = duck.transform.transform.position;
                    var firework = _fireworkItemControllerPool.Spawn();
                    firework.didFinishEvent += FireworkFinished;

                    firework.transform.position = pos;
                    firework.Fire();

                    _siraLog.Info("Hit");
                    duck.Deinit();
                }
            }
        }

        private void FireworkFinished(FireworkItemController firework)
        {
            firework.didFinishEvent -= FireworkFinished;
            _fireworkItemControllerPool.Despawn(firework);
        }

        public void Tick()
        {
            if (_shootingEnabled)
            {
                var triggerValue = _vrControllersInputManager.TriggerValue(_menuPlayerController.rightController.node);
                if (triggerValue > 0.5f && !_didRecentlyShoot)
                {
                    _didRecentlyShoot = true;
                    _koGun.Shoot();
                    _vrPlatformHelper.TriggerHapticPulse(_menuPlayerController.rightController.node, 0.1f, 0.7f, 0);
                }
                else if (triggerValue <= 0.5f)
                {
                    _didRecentlyShoot = false;
                }
            }
        }
    }
}