using System;
using System.Collections.Generic;
using Andreas.Scripts;
using FlowFieldSystem;
using Personal.Andreas.Scripts.Actors;
using UnityEngine;
using Util;

namespace Personal.Andreas.Scripts
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Get { get; private set; }
        
        public EdgeViewEnemySpawner Spawner;

        public event Action<GameObject> OnEnemyAdded;
        public event Action<GameObject> OnEnemyAddedTwo;

        [SerializeField] private GameObject _enemyContainer;
        private List<GameObject> _enemies;

        private void Awake()
        {
            if(Get == null)
            {
                Get = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
                return;
            }
        }

        private void Start()
        {
            _enemies = new();

            //  add temp flowfield
            var fieldObj = GameObject.Find("FlowFieldMap");
            var field = fieldObj.GetComponent<FlowFieldManager>();
            Spawner.AssignFlowField(field);
        }

        public void SpawnEnemy(Vector3 position, GameObject prefab)
        {
            var enemy = Instantiate(prefab, position, Quaternion.identity, _enemyContainer.transform);
            AddEnemy(enemy);
        }

        public void AddEnemy(GameObject enemy)
        {
            // _enemies.Add(enemy);

            if(Rng.Bool)
            {
                OnEnemyAdded?.Invoke(enemy);
            }
            else
            {
                OnEnemyAddedTwo?.Invoke(enemy);
            }
        }

        public void RemoveEnemy(GameObject enemy)
        {
            // _enemies.Remove(enemy);
        }
    }
}