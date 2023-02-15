using AttackNamespace;
using UnityEngine;
using UnityEngine.Events;

namespace Andreas.Scripts
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class Destructible : MonoBehaviour
    {
        public int Life = 1;

        public UnityEvent OnDestroyed;
        
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
            OnDestroyed?.Invoke();
            Delete();
        }

        public void Delete()
        {
            Destroy(gameObject);
        }
    }
}