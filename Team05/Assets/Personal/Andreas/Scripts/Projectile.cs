using System;
using UnityEngine;

namespace Andreas.Scripts
{
    public enum Alliances
    {
        None,
        Player,
        Evil,
    }

    public class Projectile : MonoBehaviour
    {
        public Alliances Alliance;
        public int Damage;
        public float Speed;

        private float _lifeTime = 6f;

        private Vector3 _direction;

        public void Init(Vector3 targetPosition)
        {
            _direction = (targetPosition - transform.position).normalized;
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            var velocity = _direction * (Speed * dt);
            transform.position += velocity;

            _lifeTime -= dt;
            if(_lifeTime <= 0)
                Destroy(gameObject);
        }

        public void OnHitReceiver()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
        }
    }
}