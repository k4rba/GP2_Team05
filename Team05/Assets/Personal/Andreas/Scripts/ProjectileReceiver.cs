using UnityEngine;
using UnityEngine.Events;

namespace Andreas.Scripts
{
    public class ProjectileReceiver : MonoBehaviour
    {
        // public Action<Projectile> OnHit;
        [SerializeField] public UnityEvent<Projectile> OnHit;

        private void OnTriggerEnter(Collider other)
        {
            var proj = other.gameObject.GetComponent<Projectile>();

            if(proj == null)
            {
                // Debug.Log("Non-Projectile hit!");
                return;
            }

            Debug.Log($"Projectile hit '{gameObject.name}'!");

            OnHit?.Invoke(proj);

            proj.OnHitReceiver();
        }
    }
}