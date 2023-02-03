using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Health {
    public class HealthSystem {
        public float _health;

        public HealthSystem() {
            _health = 0.25f;
            _health = Mathf.Clamp(_health, -0.5f, 0.5f);
        }
        
        //max health = 0.5f, min = -0.5f
        public void TransferHealth(IDamagable self, IDamagable other) {
            if (self.Health._health >= -0.40f && other.Health._health < 0.5f) {
                InstantHealing(other, 0.05f);
                var otherHealthMatFloat = other.HealthMaterial.GetFloat("_HP");
                var selfHealthMatFloat = self.HealthMaterial.GetFloat("_HP");
                other.HealthMaterial.SetFloat("_HP", otherHealthMatFloat + 0.05f);
                ClampHP(other.Health._health);
                ClampHP(otherHealthMatFloat);
                InstantDamage(self, 0.05f);
                self.HealthMaterial.SetFloat("_HP", selfHealthMatFloat - 0.05f);
                ClampHP(self.Health._health);
                ClampHP(selfHealthMatFloat);
                Debug.Log(self + " Transferred health to " + other);
            }
        }

        public void Die(IDamagable damagable) {
            if (damagable.Health._health <= -0.5f) {
                //todo: dieeeee
                Debug.Log(damagable + "Died :C");
            }
        }

        private float ClampHP(float health) => Mathf.Clamp(health, -0.5f, 0.5f);

        public void InstantHealing(HealthSystem.IDamagable player, float value) {
            player.Health._health += value;
        }
        
        public void InstantDamage(HealthSystem.IDamagable player, float value) {
            player.Health._health -= value;
        }

        public interface IDamagable {
            public Material HealthMaterial { get; set; }
            public HealthSystem Health { get; set; }
            public float Energy { get; set; }
        }
    }

    public class Healing {
        public void Hot(HealthSystem.IDamagable player, float value, float tickRate, float duration) {
            
        }
    }

    public class Damage {
        public void Dot(HealthSystem.IDamagable player, float value, float tickRate, float duration) {
        }
    }
}