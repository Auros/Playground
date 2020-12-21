using HMUI;
using Zenject;
using UnityEngine;
using BeatSaberMarkupLanguage.FloatingScreen;

namespace Playground.Components
{
    public class KoPodium : MonoBehaviour
    {
        private KoBlock _base = null!;
        private KoBlock _neck = null!;
        private KoBlock _face = null!;
        private FloatingScreen _floatingScreen = null!;

        [Inject]
        protected void Construct(KoBlock.Pool koBlockPool)
        {
            _face = koBlockPool.Spawn();
            _neck = koBlockPool.Spawn();
            _base = koBlockPool.Spawn();
            _face.transform.SetParent(transform);
            _neck.transform.SetParent(transform);
            _base.transform.SetParent(transform);

            transform.localPosition += new Vector3(0f, 0f, -1f);
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0));
            _face.transform.localPosition = new Vector3(0f, 0.95f, 0f);
            _face.transform.localRotation = new Quaternion(0.3826835f, 0f, 0f, 0.9238795f);
            _face.transform.localScale = new Vector3(2.5f, 2.5f, 0.25f);

            _neck.transform.localPosition = new Vector3(0f, 0.5f, 0f);
            _neck.transform.localScale = new Vector3(0.25f, 2f, 0.25f);

            _base.transform.localPosition = new Vector3(0f, 0.1f, 0f);
            _base.transform.localScale = new Vector3(1.5f, 0.25f, 1.5f);

            _floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(40f, 40f), false, Vector3.zero, Quaternion.identity);
            _floatingScreen.transform.SetParent(transform);

            _floatingScreen.transform.localPosition = new Vector3(0f, 1f, -0.05f);
            _floatingScreen.transform.localRotation = new Quaternion(0.3826835f, 0f, 0f, 0.9238795f);
        }

        public void SetViewController(ViewController viewController)
        {
            _floatingScreen.SetRootViewController(viewController, ViewController.AnimationType.In);
        }
    }
}