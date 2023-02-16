using UnityEngine;
using Util;

namespace Andreas.Scripts
{
    public class EnemySpawner : EnemySpawnerBase
    {
        public float SpawnRate;
        public int SpawnCount;
        
        [Space(10)]
        
        [Header("Randomly selects one of the prefabs every time it spawns")]
        public GameObject[] _enemyPrefabs;
        
        public void ActivateSpawner()
        {
            if(_enemyPrefabs == null || _enemyPrefabs.Length <= 0)
                return;
            
            InvokeRepeating(nameof(Spawn), 0f, SpawnRate);
        }

        private void Spawn()
        {
            var rndPrefab = _enemyPrefabs.RandomItem();
            var enManager = GameManager.Instance.EnemyManager;
            enManager.SpawnEnemy(transform.position, rndPrefab);
            
            if(--SpawnCount <= 0)
            {
                CancelInvoke(nameof(Spawn));
                Destroy(gameObject);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            Gizmos.color = Color.white;
        }
    }
}