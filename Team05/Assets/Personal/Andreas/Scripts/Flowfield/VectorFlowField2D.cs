using System.Collections.Generic;
using Personal.Andreas.Scripts.Util;
using UnityEngine;

namespace Personal.Andreas.Scripts.Flowfield
{
    public class VectorFlowField2D
    {
        public int ChunkSize = 5;
        public int ChunkLength => ChunkSize * ChunkSize;

        public int MaxDistance;
        public Rect Bounds;

        Dictionary<int, FlowChunk> _chunks;
        List<Vector2Int> _visited;

        Rect _tileBounds;
        Vector2Int _curStart;
        int _currentPathId = 0;

        public VectorFlowField2D()
        {
            _visited = new List<Vector2Int>();
            _chunks = new Dictionary<int, FlowChunk>();
        }

        private bool InBounds(int x, int y)
        {
            CoordinateHelper.WorldCoordsToChunkCoords(x, y, ChunkSize, out int cx, out int cy);
            int h = (cx, cy).GetHashCode();
            return _chunks.ContainsKey(h);
        }

        public Vector2 GetFieldDirection(int x, int y)
        {
            var h = CoordinateHelper.WorldCoordsToHash(x, y, ChunkSize);
            var ch = GetChunk(h);
            if(ch == null) return Vector2.zero;
            int i = CoordinateHelper.WorldCoordsToChunkTileIndex(x, y, ChunkSize, ch.Offset);
            return ch.Field[i];
        }

        private void Clear()
        {
            _chunks.Clear();
            _visited.Clear();
        }

        public void SetupAllBlocks()
        {
            Clear();
            // for(int i = 0; i < map.ChunkList.Count; i++)
            // {
            // var mapChunk = map.ChunkList[i];
            // SetupChunkBlocks(mapChunk);
            // }
        }

        public void SetupChunkBlocks()
        {
            FlowChunk flowChunk;
            // if(!_chunks.TryGetValue(ch.Hash, out flowChunk))
            // flowChunk = CreateChunk(ch.ChunkCoords.X, ch.ChunkCoords.Y);
            // flowChunk.Blocks = ch.Blocks;
        }

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

                int i = CoordinateHelper.WorldCoordsToChunkTileIndex(x, y, ChunkSize, chunk.Offset);
                chunk.Nodes[i].Reset();
            }

            _visited.Clear();
            MaxDistance = 0;
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

        private FlowChunk CreateChunk(int cx, int cy)
        {
            var ch = new FlowChunk(ChunkSize) {Offset = new Vector2Int(cx, cy)};
            int h = (cx, cy).GetHashCode();
            _chunks.Add(h, ch);
            // chunkList.Add(ch);
            return ch;
        }

        private FlowChunk GetChunk(int h)
        {
            _chunks.TryGetValue(h, out var chunk);
            return chunk;
        }

        private FlowChunk GetChunk(int x, int y)
        {
            int h = CoordinateHelper.WorldCoordsToHash(x, y, ChunkSize);
            return GetChunk(h);
        }

        private FlowChunk GetChunkWithC(int cx, int cy)
        {
            int h = (cx, cy).GetHashCode();
            _chunks.TryGetValue(h, out var chunk);
            return chunk;
        }

        private bool GetBlock(int x, int y)
        {
            var mapChunk = GetChunk(x, y);
            if(mapChunk == null) return false;
            var blockIdx = CoordinateHelper.WorldCoordsToChunkTileIndex(x, y, ChunkSize, mapChunk.Offset);
            return mapChunk.Blocks[blockIdx];
        }

        private bool GetBlock(int x, int y, FlowChunk chunk)
        {
            var blockIdx = CoordinateHelper.WorldCoordsToChunkTileIndex(x, y, ChunkSize, chunk.Offset);
            // return false;
            return chunk.Blocks[blockIdx];
        }

        private VNode GetNode(int x, int y)
        {
            var chunk = GetChunk(x, y);
            if(chunk == null)
                return VNode.Empty;
            int i = CoordinateHelper.WorldCoordsToChunkTileIndex(x, y, ChunkSize, chunk.Offset);
            return chunk.Nodes[i];
        }

        private void SetNodeLos(int x, int y, bool los)
        {
            var ch = GetChunk(x, y);
            if(ch == null) return;
            int i = CoordinateHelper.WorldCoordsToChunkTileIndex(x, y, ChunkSize, ch.Offset);
            ch.Nodes[i].LineOfSight = los;
        }

