using System;
using UnityEngine;

namespace Andreas.Scripts
{
    public class ProjectileReceiver : MonoBehaviour
    {
        public Action<Projectile> OnHit;

        private void OnTriggerEnter(Collider other)
        {
            var proj = other.gameObject.GetComponent<Projectile>();

            if(proj == null)
            {
                // Debug.Log("Non-Projectile hit!");
                return;
            }

            Debug.Log("Projectile hit!");

            OnHit?.Invoke(proj);

            proj.OnHitReceiver();
        }
    }
}