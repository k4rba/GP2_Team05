using System;
using System.Collections.Generic;
using UnityEngine;

namespace Andreas.Scripts
{
    public class EnemyTriggerSpawner : EnemySpawnerBase
    {
        public EnemySpawnManager Manager { get; set; }

        public List<Collider> Triggers;
        
        public List<EnemySpawner> Spawners;

        private void Awake()
        {
        }
    }
}