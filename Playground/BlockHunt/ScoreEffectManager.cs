using System;
using Zenject;
using UnityEngine;
using IPA.Utilities;
using System.Reflection;
using BeatSaberMarkupLanguage;

namespace Playground.BlockHunt
{
    internal class ScoreEffectManager : IInitializable, IDisposable
    {
        private readonly Sprite _sprite;
        private Material _material = null!;
        private readonly ShootingManager _shootingManager;
        private readonly FlyingSpriteEffect.Pool _flyingSpriteEffectPool;
        private readonly FireworkItemController.Pool _fireworkItemControllerPool;

        public ScoreEffectManager(ShootingManager shootingManager, FlyingSpriteEffect.Pool flyingSpriteEffectPool, FireworkItemController.Pool fireworkItemControllerPool)
        {
            _shootingManager = shootingManager;
            _material = Utilities.ImageResources.NoGlowMat;
            _flyingSpriteEffectPool = flyingSpriteEffectPool;
            _fireworkItemControllerPool = fireworkItemControllerPool;
            _sprite = Utilities.LoadSpriteRaw(Utilities.GetResource(Assembly.GetExecutingAssembly(), "Playground.Resources.0001.png"));
        }

        public void Initialize()
        {
            var sprEnumerator = _flyingSpriteEffectPool.InactiveItems.GetEnumerator();
            sprEnumerator.MoveNext();
            _material = new Material(sprEnumerator.Current.GetField<SpriteRenderer, FlyingSpriteEffect>("_spriteRenderer").material);
            
            var enumerator = _fireworkItemControllerPool.InactiveItems.GetEnumerator();
            
            enumerator.MoveNext();
            var firework = enumerator.Current;
            var particleRenderer = firework.GetField<ParticleSystem, FireworkItemController>("_particleSystem").GetComponent<ParticleSystemRenderer>();
            particleRenderer.sharedMaterial.renderQueue = 5000;

            _shootingManager.DidHitDuck += DidHit;
        }

        private void DidHit(Duck duck)
        {
            var pos = duck.transform.transform.position;
            var firework = _fireworkItemControllerPool.Spawn();
            firework.didFinishEvent += FireworkFinished;

            firework.transform.position = pos;
            firework.Fire();

            var sprite = _flyingSpriteEffectPool.Spawn();

            sprite.transform.position = pos;
            sprite.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            sprite.InitAndPresent(1f, new Vector3(pos.x, 0f, pos.z), Quaternion.identity, _sprite, _material, Color.white, false);
            sprite.didFinishEvent += SpriteFinished;
        }

        private void SpriteFinished(FlyingObjectEffect foe)
        {
            foe.didFinishEvent -= SpriteFinished;
            _flyingSpriteEffectPool.Despawn(foe as FlyingSpriteEffect);
        }

        private void FireworkFinished(FireworkItemController firework)
        {
            firework.didFinishEvent -= FireworkFinished;
            _fireworkItemControllerPool.Despawn(firework);
        }

        public void Dispose()
        {
            _shootingManager.DidHitDuck -= DidHit;
        }
    }
}