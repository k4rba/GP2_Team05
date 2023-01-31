using System;
using System.Collections.Generic;
using UnityEngine;
using Personal.Andreas.Scripts.Util;

namespace Personal.Andreas.Scripts.Flowfield
{
    public struct FieldInfo
    {
        public Vector3 pos;

        public FieldInfo(Vector3 pos)
        {
            this.pos = pos;
        }
    }

    [Serializable]
    public class VectorFlowField2D
    {
        public int ChunkLength => ChunkSize * ChunkSize;

        [NonSerialized] public int ChunkSize = 5;
        [NonSerialized] public int TileSize = 1;

        [NonSerialized] public Rect Bounds;

        private List<FlowChunk> _chunkList;
        private Dictionary<Vector2Int, FlowChunk> _chunks;

        private List<Vector2Int> _visited;

        private int _updateWidth = 5;
        private int _updateHeight = 5;

        private int _maxDistance;
        private Rect _tileBounds;
        private Vector2Int _curStart;
        private int _currentPathId = 0;


        public List<FieldInfo> NewlySet;

        public VectorFlowField2D()
        {
            _visited = new List<Vector2Int>();
            _chunks = new Dictionary<Vector2Int, FlowChunk>();
            _chunkList = new();
            NewlySet = new();
        }

        private int GetHash(int x, int y)
        {
            return new Vector3(x, 0, y).GetHashCode();
        }

        private bool InBounds(int x, int y)
        {
            // return x >= 0 && x < _width && y >= 0 && y < _height;
            CoordinateHelper.WorldCoordsToChunkCoords(x, y, ChunkSize, out int cx, out int cy);
            //int h = GetHash(x, y); // (cx, cy).GetHashCode();
            return _chunks.ContainsKey(new Vector2Int(cx, cy));
        }

        public Vector2 GetFieldDirection(int x, int y)
        {
            var h = CoordinateHelper.WorldCoordsToHash(x, y, ChunkSize);
            CoordinateHelper.WorldCoordsToChunkCoords(x, y, ChunkSize, out int cx, out int cy);
            var ch = GetChunk(cx, cy);
            if(ch == null) return Vector2.zero;
            int i = CoordinateHelper.WorldCoordsToChunkTileIndex(x, y, ChunkSize, ch.IndexOffset);
            return ch.Field[i];
        }

        private void Clear()
        {
            _chunks.Clear();
            _chunkList.Clear();
            _visited.Clear();
        }

        private void AddChunk(FlowChunk chunk)
        {
            _chunks.Add(chunk.IndexOffset, chunk);
            _chunkList.Add(chunk);
        }

        public void Setup(FlowChunk[] chunks)
        {
            Clear();

            for(int i = 0; i < chunks.Length; i++)
            {
                var ch = chunks[i];
                AddChunk(ch);
            }

            // for(int i = 0; i < map.ChunkList.Count; i++)
            // {
            // var mapChunk = map.ChunkList[i];
            // SetupChunkBlocks(mapChunk);
            // }
        }

        // public void SetupChunkBlocks(byte tile)
        // {
        // FlowChunk flowChunk;
        // if(!_chunks.TryGetValue(ch.Hash, out flowChunk))
        // flowChunk = CreateChunk(ch.ChunkCoords.X, ch.ChunkCoords.Y);
        // flowChunk.Blocks = ch.Blocks;
        // }

        public void Reset(int sx, int ex, int sy, int ey)
        {
            int prevcx = 999999;
            int prevcy = 999999;

            FlowChunk chunk = null;

            for(int x = sx; x < ex; x++)
            for(int y = sy; y < ey; y++)
            {
                CoordinateHelper.WorldCoordsToChunkCoords(x, y, ChunkSize, out int cx, out int cy);
                if(cx != prevcx || cy != prevcy)
                    chunk = GetChunk(x, y);
                if(chunk == null)
                    continue;

                prevcx = cx;
                prevcy = cy;

                int i = CoordinateHelper.WorldCoordsToChunkTileIndex(x, y, ChunkSize, chunk.IndexOffset);
                chunk.Nodes[i].Reset();
            }

            _visited.Clear();
            _maxDistance = 0;
        }

