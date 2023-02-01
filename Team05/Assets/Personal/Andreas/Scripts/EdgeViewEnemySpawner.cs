using FlowFieldSystem;
using Personal.Andreas.Scripts;
using UnityEngine;
using Util;

namespace Andreas.Scripts
{
    public class EdgeViewEnemySpawner : MonoBehaviour
    {
        [SerializeField] private FlowFieldManager _ffManager;
        [SerializeField] private EnemyManager _enemyManager;
        
        [SerializeField] private float _radius = 2f;
        [SerializeField] private Timer _spawnRate = 0.5f;

        private bool _spawningEnabled;

        private void Update()
        {
            if(_spawnRate.UpdateTick())
            {
                SpawnEnemy();
            }
        }

        private Vector3 FindSpawnPosition()
        {
            var chunks = _ffManager.GetFlowChunks();

            var field = _ffManager.GetField();

            Vector3 retPos = Vector3.zero;

            while(retPos == Vector3.zero)
            {
                var rndChunk = chunks.RandomItem();

                int rndTileIndex = Rng.Next(field.ChunkLength);

                var rndTileBlock = rndChunk.Blocks[rndTileIndex];
                if(rndTileBlock)
                    continue;

                retPos = CoordinateHelper.TileIndexToPosition(rndTileIndex, field.ChunkSize, rndChunk.IndexOffset);
            }

            return retPos;
        }

        private void SpawnEnemy()
        {
            var position = FindSpawnPosition();
            var prefab = PrefabManager.Get.Glob;
            _enemyManager.SpawnEnemy(position, prefab);
        }
    }
}