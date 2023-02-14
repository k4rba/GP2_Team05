using System;
using System.Collections.Generic;
using Andreas.Scripts;
using Personal.Andreas.Scripts.Actors;
using UnityEngine;
using Util;

namespace Personal.Andreas.Scripts
{
    public class EnemyManager : MonoBehaviour
    {
        public List<Enemy> Enemies => _enemies;
        
        public event Action<GameObject> OnEnemyAdded;
        public event Action<GameObject> OnEnemyAddedTwo;
        
        public EdgeViewEnemySpawner Spawner;

        private List<Enemy> _enemies;
        [SerializeField] private GameObject _enemyContainer;

        private void Start()
        {
            _enemies = new();
        }

        public Enemy SpawnEnemy(Vector3 position, GameObject prefab)
        {
            var enemyObj = Instantiate(prefab, position, Quaternion.identity, _enemyContainer.transform);
            var enemy = enemyObj.GetComponent<Enemy>();
            AddEnemy(enemy);
            return enemy;
        }

        public void AddEnemy(Enemy enemy)
        {
            _enemies.Add(enemy);

            if(Rng.Bool)
            {
                OnEnemyAdded?.Invoke(enemy.gameObject);
            }
            else
            {
                OnEnemyAddedTwo?.Invoke(enemy.gameObject);
            }
        }

        public void RemoveEnemy(Enemy enemy)
        {
            _enemies.Remove(enemy);
        }
    }
}