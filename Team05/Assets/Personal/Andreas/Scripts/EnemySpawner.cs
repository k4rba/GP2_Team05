using UnityEngine;
using Util;

namespace Andreas.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
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
            return Vector3.zero;
        }
        
        private void SpawnEnemy()
        {
            var position = FindSpawnPosition();
            
        }
        
    }
}