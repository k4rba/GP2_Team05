using Health;
using UnityEngine;

namespace Personal.Andreas.Scripts.Actors
{
    public class Actor : MonoBehaviour, HealthSystem.IDamagable
    {
        public float Health { get; set; }
        public float Energy { get; set; }
    }
}