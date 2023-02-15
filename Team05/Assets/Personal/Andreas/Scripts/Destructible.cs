using AttackNamespace;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace Andreas.Scripts {
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class Destructible : MonoBehaviour {
        public int Life = 1;

        public UnityEvent OnDestroyed;

        private void OnTriggerEnter(Collider other) {
            var attack = other.GetComponent<Attack.IAttack>();

            if (attack == null)
                return;

            if (--Life <= 0) {
                Destruct();
            }
        }

        public void Destruct() {
            SpawnPlanks();
            OnDestroyed?.Invoke();
            Delete();
        }

        public void Delete() {
            Destroy(gameObject);
        }

        public void SpawnPlanks() {
            var plank = FastResources.Load<GameObject>("Plank");
            Instantiate(plank, transform.position, Quaternion.identity);
            Instantiate(plank, transform.position, Quaternion.identity);
            Instantiate(plank, transform.position, Quaternion.identity);
        }
    }
}