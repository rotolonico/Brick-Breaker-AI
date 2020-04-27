using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class BlocksHandler : MonoBehaviour
    {
        public static BlocksHandler Instance;

        private Transform[] blocks;

        private void Awake() => Instance = this;

        private void Start() => StartCoroutine(InitializeBlocks());

        public IEnumerator InitializeBlocks()
        {
            blocks = null;
            yield return null;
            blocks = GameObject.FindGameObjectsWithTag("HorizontalWall").Select(g => g.transform).ToArray();
        }

        public float[] GetBlocksRelativePosition(Vector2 position)
        {
            if (blocks == null) return new[] {0f, 0f};
            var leftBlocks = blocks.Count(block => block.position.x < position.x);
            var rightBlocks = blocks.Length - leftBlocks;
            return blocks.Length != 0
                ? new[] {(float) leftBlocks / blocks.Length, (float) rightBlocks / blocks.Length}
                : new[] {0f, 0f};
        }
    }
}