using System.Collections.Generic;
using FlowFieldSystem;
using Personal.Andreas.Scripts;
using UnityEngine;
using Util;

namespace Andreas.Scripts
{
    public class EdgeViewEnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyManager _enemyManager;
        [SerializeField] private bool _spawningEnabled = false;
        [SerializeField] private Timer _spawnRate = 5f;

        private List<FlowFieldManager> _fields = new();

        public void AssignFlowField(FlowFieldManager ff)
        {
            _fields.Add(ff);
        }

        public void EnableSpawning(bool enableSpawn)
        {
            _spawningEnabled = enableSpawn;
        }

        private void Update()
        {
            if(_spawningEnabled && _spawnRate.UpdateTick())
            {
                SpawnEnemy();
            }
        }

        private bool FindSpawnPosition(out Vector3 retPos)
        {
            retPos = Vector3.zero;

            if(_fields.Count <= 0)
            {
                Debug.Log("no fields assigned to enemy spawner");
                return false;
            }

            var rndField = _fields.RandomItem();

            var chunks = rndField.GetFlowChunks();

            if(chunks.Count <= 0)
                return false;

            var field = rndField.GetField();

            while(retPos == Vector3.zero)
            {
                var rndChunk = chunks.RandomItem();

                int rndTileIndex = Rng.Next(field.ChunkLength - 1);

                if(rndTileIndex >= rndChunk.Blocks.Length)
                {
                    Debug.Log("FindSpawnPosition - rndTileIndex out of bounds");
                    continue;
                }

                var rndTileBlock = rndChunk.Blocks[rndTileIndex];
                if(rndTileBlock)
                    continue;

                retPos = CoordinateHelper.TileIndexToPosition(rndTileIndex, field.ChunkSize, rndChunk.IndexOffset);
            }

            return true;
        }

        private void SpawnEnemy()
        {
            if(FindSpawnPosition(out Vector3 position))
            {
                var prefab = PrefabManager.Get.Glob;
                _enemyManager.SpawnEnemy(position, prefab);
            }
        }
    }
}