        private bool HaveLineOfSight(int x, int y, int startx, int starty)
        {
            //  todo - PERFORAMNCE
            bool los = false;

            int xd = startx - x;
            int yd = starty - y;

            int xdabs = Mathf.Abs(xd);
            int ydabs = Mathf.Abs(yd);

            int xone = (int)Mathf.Sign(xd);
            int yone = (int)Mathf.Sign(yd);

            if(xdabs >= ydabs && GetNode(x + xone, y).LineOfSight)
                los = true;
            else if(ydabs >= xdabs && GetNode(x, y + yone).LineOfSight)
                los = true;

            if(ydabs > 0 && xdabs > 0)
            {
                if(!GetNode(x + xone, y + yone).LineOfSight)
                    los = false;
                else if(ydabs == xdabs)
                    if(GetBlock(x + xone, y) || GetBlock(x, y + yone))
                        los = false;
            }

            return los;
        }

        // private FlowChunk CreateChunk(int cx, int cy)
        // {
        // var ch = new FlowChunk(ChunkSize) { IndexOffset = new Vector2Int(cx, cy) };
        // int h = (cx, cy).GetHashCode();
        // _chunks.Add(h, ch);
        // _chunkList.Add(ch);
        // return ch;
        // }

        private FlowChunk GetChunk(Vector2Int index)
        {
            _chunks.TryGetValue(index, out var chunk);
            return chunk;
        }

        private FlowChunk GetChunk(int x, int y)
        {
            int h = CoordinateHelper.WorldCoordsToHash(x, y, ChunkSize);
            CoordinateHelper.WorldCoordsToChunkCoords(x, y, ChunkSize, out int cx, out int cy);
            return GetChunk(new Vector2Int(cx, cy));
        }

        private FlowChunk GetChunkWithC(int cx, int cy)
        {
            int h = GetHash(cx, cy); // new Vector3(cx, 0, cy).GetHashCode(); // (cx, cy).GetHashCode();
            _chunks.TryGetValue(new Vector2Int(cx, cy), out var chunk);
            return chunk;
        }

        private bool GetBlock(int x, int y)
        {
            var mapChunk = GetChunk(x, y);
            if(mapChunk == null) return false;
            var blockIdx = CoordinateHelper.WorldCoordsToChunkTileIndex(x, y, ChunkSize, mapChunk.IndexOffset);
            return mapChunk.Blocks[blockIdx];
        }

        private bool GetBlock(int x, int y, FlowChunk chunk)
        {
            var blockIdx = CoordinateHelper.WorldCoordsToChunkTileIndex(x, y, ChunkSize, chunk.IndexOffset);
            // return false;
            return chunk.Blocks[blockIdx];
        }

        private VNode GetNode(int x, int y)
        {
            var chunk = GetChunk(x, y);
            if(chunk == null)
                return VNode.Empty;
            int i = CoordinateHelper.WorldCoordsToChunkTileIndex(x, y, ChunkSize, chunk.IndexOffset);
            return chunk.Nodes[i];
        }

        private void SetNodeLos(int x, int y, bool los)
        {
            var ch = GetChunk(x, y);
            if(ch == null) return;
            int i = CoordinateHelper.WorldCoordsToChunkTileIndex(x, y, ChunkSize, ch.IndexOffset);
            ch.Nodes[i].LineOfSight = los;
        }

        private void SetNodeDistance(int x, int y, int distance, FlowChunk ch = null)
        {
            ch ??= GetChunk(x, y);
            if(ch == null) return;
            int i = CoordinateHelper.WorldCoordsToChunkTileIndex(x, y, ChunkSize, ch.IndexOffset);
            ch.Nodes[i].SetDistance((ushort)distance, (ushort)_currentPathId);
            // ch.Nodes[i].distance = (ushort)distance;
            // ch.Nodes[i].pathId = (ushort)currentPathId;
        }

