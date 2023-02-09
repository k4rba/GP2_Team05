using System;
using System.Collections.Generic;
using Personal.Andreas.Scripts;
using UnityEngine;

namespace Andreas.Scripts
{
    public class EnemySpawnManager : MonoBehaviour
    {
        private EnemyManager _manager;

        private List<EnemySpawnerBase> _spawners;

        private void Awake()
        {
            _spawners = new();
        }

        public void Spawn()
        {
            // _manager.SpawnEnemy();
        }
    }
}