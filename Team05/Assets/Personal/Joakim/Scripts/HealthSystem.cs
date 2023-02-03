using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

                InstantDamage(self, 0.05f);

                Debug.Log(self + " Transferred health to " + other);
                
                var otherHealthMatFloat = other.HealthMaterial.GetFloat("_HP");
                Debug.Log($"TRANSFER: hp: {other.Health._health}   shader: {otherHealthMatFloat}");
            }
        }

        public void InstantHealing(HealthSystem.IDamagable player, float value) {
            player.Health._health = ClampHP(player.Health._health + value);
            var playerHealthMat = player.HealthMaterial.GetFloat("_HP");
            player.HealthMaterial.SetFloat("_HP", ClampHP(playerHealthMat + value));
        }

        public void InstantDamage(HealthSystem.IDamagable player, float value) {
            if (player.Health._health >= -0.45f) {
                player.Health._health = ClampHP(player.Health._health - value);
                var playerHealthMat = player.HealthMaterial.GetFloat("_HP");
                player.HealthMaterial.SetFloat("_HP",ClampHP(playerHealthMat - value));
                
                playerHealthMat = player.HealthMaterial.GetFloat("_HP");
                Debug.Log($"DAMAGE: hp: {player.Health._health}   shader: {playerHealthMat}");
            }
            else {
                Die(player);
            }
        }

        public void ResetHealth(IDamagable player) {
            player.Health._health = 0.25f;
            player.HealthMaterial.SetFloat("_HP", 0.25f);
        }

        public void Die(IDamagable damagable) {
            Debug.Log(damagable + "Died :C");
            SceneManager.LoadScene("MainScene");
        }

        private float ClampHP(float health) => Mathf.Clamp(health, -0.5f, 0.5f);


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