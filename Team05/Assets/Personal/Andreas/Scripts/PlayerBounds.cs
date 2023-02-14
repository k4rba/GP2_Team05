using System;
using UnityEngine;

namespace Andreas.Scripts
{
    public class PlayerBounds : MonoBehaviour
    {
        private Rigidbody _self;
        private Rigidbody _other;

        [SerializeField] private float _maxDistance = 2f;

        private bool _ready;

        private void Awake()
        {
            _self = GetComponent<Rigidbody>();
        }

        public void LinkWithOther(Rigidbody tf)
        {
            _other = tf;
            _ready = true;
        }

        private void FixedUpdate()
        {
            if(!_ready)
                return;
            
            var self = transform.position;
            var other = _other.position;
            var distance = Vector3.Distance(self, other);

            if(distance > _maxDistance)
            {
                var diff = distance - _maxDistance;
                var dir = (other - self).normalized;
                Debug.Log(diff);
                _self.MovePosition(self + new Vector3(dir.x, 0, dir.z) * diff);
            }
        }
    }
}