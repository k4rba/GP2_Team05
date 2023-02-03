using System;
using UnityEngine;

namespace Andreas.Scripts {
    public class HitCheck : MonoBehaviour {

        private GameObject _target;
        private bool _didHit = false;
        
        public void Set(GameObject target) {
            _target = target;
        }
        
        private void OnTriggerEnter(Collider other) {

            if (!_didHit && other.gameObject == _target) {
                //  do damage .. ONCE
                _didHit = true;
                Debug.Log("Did damage");
                other.GetComponent<Player>().Health.InstantDamage(other.GetComponent<Player>(), 0.05f);
                Destroy(gameObject);
            }
            else {
                Debug.Log("Did no damage");
            }
        }

        private void Update() {
            
        }
        
    }
}