        public void UpdateField(Vector2 worldPosition)
        {
            //  update area size


            int ts = TileSize;

            CoordinateHelper.PositionToWorldCoords(worldPosition.x, worldPosition.y, ts, out int startX,
                out int startY);

            int sx = startX - _updateWidth;
            int ex = startX + _updateWidth;
            int sy = startY - _updateHeight;
            int ey = startY + _updateHeight;

            int cx, cy;

            NewlySet.Clear();
            Reset(sx, ex, sy, ey);

            _currentPathId++;

            _curStart.x = startX;
            _curStart.y = startY;

            var startChunk = GetChunk(startX, startY);
            if(startChunk == null)
                return;

            Vector2 endPos = new Vector2(startX, startY);

            _tileBounds = new Rect(startX - _updateWidth, startY + _updateHeight, _updateWidth * 2, _updateHeight * 2);

            var size = new Vector2(_updateWidth * ts, _updateHeight * ts);
            Bounds = new Rect((int)(worldPosition.x - size.x), (int)(worldPosition.y - size.y), (int)size.x * 2,
                (int)size.y * 2);

            _visited.Add(new Vector2Int(startX, startY));

            Debug.Log($"tiles:   x{startX}({sx}-{ex}), y{startY}({sy}-{ey})");

            int starti =
                CoordinateHelper.WorldCoordsToChunkTileIndex(startX, startY, ChunkSize, startChunk.IndexOffset);
            var startNode = new VNode
            {
                Distance = 1,
                LineOfSight = true
            };

            startChunk.Nodes[starti] = startNode;
            startChunk.Field[starti] = Vector2Int.zero;

            FlowChunk chunk = null;
            int prevMapX = 99999, prevMapY = 99999;
            int prevCX = 999999, prevCY = 99999;
            FlowChunk neiChunk = null;

            for(int i = 0; i < _visited.Count; i++)
            {
                if(_visited.Count > 10000)
                {
                    Debug.LogWarning("Flowfield visits broke");
                    break;
                }

                var v = _visited[i];

                if(v.x == 4 && v.y == 0)
                {
                    
                }
                
                CoordinateHelper.WorldCoordsToChunkCoords(v.x, v.y, ChunkSize, out cx, out cy);
                if(cx != prevCX || cy != prevCY)
                    chunk = GetChunk(v.x, v.y);

                var currentNode = chunk.GetNode(v.x, v.y); // nodes[v.X, v.Y];
                if(v.x != startX || v.y != startY)
                {
                    SetNodeLos(v.x, v.y, HaveLineOfSight(v.x, v.y, startX, startY));
                }

                foreach(var n in GetNeis(v.x, v.y, neiChunk))
                {
                    CoordinateHelper.WorldCoordsToChunkCoords(n.x, n.y, ChunkSize, out int ncx, out int ncy);

                    //if(ncx != prevNeiX || ncy != prevNeiY)
                    neiChunk = GetChunkWithC(ncx, ncy);

                    if(GetBlock(n.x, n.y, neiChunk))
                        continue;

                    SetNodeDistance(n.x, n.y, currentNode.Distance + 1, neiChunk);
                    _visited.Add(n);
                }

                prevCX = cx;
                prevCY = cy;

                if(currentNode.Distance > _maxDistance)
                    _maxDistance = currentNode.Distance;
            }

            prevCX = 999999;
            prevCY = 99999;
            int prevNpX = 99999, prevNpY = 99999;
            prevMapX = 99999;
            prevMapY = 9999;

            for(int y = sy; y < ey; y++)
            for(int x = sx; x < ex; x++)
            {
                CoordinateHelper.WorldCoordsToChunkCoords(x, y, ChunkSize, out cx, out cy);
                if(cx != prevCX || cy != prevCY)
                    chunk = GetChunkWithC(cx, cy);
                prevCX = cx;
                prevCY = cy;
                if(chunk == null)
                    continue;

                int chunki = CoordinateHelper.WorldCoordsToChunkTileIndex(x, y, ChunkSize, chunk.IndexOffset);
                var currentNode = chunk.Nodes[chunki];
                // if(currentNode.PathId != _currentPathId)
                    // continue;

                int nearest = 5000;
                Vector2 dir = Vector2.zero;
                Vector2 tilePosition = new Vector2(x, y);

                foreach(var np in GetFlowNeis(x, y, neiChunk))
                {
                    CoordinateHelper.WorldCoordsToChunkCoords(np.x, np.y, ChunkSize, out int ncx, out int ncy);
                    if(ncx != prevNpX || ncy != prevNpY)
                        neiChunk = GetChunkWithC(ncx, ncy);
                    prevNpX = ncx;
                    prevNpY = ncy;
                    if(neiChunk == null)
                        continue;
                    var ne = neiChunk.GetNode(np.x, np.y);
                    int dis = ne.Distance - currentNode.Distance;
                    if(dis < nearest)
                    {
                        nearest = dis;
                        dir = (np.ToVector2() - tilePosition).normalized;
                    }
                }

                if(currentNode.LineOfSight)
                    dir = (endPos - tilePosition).normalized;

                chunk.Field[chunki] = chunk.Blocks[chunki] ? Vector2.zero : dir;

                var pos = new Vector3(
                    x + TileSize * .5f,
                    0,
                    y + TileSize * .5f);
                NewlySet.Add(new FieldInfo(pos));
            }

            // startNode.distance = 1;
            // SetNodeDistance(startX, startY, 1);
        }

