using System;
using UnityEngine;

namespace Andreas.Scripts
{
    public class PlayerBounds : MonoBehaviour
    {
        private float MaxDistance => Data.MaxDistance;
        private float Power => Data.Power;
        
        public PlayerBoundsData Data;
        
        private Rigidbody _self;
        private Rigidbody _other;

        private bool _ready;
        private bool _wasOutOfBounds;

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

            if(distance > MaxDistance)
            {
                var diff = distance - MaxDistance;
                var dir = (other - self).normalized;

                var pushPower = diff * 0.1f * Power;
                _self.AddForce(dir * pushPower, ForceMode.VelocityChange);
                _wasOutOfBounds = true;
            }
            else
            {
                if(_wasOutOfBounds)
                {
                    _self.velocity = Vector3.zero;
                    _wasOutOfBounds = false;
                }
            }
        }
    }
}