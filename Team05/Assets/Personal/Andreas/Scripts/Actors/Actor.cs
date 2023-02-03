using Health;
using UnityEngine;

namespace Personal.Andreas.Scripts.Actors
{
    public class Actor : MonoBehaviour, HealthSystem.IDamagable
    {
        public Material HealthMaterial { get; set; }
        public HealthSystem Health { get; set; }
        public int CurrentHealth { get; set; }
        public float Energy { get; set; }
    }
}