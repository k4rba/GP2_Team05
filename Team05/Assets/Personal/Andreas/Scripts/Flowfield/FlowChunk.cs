using Personal.Andreas.Scripts.Util;
using UnityEngine;

namespace Personal.Andreas.Scripts.Flowfield
{
    public class FlowChunk
    {
        public Vector2[] Field;
        public VNode[] Nodes;
        public bool[] Blocks;
        public Vector2Int Offset;

        private byte _size;

        public FlowChunk(int size)
        {
            int length = size * size;
            Nodes = new VNode[length];
            Field = new Vector2[length];
            Blocks = new bool[length];
        }

        public VNode GetNode(int x, int y)
        {
            int i = CoordinateHelper.WorldCoordsToChunkTileIndex(x, y, _size, Offset);
            return Nodes[i];
        }
    }
}