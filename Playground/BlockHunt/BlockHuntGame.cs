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

        private readonly Duck.Pool _duckPool;
        private readonly KoBlock.Pool _koBlockPool;
        private readonly GameObject _blockHuntRoot;
        private readonly List<Duck> _activeDucks = new List<Duck>();
        private readonly List<KoBlock> _environmentBlocks = new List<KoBlock>();
        private readonly TweeningManager _tweeningManager;
        private bool _isActive = false;

        public BlockHuntGame(Duck.Pool duckPool, KoBlock.Pool koBlockPool, TweeningManager tweeningManager)
        {
            _blockHuntRoot = new GameObject("BlockHuntRoot");
            _tweeningManager = tweeningManager;
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

                    await SiraUtil.Utilities.AwaitSleep(10);
                }
            }
        }

        public void Begin()
        {

        }

        public void Create()
        {
            foreach (var block in _environmentBlocks)
            {
                _koBlockPool.Despawn(block);
            }
            _environmentBlocks.Clear();
            _ = CreateLane(3, 10, 3);
            _ = CreateLane(5, 20, 4);
            _ = CreateLane(7, 35, 5);
            _isActive = true;
        }

        public async void Destroy()
        {
            _isActive = false;
            foreach (var block in _environmentBlocks)
            {
                var pos = block.transform.localPosition;
                _tweeningManager.AddTween(new FloatTween(pos.y, pos.y + 50, (val) =>
                {
                    block.transform.localPosition = new Vector3(pos.x, val, pos.z);
                }, 1f, EaseType.InCubic), block.gameObject);
                await SiraUtil.Utilities.AwaitSleep(10);
            }
        }

        public void Stop()
        {
            foreach (var block in _environmentBlocks)
            {
                _environmentBlocks.Remove(block);
                _koBlockPool.Despawn(block);
            }
            _environmentBlocks.Clear();
        }


        private float _cycleTime = 0f;
        private readonly float _cycleLength = 2f;

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

                    duckA.Init(ran.transform.localPosition + new Vector3(0f, 0f, 1f));
                    duckB.Init(ran2.transform.localPosition + new Vector3(0f, 0f, 1f));

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