        private void SetNodeDistance(int x, int y, int distance, FlowChunk ch = null)
        {
            ch ??= GetChunk(x, y);
            if(ch == null) return;
            int i = CoordinateHelper.WorldCoordsToChunkTileIndex(x, y, ChunkSize, ch.Offset);
            ch.Nodes[i].SetDistance((ushort)distance, (ushort)_currentPathId);
            // ch.Nodes[i].distance = (ushort)distance;
            // ch.Nodes[i].pathId = (ushort)currentPathId;
        }

        public void UpdateField(Vector2 p)
        {
            
            //  update area size
            const int w = 18;
            const int h = 10;
            
            int ts = ChunkSize;

            CoordinateHelper.PositionToWorldCoords(p.x, p.y, ts, out int startX, out int startY);

            // int startX = (int)(p.X / ts);
            // int startY = (int)(p.Y / ts);

            int sx = startX - w;
            int ex = startX + w;
            int sy = startY - h;
            int ey = startY + h;

            int cx, cy;

            Reset(sx, ex, sy, ey);

            _currentPathId++;

            _curStart.x = startX;
            _curStart.y = startY;

            var startChunk = GetChunk(startX, startY);
            if(startChunk == null)
                return;

            Vector2 endPos = new Vector2(startX, startY);

            _tileBounds = new Rect(startX - w, startY - h, w * 2, h * 2);

            var size = new Vector2(w * ts, h * ts);
            Bounds = new Rect((int)(p.x - size.x), (int)(p.y - size.y), (int)size.x * 2, (int)size.y * 2);

            _visited.Add(new Vector2Int(startX, startY));

            int starti = CoordinateHelper.WorldCoordsToChunkTileIndex(startX, startY, ChunkSize, startChunk.Offset);
            var startNode = new VNode()
            {
                Distance = 1,
                LineOfSight = true
            };

            startChunk.Nodes[starti] = startNode;
            startChunk.Field[starti] = Vector2Int.zero;

            FlowChunk chunk = null;
            int prevMapX = 99999, prevMapY = 9999;
            int prevCX = 999999, prevCY = 99999;
            FlowChunk neiChunk = null;

            for(int i = 0; i < _visited.Count; i++)
            {
                var v = _visited[i];
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

                if(currentNode.Distance > MaxDistance)
                    MaxDistance = currentNode.Distance;
            }

            prevCX = 999999;
            prevCY = 99999;
            int prevNpX = 99999, prevNpY = 99999;
            prevMapX = 99999;
            prevMapY = 9999;

            for(int X = sx; X < ex; X++)
            for(int Y = sy; Y < ey; Y++)
            {
                CoordinateHelper.WorldCoordsToChunkCoords(X, Y, ChunkSize, out cx, out cy);
                if(cx != prevCX || cy != prevCY)
                    chunk = GetChunkWithC(cx, cy);
                prevCX = cx;
                prevCY = cy;
                if(chunk == null)
                    continue;

                int chunki = CoordinateHelper.WorldCoordsToChunkTileIndex(X, Y, ChunkSize, chunk.Offset);
                var currentNode = chunk.Nodes[chunki];
                if(currentNode.PathId != _currentPathId)
                    continue;

                int nearest = 5000;
                Vector2 dir = Vector2.zero;
                Vector2 pos = new Vector2(X, Y);

                foreach(var np in getFlowNeis(X, Y, neiChunk))
                {
                    CoordinateHelper.WorldCoordsToChunkCoords(np.x, np.y,ChunkSize, out int ncx, out int ncy);
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
                        dir = (np.ToVector2() - pos).normalized;
                    }
                }

                if(currentNode.LineOfSight)
                    dir = (endPos - pos).normalized;

                chunk.Field[chunki] = chunk.Blocks[chunki] ? Vector2.zero : dir;
            }

            // startNode.distance = 1;
            // SetNodeDistance(startX, startY, 1);
        }

        private FlowChunk GetChunkOrPrev(int x, int y, FlowChunk ch)
        {
            if(ch == null) return GetChunk(x, y);
            int px = ch.Offset.x;
            int py = ch.Offset.y;
            CoordinateHelper.WorldCoordsToChunkCoords(x, y, ChunkSize, out int cx, out int cy);
            if(cx != px || cy != py)
                ch = GetChunk(x, y);
            return ch;
        }

        private IEnumerable<Vector2Int> getFlowNeis(int x, int y, FlowChunk chunk)
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
            // if(x >= _tileBounds.Right || x < _tileBounds.Left)
                // return false;
            // if(y >= _tileBounds.Bottom || y < _tileBounds.Top)
                // return false;
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
            // if(x >= _tileBounds.Right || x < _tileBounds.Left)
                // return false;
            // if(y >= _tileBounds.Bottom || y < _tileBounds.Top)
                // return false;

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
        
    }
}