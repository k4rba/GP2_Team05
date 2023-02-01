using Personal.Andreas.Scripts.Flowfield;
using Personal.Andreas.Scripts.Util;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Personal.Andreas.Scripts
{
    public class FlowFieldManager : MonoBehaviour
    {
        [SerializeField] private VectorFlowField2D _field;
        [SerializeField] private Transform _unit;

        [SerializeField] private bool _drawIndexes = false;
        [SerializeField] private bool _drawTiles = false;
        [SerializeField] private bool _drawChunks = true;

        private const int tempWorldSize = 5;
        private const int tempWorldLength = tempWorldSize * tempWorldSize;

        private Vector2Int prevPos;

        private void Start()
        {
            SetupTempFlowField();
        }

        //  temporary testing
        private bool[] CreateRandomBlocks()
        {
            bool fullyBlock = Rng.Roll(20);

            var blocks = new bool[_field.ChunkLength];
            for(int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = fullyBlock ? true : Rng.Roll(5);
            }

            return blocks;
        }

        private void SetupTempFlowField()
        {
            _field ??= new VectorFlowField2D();

            //  temporary
            var chunks = new FlowChunk[tempWorldLength];

            for(int i = 0; i < chunks.Length; i++)
            {
                var ch = new FlowChunk(_field.ChunkSize);
                ch.IndexOffset = new Vector2Int(
                    (i % tempWorldSize),
                    (i / tempWorldSize));
                ch.Blocks = CreateRandomBlocks();

                //  random fields
                for(int j = 0; j < ch.Field.Length; j++)
                {
                    ch.Field[j] = new Vector2(Rng.NextF(-1f, 1f), Rng.NextF(-1f, 1f)).normalized;
                }

                chunks[i] = ch;
            }

            _field.Setup(chunks);
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
            var pos = _unit.transform.position;

            CoordinateHelper.PositionToWorldCoords(pos.x, pos.z, _field.TileSize, out int startX, out int startY);

            var newPos = new Vector2Int(startX, startY);

            if(newPos != prevPos)
            {
                prevPos = newPos;
                _field.UpdateField(new Vector2(pos.x, pos.z));
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if(_field == null || _field.GetChunks().Count == 0)
            {
                SetupTempFlowField();
                return;
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
                    int tileX = j % tempWorldSize;
                    int tileY = j / tempWorldSize;

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
            var start = pos + new Vector3(_field.TileSize * .5f, 0, _field.TileSize * .5f);
            var end = start + finalDir * 0.35f;

            if(_drawTiles)
            {
                var color = block ? Color.red : Color.green;
                Handles.color = color;
                Handles.DrawWireDisc(start, Vector3.up, 0.1f);
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
            var topLeft = new Vector3(pos.x, 0, pos.y);
            var topRight = new Vector3(pos.x + size.x, 0, pos.y);
            var botRight = new Vector3(pos.x + size.x, 0, pos.y + size.y);
            var botLeft = new Vector3(pos.x, 0, pos.y + size.y);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, botRight);
            Gizmos.DrawLine(botRight, botLeft);
            Gizmos.DrawLine(botLeft, topLeft);
            Gizmos.color = Color.white;
        }

#endif
    }
}