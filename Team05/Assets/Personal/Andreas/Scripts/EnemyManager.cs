using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Personal.Andreas.Scripts
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyContainer;
        private List<GameObject> _enemies;

        public event Action<GameObject> OnEnemyAdded;

        private void Start()
        {
            _enemies = new();
        }

        public void SpawnEnemy(Vector3 position, GameObject prefab)
        {
            var enemy = Instantiate(prefab, position, Quaternion.identity, _enemyContainer.transform);
            AddEnemy(enemy);
        }

        public void AddEnemy(GameObject enemy)
        {
            // _enemies.Add(enemy);
            OnEnemyAdded?.Invoke(enemy);
        }

        public void RemoveEnemy(GameObject enemy)
        {
            // _enemies.Remove(enemy);
        }
        
    }
}
