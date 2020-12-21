using SiraUtil.Objects;
using System.Collections.Generic;
using Tweening;
using UnityEngine;

namespace Playground.Managers
{
    internal class DenyahBackNoteManager
    {
        private readonly GameObject _newParent;
        private readonly TweeningManager _tweeningManager;
        private readonly BloomFogEnvironment _bloomFogEnvironment;
        private readonly ObjectStateContainer _objectStateContainer;
        private readonly List<ObjectState> _objectStates;

        public DenyahBackNoteManager(TweeningManager tweeningManager, BloomFogEnvironment bloomFogEnvironment)
        {
            _newParent = new GameObject("DenyahBackNotes");

            _tweeningManager = tweeningManager;
            _bloomFogEnvironment = bloomFogEnvironment;

            _objectStates = new List<ObjectState>();
            var root = _bloomFogEnvironment.gameObject.transform;
            var note18 = root.Find("Note (18)");
            var note19 = root.Find("Note (19)");
            var arrow0 = root.Find("Arrow");
            var shadow2 = root.Find("Shadow (2)");
            var arrow2 = root.Find("Arrow (2)");
            var arrow3 = root.Find("Arrow (3)");
            var arrow1 = root.Find("Arrow (1)");

            _newParent.transform.SetParent(root);
            var newT = _newParent.transform;
            note18.SetParent(newT);
            note19.SetParent(newT);
            arrow0.SetParent(newT);
            shadow2.SetParent(newT);
            arrow2.SetParent(newT);
            arrow3.SetParent(newT);
            arrow1.SetParent(newT);

            _objectStates.Add(new ObjectState(note18));
            _objectStates.Add(new ObjectState(note19));
            _objectStates.Add(new ObjectState(arrow0));
            _objectStates.Add(new ObjectState(shadow2));
            _objectStates.Add(new ObjectState(arrow2));
            _objectStates.Add(new ObjectState(arrow3));
            _objectStates.Add(new ObjectState(arrow1));

            _objectStateContainer = new ObjectStateContainer(_newParent);
        }

        public void Yeet(float minDuration = 3f, float maxDuration = 5f, float height = 10f)
        {
            foreach (var denyah in _objectStates)
            {
                var duration = Random.Range(minDuration, maxDuration);
                _tweeningManager.AddTween(new Vector3Tween(denyah.pose.position, denyah.pose.position + new Vector3(0f, height, 0f), (val) =>
                {
                    denyah.transform.SetLocalPositionAndRotation(val, Quaternion.Euler(denyah.transform.localRotation.eulerAngles + new Vector3(0f, 0f, duration)));
                }, duration, EaseType.InCubic), denyah.transform.gameObject);
                _tweeningManager.AddTween(new Vector3Tween(denyah.pose.rotation.eulerAngles, denyah.pose.rotation.eulerAngles + new Vector3(duration * -1, duration, duration * -1), (val) =>
                {
                    denyah.transform.localRotation = Quaternion.Euler(val);
                }, duration, EaseType.InCubic), denyah.transform.gameObject);
            }
        }

        public void Revert(bool animated = false)
        {
            if (!animated)
            {
                _objectStateContainer.Revert();
                return;
            }
            foreach (var denyah in _objectStates)
            {
                _tweeningManager.AddTween(new Vector3Tween(denyah.transform.localPosition, denyah.pose.position, (val) =>
                {
                    denyah.transform.localPosition = val;
                }, 2f, EaseType.OutQuad), denyah.transform.gameObject);
                _tweeningManager.AddTween(new Vector3Tween(denyah.transform.localRotation.eulerAngles, denyah.pose.rotation.eulerAngles, (val) =>
                {
                    denyah.transform.localRotation = Quaternion.Euler(val);
                }, 2f, EaseType.OutQuad), denyah.transform.gameObject);
            }
        }
    }
}