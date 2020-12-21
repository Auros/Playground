using UnityEngine;
using Playground.Components;
using Playground.Interfaces;
using System.Collections.Generic;

namespace Playground.BlockHunt
{
    internal class BlockHuntGame : IKoGame
    {
        public string Name => "Block Hunt";

        private readonly KoBlock.Pool _koBlockPool;
        private readonly GameObject _blockHuntRoot;
        private readonly List<KoBlock> _environmentBlocks = new List<KoBlock>();

        public BlockHuntGame(KoBlock.Pool koBlockPool)
        {
            _blockHuntRoot = new GameObject("BlockHuntRoot");
            _koBlockPool = koBlockPool;

            _blockHuntRoot.transform.localPosition = Vector3.zero;
            _blockHuntRoot.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

            CreateLane(3, 10, 3);
            CreateLane(5, 20, 4);
            CreateLane(7, 35, 5);
        }

        private void CreateLane(int distance, int width, int height, float scale = 0.5f, float laneOffset = 0f, float floorOffset = 0.25f)
        {
            for (int i = 0; i < height; i++)
            {
                for (int q = 0; q < width; q++)
                {
                    var block = _koBlockPool.Spawn();
                    block.Color = Color.green;
                    var scaleValue = scale * 2f;
                    block.transform.SetParent(_blockHuntRoot.transform);
                    block.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
                    block.transform.localPosition = new Vector3((q - (width / 2) + laneOffset) * scale, i * scale + floorOffset, distance);
                    _environmentBlocks.Add(block);
                }
            }
        }

        public void Begin()
        {

        }

        public void Create()
        {

        }

        public void Destroy()
        {

        }

        public void Stop()
        {

        }
    }
}