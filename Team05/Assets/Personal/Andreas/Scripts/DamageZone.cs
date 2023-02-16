using UnityEngine;

namespace Andreas.Scripts
{
    public class DamageZone : MonoBehaviour
    {
        public float Damage = 999f;

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<Player>();
            if(player != null)
            {
                player.Health.InstantDamage(player, Damage);
            }
        }
    }
}