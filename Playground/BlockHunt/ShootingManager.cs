using Zenject;
using UnityEngine;
using SiraUtil.Tools;
using Playground.Factories;
using Playground.Components;

namespace Playground.BlockHunt
{
    internal class ShootingManager : ITickable
    {
        private bool _shootingEnabled;
        private bool _didRecentlyShoot;

        private readonly KoGun _koGun;
        private readonly SiraLog _siraLog;
        private readonly IVRPlatformHelper _vrPlatformHelper;
        private readonly MenuPlayerController _menuPlayerController;
        private readonly VRControllersInputManager _vrControllersInputManager;

        public ShootingManager(SiraLog siraLog, IVRPlatformHelper vrPlatformHelper, MenuPlayerController menuPlayerController, VRControllersInputManager vrControllersInputManager, KoGunFactory.Fact koGunFactory)
        {
            _siraLog = siraLog;
            _vrPlatformHelper = vrPlatformHelper;
            _menuPlayerController = menuPlayerController;
            _vrControllersInputManager = vrControllersInputManager;

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
                    _siraLog.Info("Hit");
                    duck.Deinit();
                }
            }
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
                }
                else if (triggerValue <= 0.5f)
                {
                    _didRecentlyShoot = false;
                }
            }
        }
    }
}