        private FlowChunk GetChunkOrPrev(int x, int y, FlowChunk ch)
        {
            if(ch == null) return GetChunk(x, y);
            int px = ch.IndexOffset.x;
            int py = ch.IndexOffset.y;
            CoordinateHelper.WorldCoordsToChunkCoords(x, y, ChunkSize, out int cx, out int cy);
            if(cx != px || cy != py)
                ch = GetChunk(x, y);
            return ch;
        }

        private IEnumerable<Vector2Int> GetFlowNeis(int x, int y, FlowChunk chunk)
        {
            chunk = GetChunkOrPrev(x, y, chunk);

            if(FlowNei(x + 1, y, chunk))
            {
                yield return new Vector2Int(x + 1, y);
            }

            if(FlowNei(x - 1, y, chunk))
            {
                yield return new Vector2Int(x - 1, y);
            }

            if(FlowNei(x, y + 1, chunk))
            {
                yield return new Vector2Int(x, y + 1);
            }

            if(FlowNei(x, y - 1, chunk))
            {
                yield return new Vector2Int(x, y - 1);
            }

            if(FlowNeiDiag(x, y, Orientation.UP_RIGHT, chunk))
            {
                yield return new Vector2Int(x + 1, y - 1);
            }

            if(FlowNeiDiag(x, y, Orientation.DOWN_RIGHT, chunk))
            {
                yield return new Vector2Int(x + 1, y + 1);
            }

            if(FlowNeiDiag(x, y, Orientation.DOWN_LEFT, chunk))
            {
                yield return new Vector2Int(x - 1, y + 1);
            }

            if(FlowNeiDiag(x, y, Orientation.UP_LEFT, chunk))
            {
                yield return new Vector2Int(x - 1, y - 1);
            }
        }

        private bool FlowNei(int x, int y, FlowChunk c)
        {
            c = GetChunkOrPrev(x, y, c);
            if(c == null)
                return false;
            if(!InBounds(x, y))
                return false;
            if(GetBlock(x, y, c))
                return false;
            if(x >= _tileBounds.Right() || x < _tileBounds.Left())
                return false;
            if(y >= _tileBounds.Bottom() || y < _tileBounds.Top())
                return false;
            return true;
        }

        private bool FlowNeiDiag(int sx, int sy, Orientation di, FlowChunk c)
        {
            var dir = di.ToVector2();
            int x = sx + dir.x;
            int y = sy + dir.y;
            c = GetChunkOrPrev(x, y, c);

            if(c == null)
                return false;
            if(!InBounds(x, y))
                return false;
            if(GetBlock(x, y, c))
                return false;
            if(x >= _tileBounds.Right() || x < _tileBounds.Left())
                return false;
            if(y >= _tileBounds.Bottom() || y < _tileBounds.Top())
                return false;

            switch(di)
            {
                case Orientation.UP_LEFT:
                    if(!FlowNei(sx - 1, sy, c) || !FlowNei(sx, sy - 1, c))
                        return false;
                    break;

                case Orientation.UP_RIGHT:
                    if(!FlowNei(sx + 1, sy, c) || !FlowNei(sx, sy - 1, c))
                        return false;
                    break;

                case Orientation.DOWN_LEFT:
                    if(!FlowNei(sx - 1, sy, c) || !FlowNei(sx, sy + 1, c))
                        return false;
                    break;

                case Orientation.DOWN_RIGHT:
                    if(!FlowNei(sx + 1, sy, c) || !FlowNei(sx, sy + 1, c))
                        return false;
                    break;
            }

            return true;
        }

        public IEnumerable<Vector2Int> GetNeis(int x, int y, FlowChunk c)
        {
            if(NiceNode(x + 1, y, c))
                yield return new Vector2Int(x + 1, y);
            if(NiceNode(x - 1, y, c))
                yield return new Vector2Int(x - 1, y);
            if(NiceNode(x, y + 1, c))
                yield return new Vector2Int(x, y + 1);
            if(NiceNode(x, y - 1, c))
                yield return new Vector2Int(x, y - 1);
        }

        private bool NiceNode(int x, int y, FlowChunk c)
        {
            if(x > _tileBounds.Right() || x < _tileBounds.Left())
                return false;
            if(y > _tileBounds.Bottom() || y < _tileBounds.Top())
                return false;
            c = GetChunkOrPrev(x, y, c);
            if(c == null)
                return false;
            if(!InBounds(x, y))
                return false;
            if(c.GetNode(x, y).Distance > 0)
                return false;

            return true;
        }

        public List<FlowChunk> GetChunks() => _chunkList;
    }
}