using Zenject;
using Tweening;
using UnityEngine;
using Playground.Components;
using Playground.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Playground.BlockHunt
{
    internal class BlockHuntGame : IKoGame, ITickable
    {
        public string Name => "Block Hunt";

        public bool Playing => _isActive;

        public bool Activating => throw new System.NotImplementedException();

        public bool Deactivating => throw new System.NotImplementedException();

        private bool _isActive = false;
        private readonly Duck.Pool _duckPool;
        private readonly GameObject _blockHuntRoot;
        private readonly KoBlock.Pool _koBlockPool;
        private readonly ShootingManager _shootingManager;
        private readonly TweeningManager _tweeningManager;
        private readonly List<Duck> _activeDucks = new List<Duck>();
        private readonly List<KoBlock> _environmentBlocks = new List<KoBlock>();

        private float _cycleTime = 0f;
        private readonly float _cycleLength = 2f;

        public BlockHuntGame(Duck.Pool duckPool, KoBlock.Pool koBlockPool, ShootingManager shootingManager, TweeningManager tweeningManager)
        {
            _blockHuntRoot = new GameObject("BlockHuntRoot");
            _tweeningManager = tweeningManager;
            _shootingManager = shootingManager;
            _koBlockPool = koBlockPool;
            _duckPool = duckPool;

            _blockHuntRoot.transform.localPosition = Vector3.zero;
            _blockHuntRoot.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }

        private async Task CreateLane(int distance, int width, int height, float scale = 0.5f, float laneOffset = 0f, float floorOffset = 0.25f)
        {
            for (int i = 0; i < height; i++)
            {
                for (int q = 0; q < width; q++)
                {
                    var block = _koBlockPool.Spawn();
                    block.transform.localPosition = new Vector3(0f, -10f, 0f);
                    block.Color = new Color(0f, 5f, 0f);
                    var scaleValue = scale * 2f;
                    block.transform.SetParent(_blockHuntRoot.transform);
                    block.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
                    //block.transform.localPosition = new Vector3((q - (width / 2) + laneOffset) * scale, i * scale + floorOffset, distance);
                    var endVec = new Vector3((q - (width / 2) + laneOffset) * scale, i * scale + floorOffset, distance);
                    var newVec = endVec + new Vector3(0f, 50f, 0f);

                    _tweeningManager.AddTween(new FloatTween(newVec.y, endVec.y, (val) =>
                    {
                        block.transform.localPosition = new Vector3(endVec.x, val, endVec.z);
                    }, 1f, EaseType.OutCubic), block.gameObject);
                    
                    _environmentBlocks.Add(block);

                    await SiraUtil.Utilities.AwaitSleep(5);
                }
            }
        }

        public void Begin()
        {
            _isActive = true;
            _shootingManager.Enable();
        }

        public void Create()
        {
            foreach (var block in _environmentBlocks)
            {
                _koBlockPool.Despawn(block);
            }
            _environmentBlocks.Clear();
            _ = CreateLane(7, 15, 3);
            _ = CreateLane(9, 25, 4);
            _ = CreateLane(13, 55, 6);
        }

        public async void Destroy()
        {
            _isActive = false;
            foreach (var block in _environmentBlocks)
            {
                DespawnNote(block);
                await SiraUtil.Utilities.AwaitSleep(5);
            }
            _environmentBlocks.Clear();
        }

        public void Stop()
        {
            _isActive = false;
            _shootingManager.Disable();
        }

        private void DespawnNote(KoBlock block)
        {
            var pos = block.transform.localPosition;
            var tween = _tweeningManager.AddTween(new FloatTween(pos.y, pos.y + 50, (val) =>
            {
                block.transform.localPosition = new Vector3(pos.x, val, pos.z);
            }, 1f, EaseType.InCubic), block.gameObject);
            tween.onCompleted = delegate ()
            {
                _koBlockPool.Despawn(block);
            };
        }

        public void Tick()
        {
            if (_isActive)
            {
                if (_cycleTime >= _cycleLength)
                {
                    var ran = _environmentBlocks[Random.Range(0, _environmentBlocks.Count)];
                    var ran2 = _environmentBlocks[Random.Range(0, _environmentBlocks.Count)];

                    var duckA = _duckPool.Spawn();
                    var duckB = _duckPool.Spawn();

                    duckA.transform.SetParent(_blockHuntRoot.transform);
                    duckB.transform.SetParent(_blockHuntRoot.transform);

                    duckA.Init(ran.transform.localPosition + new Vector3(0f, 0f, 2f));
                    duckB.Init(ran2.transform.localPosition + new Vector3(0f, 0f, 2f));

                    duckA.DidEscape += DidEscape;
                    duckB.DidEscape += DidEscape;

                    _cycleTime = 0;
                }
                _cycleTime += Time.deltaTime;
            }
        }

        private void DidEscape(Duck duck)
        {
            duck.DidEscape -= DidEscape;
            _activeDucks.Remove(duck);
            _duckPool.Despawn(duck);
        }
    }
}