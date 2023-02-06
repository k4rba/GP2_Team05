using System;
using UnityEngine;
using Util;

namespace Andreas.Scripts
{
    //  follow state (temp)
    public class PlayerDebugComponent : MonoBehaviour
    {
        private Transform _target;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void SetFollow(Transform target)
        {
            _target = target;
        }

        private void FixedUpdate()
        {
            var pos = transform.position;
            var distance = pos.FastDistance(_target.position);
            
            if(distance < 2f)
                return;
            
            float speed = 3f;
            var direction = (_target.position - pos).normalized;
            pos += direction * (speed * Time.fixedDeltaTime);
            _rigidbody.MovePosition(pos);
        }
    }
}