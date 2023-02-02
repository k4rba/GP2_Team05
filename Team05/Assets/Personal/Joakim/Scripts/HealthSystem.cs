using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Health {
    public class HealthSystem {
        public int _health;

        public HealthSystem() {
            _health = 100;
            _health = Mathf.Clamp(_health, 0, 100);
        }
        
        //Health material max health = -0.6, min health = 0.6
        public void TransferHealth(IDamagable self, IDamagable other, Healing heal, Damage dmg) {
            if (self.Health._health > 10 && other.Health._health <= 100) {
                heal.InstantHealing(other, 10);
                other.HealthMaterial.SetFloat("HP", other.HealthMaterial.GetFloat("HP") + 0.06f);
                dmg.InstantDamage(self, 10);
                self.HealthMaterial.SetFloat("HP", self.HealthMaterial.GetFloat("HP") - 0.06f);
            }
        }

        public interface IDamagable {
            public Material HealthMaterial { get; set; }
            public HealthSystem Health { get; set; }
            public float Energy { get; set; }
        }
    }

    public class Healing {
        public void InstantHealing(HealthSystem.IDamagable player, float value) {
        }

        public void Hot(HealthSystem.IDamagable player, float value, float tickRate, float duration) {
        }
    }

    public class Damage {
        public void InstantDamage(HealthSystem.IDamagable player, float value) {
        }

        public void Dot(HealthSystem.IDamagable player, float value, float tickRate, float duration) {
        }
    }
}