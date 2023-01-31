using Personal.Andreas.Scripts.Flowfield;
using Personal.Andreas.Scripts.Util;
using UnityEditor;
using UnityEngine;

namespace Personal.Andreas.Scripts
{
    public class FlowFieldManager : MonoBehaviour
    {
        [SerializeField] private VectorFlowField2D _field;

        private const int tempWorldSize = 5;
        private const int tempWorldLength = tempWorldSize * tempWorldSize;

        private bool[] CreateRandomBlocks()
        {
            var blocks = new bool[_field.ChunkLength];
            for(int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = Rng.Roll(5);
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
                    (i % tempWorldSize) * _field.ChunkSize,
                    (i / tempWorldSize) * _field.ChunkSize);
                ch.Blocks = CreateRandomBlocks();

                //  random fields
                for(int j = 0; j < ch.Field.Length; j++)
                {
                    ch.Field[j] = new Vector2(Rng.NextF(-1f, 1f), Rng.NextF(-1f, 1f));
                }
                
                chunks[i] = ch;
            }

            _field.Setup(chunks);
        }

        private void OnDrawGizmos()
        {
            if(_field == null || _field.GetChunks().Count == 0)
            {
                SetupTempFlowField();
                return;
            }

            var chunks = _field.GetChunks();

            for(int i = 0; i < chunks.Count; i++)
            {
                var ch = chunks[i];

                var pos = ch.IndexOffset;
                DrawChunk(pos);

                for(int j = 0; j < ch.Field.Length; j++)
                {
                    int tileX = j % tempWorldSize;
                    int tileY = j / tempWorldSize;

                    var tilePos = new Vector3(
                        pos.x + (tileX * _field.TileSize),
                        0,
                        pos.y + (tileY * _field.TileSize));

                    var direction = ch.Field[j];
                    DrawTile(tilePos, direction);
                }
            }
            
        }

        private void DrawTile(Vector3 pos, Vector2 dir)
        {
            var finalDir = dir.ToVector3XZ();
            var start = pos + new Vector3(_field.TileSize * .5f, 0, _field.TileSize * .5f);
            var end = start + finalDir * 0.35f;

            Handles.color = Color.red;
            Handles.DrawWireDisc(start, Vector3.up, 0.1f);
            if(finalDir != Vector3.zero)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(start, end);
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
    }
}