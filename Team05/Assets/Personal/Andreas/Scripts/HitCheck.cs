using AudioSystem;
using UnityEngine;

namespace Andreas.Scripts {
    public class HitCheck : MonoBehaviour {

        private GameObject _target;
        private bool _didHit = false;

        private float _lifeTimer = 0.2f;
        
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
                AudioManager.PlaySfx("MeleeHit2_mixdown", transform.position);
            }
            else {
                Debug.Log("Did no damage");
            }
        }

        private void Update()
        {
            _lifeTimer -= Time.deltaTime;
            
            if(_lifeTimer <= 0)
                Destroy(gameObject);
            
        }
        
    }
}