using UnityEngine;
using Playground.Components;
using Playground.Interfaces;
using System.Collections.Generic;
using Tweening;
using System.Threading.Tasks;

namespace Playground.BlockHunt
{
    internal class BlockHuntGame : IKoGame
    {
        public string Name => "Block Hunt";

        private readonly KoBlock.Pool _koBlockPool;
        private readonly GameObject _blockHuntRoot;
        private readonly List<KoBlock> _environmentBlocks = new List<KoBlock>();
        private readonly TweeningManager _tweeningManager;

        public BlockHuntGame(KoBlock.Pool koBlockPool, TweeningManager tweeningManager)
        {
            _blockHuntRoot = new GameObject("BlockHuntRoot");
            _tweeningManager = tweeningManager;
            _koBlockPool = koBlockPool;

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
                    block.Color = Color.green;
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
        }

        public async void Destroy()
        {
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
    }
}