using Tweening;
using UnityEngine;
using Playground.Components;

namespace Playground.Managers
{
    internal class JojoPodiumManager
    {
        private KoPodium _podium = null!;
        private readonly TweeningManager _tweeningManager;

        private float inRot = 180f;
        private float outRot = 130f;

        public JojoPodiumManager(TweeningManager tweeningManager)
        {
            _tweeningManager = tweeningManager;
        }

        public void Init(KoPodium podium, float inRot, float outRot)
        {
            _podium = podium;
            this.inRot = inRot;
            this.outRot = outRot;
        }

        public void MoveOutOfTheWay()
        {
            _tweeningManager.AddTween(new FloatTween(0f, 3f, (val) =>
            {
                _podium.transform.localPosition = new Vector3(val, _podium.transform.localPosition.y, _podium.transform.localPosition.z);
            }, 1f, EaseType.OutQuart), _podium.gameObject);
            _tweeningManager.AddTween(new FloatTween(inRot, outRot, (val) =>
            {
                _podium.transform.localRotation = Quaternion.Euler(0f, val, 0f);
            }, 1f, EaseType.OutQuart), _podium.gameObject);
        }

        public void Return()
        {
            _tweeningManager.AddTween(new FloatTween(3f, 0f, (val) =>
            {
                _podium.transform.localPosition = new Vector3(val, _podium.transform.localPosition.y, _podium.transform.localPosition.z); ;
            }, 1f, EaseType.OutQuart), _podium.gameObject);
            _tweeningManager.AddTween(new FloatTween(outRot, inRot, (val) =>
            {
                _podium.transform.localRotation = Quaternion.Euler(0f, val, 0f);
            }, 1f, EaseType.OutQuart), _podium.gameObject);
        }
    }
}