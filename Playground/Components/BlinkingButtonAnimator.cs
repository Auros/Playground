using HMUI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Playground.BlockHunt
{
    internal class BlinkingButtonAnimator : MonoBehaviour
    {
        private bool _toggle;
        private float _time = 0f;
        private ImageView _imageView = null!;
        private readonly float _cycleRate = 0.5f;
        private NoTransitionsButton _button = null!;

        private bool _blink;
        public bool Blink
        {
            get => _blink;
            set
            {
                _blink = value;
                if (!_blink)
                {
                    _imageView.color = NormalColor;
                }
            }
        }
        public Color NormalColor { get; set; }
        public Color BlinkingColor { get; set; }
        public Color SelectedColor { get; set; }

        protected void Awake()
        {
            _button = GetComponent<NoTransitionsButton>();
            _imageView = GetComponentInChildren<ImageView>();
            _button.selectionStateDidChangeEvent += SelectionChanged;
        }

        private void SelectionChanged(NoTransitionsButton.SelectionState state)
        {
            if (state == NoTransitionsButton.SelectionState.Normal)
            {
                _imageView.color = NormalColor;
            }
            else
            {
                _imageView.color = SelectedColor;
            }
        }

        protected void OnDestroy()
        {
            if (_button != null)
            {
                _button.selectionStateDidChangeEvent -= SelectionChanged;
            }
        }

        protected void Update()
        {
            _time += Time.deltaTime;
            if (Blink && _button.selectionState == NoTransitionsButton.SelectionState.Normal)
            {
                if (_time >= _cycleRate)
                {
                    _time = 0;
                    _toggle = !_toggle;

                    _imageView.color = !_toggle ? NormalColor : BlinkingColor;
                }
            }
        }
    }
}