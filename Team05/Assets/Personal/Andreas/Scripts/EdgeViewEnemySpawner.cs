using System;
using System.Collections.Generic;
using FlowFieldSystem;
using Personal.Andreas.Scripts;
using UnityEngine;
using Util;

namespace Andreas.Scripts {
    public class EdgeViewEnemySpawner : MonoBehaviour {
        public static EdgeViewEnemySpawner Get;

        // [SerializeField] private FlowFieldManager _ffManager;
        [SerializeField] private EnemyManager _enemyManager;

        [SerializeField] private bool _spawningEnabled = false;
        [SerializeField] private Timer _spawnRate = 5f;

        private List<FlowFieldManager> _fields = new();


        public List<FlowFieldManager> GetFields => _fields;

        public void AssignFlowField(FlowFieldManager ff) {
            _fields.Add(ff);
        }


        private void Awake() {
            Get = this;
        }

        public void EnableSpawning(bool enableSpawn) {
            _spawningEnabled = enableSpawn;
        }

        private void Update() {
            if (_spawningEnabled && _spawnRate.UpdateTick()) {
                SpawnEnemy();
            }
        }

        private bool FindSpawnPosition(out Vector3 retPos) {
            var rndField = _fields.RandomItem();

            var chunks = rndField.GetFlowChunks();
            retPos = Vector3.zero;

            if (chunks.Count <= 0)
                return false;

            var field = rndField.GetField();

            while (retPos == Vector3.zero) {
                var rndChunk = chunks.RandomItem();

                int rndTileIndex = Rng.Next(field.ChunkLength);

                var rndTileBlock = rndChunk.Blocks[rndTileIndex];
                if (rndTileBlock)
                    continue;

                retPos = CoordinateHelper.TileIndexToPosition(rndTileIndex, field.ChunkSize, rndChunk.IndexOffset);
            }

            return true;
        }

        private void SpawnEnemy() {
            if (FindSpawnPosition(out Vector3 position)) {
                var prefab = PrefabManager.Get.Glob;
                _enemyManager.SpawnEnemy(position, prefab);
            }
        }
    }
}