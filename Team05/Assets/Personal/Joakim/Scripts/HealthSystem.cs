using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Health{

    public class HealthSystem : MonoBehaviour {
        public interface IDamagable {
            public float Health { get; set; }
            public float Energy { get; set; }
        }
    }

    public class Damage {
        public void InstantDamage(HealthSystem.IDamagable player, float value) {
            
        }        
        
        public void InstantHealing(HealthSystem.IDamagable player, float value) {
            
        }
        
        public void Dot(HealthSystem.IDamagable player, float value, float tickRate, float duration) {
            
            
        }
        
        public void Hot(HealthSystem.IDamagable player, float value, float tickRate, float duration) {
            
        }
    }
}
