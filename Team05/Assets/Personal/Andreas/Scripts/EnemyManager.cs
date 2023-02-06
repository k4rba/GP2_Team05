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
        public event Action<GameObject> OnEnemyAdded;
        public event Action<GameObject> OnEnemyAddedTwo;
        
        public EdgeViewEnemySpawner Spawner;

        [SerializeField] private GameObject _enemyContainer;
        private List<Enemy> _enemies;

        private void Start()
        {
            _enemies = new();

            //  add temp flowfield
            // var fieldObj = GameObject.Find("FlowFieldMap");
            // var field = fieldObj.GetComponent<FlowFieldManager>();
            // Spawner.AssignFlowField(field);
        }

        public void SpawnEnemy(Vector3 position, GameObject prefab)
        {
            var enemy = Instantiate(prefab, position, Quaternion.identity, _enemyContainer.transform);
            AddEnemy(enemy.GetComponent<Enemy>());
        }

        public void AddEnemy(Enemy enemy)
        {
            // _enemies.Add(enemy);

            if(Rng.Bool)
            {
                OnEnemyAdded?.Invoke(enemy.gameObject);
            }
            else
            {
                OnEnemyAddedTwo?.Invoke(enemy.gameObject);
            }
        }

        public void RemoveEnemy(GameObject enemy)
        {
            // _enemies.Remove(enemy);
        }
    }
}