using UnityEngine;
using Util;

namespace FlowFieldSystem
{
    public class FlowChunk
    {
        public Vector2[] Field;
        public VNode[] Nodes;
        public bool[] Blocks;
        public Vector2Int IndexOffset;
        
        private int _size;

        public FlowChunk(int size)
        {
            int length = size * size;
            _size = size;
            Nodes = new VNode[length];
            Field = new Vector2[length];
            Blocks = new bool[length];
        }

        public VNode GetNode(int x, int y)
        {
            int i = CoordinateHelper.WorldCoordsToChunkTileIndex(x, y, _size, IndexOffset);
            return Nodes[i];
        }
    }
}