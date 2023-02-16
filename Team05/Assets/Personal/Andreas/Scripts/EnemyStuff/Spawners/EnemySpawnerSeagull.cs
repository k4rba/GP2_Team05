using Andreas.Scripts.EnemyStates;
using UnityEngine;
using Util;

namespace Andreas.Scripts
{
    public class EnemySpawnerSeagull : MonoBehaviour
    {
        public int Count;

        
        private void Start()
        {
            var prefab = FastResources.Load<GameObject>("Prefabs/Enemies/Seagull");

            float range = 5f;

            for(int i = 0; i < Count; i++)
            {
                var rndDir = Random.insideUnitSphere * Rng.NextF(1f, range);
                var pos = transform.position + rndDir;
                var en = GameManager.Instance.EnemyManager.SpawnEnemy(pos, prefab);
                en.StateManager.SetState(new EnemyStateSeagull(transform.position));
            }
        }
    }
}