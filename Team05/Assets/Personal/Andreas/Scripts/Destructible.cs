using AttackNamespace;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace Andreas.Scripts
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class Destructible : MonoBehaviour
    {
        public int Life = 1;

        [Space(10)] [SerializeField] private int _debrisCount = 3;
        [SerializeField] private GameObject[] _debrisPrefabs;

        [Space(20)] public UnityEvent OnDestroyed;

        private void OnTriggerEnter(Collider other)
        {
            var attack = other.GetComponent<Attack.IAttack>();

            if(attack == null)
                return;

            if(--Life <= 0)
            {
                Destruct();
            }
        }

        public void Destruct()
        {
            SpawnPlanks();
            OnDestroyed?.Invoke();
            Delete();
        }

        public void Delete()
        {
            Destroy(gameObject);
        }

        public void SpawnPlanks()
        {
            if(_debrisPrefabs is not { Length: > 0 })
                return;

            for(int i = 0; i < _debrisCount; i++)
            {
                var randomRot = Quaternion.Euler(new Vector3(Rng.NextF(360f), Rng.NextF(360f), Rng.NextF(360f)));
                Instantiate(_debrisPrefabs.RandomItem(), transform.position, randomRot);
            }
        }

        private Vector3 GetRandomPosition()
        {
            return Vector3.zero;
        }
        
    }
}