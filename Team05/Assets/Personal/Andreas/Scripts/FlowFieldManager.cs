using System.Collections.Generic;
using System.Diagnostics;
using Andreas.Scripts;
using Personal.Andreas.Scripts;
using UnityEngine;
using Util;
using Debug = UnityEngine.Debug;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FlowFieldSystem
{
    public class FlowFieldManager : MonoBehaviour
    {
        [SerializeField] private GameObject _ground;
        [SerializeField] private GameObject _obstacles;
        [Space(10)]
        [SerializeField] private VectorFlowField2D _field;
        [SerializeField] private Transform _unit;
        [Space(10)]
        [SerializeField] private float _height = 0f;
        [SerializeField] private bool _drawIndexes = false;
        [SerializeField] private bool _drawTiles = false;
        [SerializeField] private bool _drawChunks = true;
        [SerializeField] private bool _reload = true;

        public FlowAgentManagerNew AgentManager;

        private bool _prevReload;

        private Vector2Int prevPos;

        private void Start()
        {
            _prevReload = _reload;
        }

        public VectorFlowField2D GetField() => _field;

        public List<FlowChunk> GetFlowChunks()
        {
            return _field.GetChunks();
        }

        public Transform GetUnit() => _unit;

        public void SetupFromPlayer(GameObject grounds, GameObject obstacles, Transform unit,
            FlowAgentManagerNew agentManager)
        {
            _ground = grounds;
            _obstacles = obstacles;
            _unit = unit;
            AgentManager = agentManager;

            Debug.Log("Generating FlowField");
#if UNITY_EDITOR
            var sw = Stopwatch.StartNew();
#endif
            GenerateFlowField();
#if UNITY_EDITOR
            sw.Stop();
            Debug.Log($"FlowField generated - {sw.Elapsed.TotalMilliseconds}ms");
#else
            Debug.Log($"FlowField generated");
#endif

            var spawner = EnemyManager.Get.Spawner;
            spawner.AssignFlowField(this);
        }

        public void GenerateFlowField()
        {
            _field.Clear();
            //  ground
            var groundColliders = _ground.GetComponentsInChildren<Collider>();

            foreach(var col in groundColliders)
            {
                var bounds = col.bounds;

                int sx = (int)bounds.min.x;
                int ex = (int)bounds.max.x;
                int sy = (int)bounds.min.z;
                int ey = (int)bounds.max.z;

                AddChunksInArea(sx, ex, sy, ey);
            }

            var obsColliders = _obstacles.GetComponentsInChildren<Collider>();

            foreach(var col in obsColliders)
            {
                var bounds = col.bounds;

                GetRangesFromBounds(bounds, out int startX, out int endX, out int startY, out int endY);

                _field.SetBlocks(startX, endX, startY, endY, true);
            }
        }

        private void GetRangesFromBounds(Bounds bounds, out int startX, out int endX, out int startY, out int endY)
        {
            int sx = (int)bounds.min.x;
            int ex = (int)bounds.max.x;
            int sy = (int)bounds.min.z;
            int ey = (int)bounds.max.z;

            CoordinateHelper.PositionToWorldCoords(sx, sy, _field.TileSize, out startX, out startY);
            CoordinateHelper.PositionToWorldCoords(ex, ey, _field.TileSize, out endX, out endY);
        }


        private void AddChunksInArea(int sx, int ex, int sy, int ey)
        {
            CoordinateHelper.PositionToChunkCoords(sx, sy, _field.TileSize, _field.ChunkSize, out int startCx,
                out int startCy);
            CoordinateHelper.PositionToChunkCoords(ex, ey, _field.TileSize, _field.ChunkSize, out int endCx,
                out int endCy);

            endCy++;
            endCx++;

            for(int cy = startCy; cy < endCy; cy++)
            for(int cx = startCx; cx < endCx; cx++)
            {
                var ch = _field.CreateChunk();
                ch.IndexOffset = new Vector2Int(cx, cy);

                _field.AddChunk(ch);
            }
        }

        private void Update()
        {
            UpdateField();
        }

        public Vector2 GetDirection(Vector3 position)
        {
            return _field.GetFieldDirection(position);
        }

        private void UpdateField()
        {
            if(_unit == null)
                return;
            
            var pos = _unit.transform.position;

            CoordinateHelper.PositionToWorldCoords(pos.x, pos.z, _field.TileSize, out int startX, out int startY);

            var newPos = new Vector2Int(startX, startY);

            if(newPos != prevPos)
            {
                prevPos = newPos;
                _field.UpdateField(new Vector2(pos.x, pos.z));
            }
        }

        public void SetUnit(Transform transform)
        {
            _unit = transform;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if(_field == null || _field.GetChunks().Count == 0)
            {
                GenerateFlowField();
                return;
            }

            if(_prevReload != _reload)
            {
                _prevReload = _reload;
                GenerateFlowField();
            }

            DrawRect(_field.Bounds.position, _field.Bounds.size);

            UpdateField();

            var chunks = _field.GetChunks();

            for(int i = 0; i < chunks.Count; i++)
            {
                var ch = chunks[i];

                var pos = new Vector2(ch.IndexOffset.x * _field.ChunkSize, ch.IndexOffset.y * _field.ChunkSize);

                if(_drawChunks)
                {
                    DrawChunk(pos);
                }

                for(int j = 0; j < ch.Field.Length; j++)
                {
                    int tileX = j % _field.ChunkSize;
                    int tileY = j / _field.ChunkSize;

                    var tilePos = new Vector3(
                        pos.x + (tileX * _field.TileSize),
                        0,
                        pos.y + (tileY * _field.TileSize));

                    var direction = ch.Field[j];
                    DrawTile(tilePos, direction,
                        tileX + ch.IndexOffset.x * _field.ChunkSize,
                        tileY + ch.IndexOffset.y * _field.ChunkSize,
                        ch.Blocks[j]);
                }
            }
        }

        private void DrawTile(Vector3 pos, Vector2 dir, int x, int y, bool block)
        {
            var finalDir = dir.ToVector3XZ();
            var start = pos + new Vector3(_field.TileSize * .5f, _height, _field.TileSize * .5f);
            var end = start + finalDir * 0.35f;

            if(_drawTiles)
            {
                var color = block ? Color.red : Color.green;
                Handles.color = color;
                Handles.DrawWireDisc(start, Vector3.up, 0.075f);
                if(!block && finalDir != Vector3.zero)
                {
                    Gizmos.color = color;
                    Gizmos.DrawLine(start, end);
                }
            }

            if(_drawIndexes)
            {
                start.y += 0.4f;
                Handles.Label(start, $"{x},{y}");
            }
        }

        private void DrawChunk(Vector2 pos)
        {
            Gizmos.color = Color.blue;
            DrawRect(pos, new Vector2(_field.ChunkSize, _field.ChunkSize));
        }

        private void DrawRect(Vector2 pos, Vector2 size)
        {
            var topLeft = new Vector3(pos.x, _height, pos.y);
            var topRight = new Vector3(pos.x + size.x, _height, pos.y);
            var botRight = new Vector3(pos.x + size.x, _height, pos.y + size.y);
            var botLeft = new Vector3(pos.x, _height, pos.y + size.y);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, botRight);
            Gizmos.DrawLine(botRight, botLeft);
            Gizmos.DrawLine(botLeft, topLeft);
            Gizmos.color = Color.white;
        }

#endif
    }
}