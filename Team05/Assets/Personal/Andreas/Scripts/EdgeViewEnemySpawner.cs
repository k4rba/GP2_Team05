using System.Collections.Generic;
using FlowFieldSystem;
using Personal.Andreas.Scripts;
using UnityEngine;
using UnityEngine.AI;
using Util;

namespace Andreas.Scripts
{
    public class EdgeViewEnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyManager _enemyManager;
        [SerializeField] private bool _spawningEnabled = false;
        [SerializeField] private Timer _spawnRate = 5f;

        private int maxEnemyCount = 999;

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

        private bool FindSpawnPosition(out Vector3 pos)
        {
            var radius = 5f;
            var rndDir = Random.insideUnitSphere * radius;
            NavMeshHit hit;
            NavMesh.SamplePosition(rndDir, out hit, radius, 1);
            pos = hit.position;
            return true;
        }
        
        // private bool FindSpawnPosition(out Vector3 retPos)
        // {
        //     retPos = Vector3.zero;
        //
        //     if(_fields.Count <= 0)
        //     {
        //         Debug.Log("no fields assigned to enemy spawner");
        //         return false;
        //     }
        //
        //     var rndField = _fields.RandomItem();
        //
        //     var chunks = rndField.GetFlowChunks();
        //
        //     if(chunks.Count <= 0)
        //         return false;
        //
        //     var field = rndField.GetField();
        //
        //     while(retPos == Vector3.zero)
        //     {
        //         var rndChunk = chunks.RandomItem();
        //
        //         int rndTileIndex = Rng.Next(field.ChunkLength - 1);
        //
        //         if(rndTileIndex >= rndChunk.Blocks.Length)
        //         {
        //             Debug.Log("FindSpawnPosition - rndTileIndex out of bounds");
        //             continue;
        //         }
        //
        //         var rndTileBlock = rndChunk.Blocks[rndTileIndex];
        //         if(rndTileBlock)
        //             continue;
        //
        //         retPos = CoordinateHelper.TileIndexToPosition(rndTileIndex, field.ChunkSize, rndChunk.IndexOffset);
        //     }
        //
        //     return true;
        // }

        private static string[] enemyPrefabNames =
        {
            "Bosse", "Gunnar"
        };
        
        private void SpawnEnemy()
        {
            if(_enemyManager.Enemies.Count >= maxEnemyCount)
            {
                return;
            }
            
            if(FindSpawnPosition(out Vector3 position))
            {
                var randomEnemyPrefab = enemyPrefabNames.RandomItem();
                var prefab = FastResources.Load<GameObject>($"Prefabs/Enemies/{randomEnemyPrefab}");
                _enemyManager.SpawnEnemy(position, prefab);
            }
        }
    }
}