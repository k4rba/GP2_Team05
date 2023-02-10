using UnityEngine;

namespace Andreas.Scripts
{
    public class EnemySpawnerBase : MonoBehaviour
    {
        public EnemySpawnManager Manager { get; set; }

        protected virtual void SpawnEnemy()
        {
            
        }
    }
}