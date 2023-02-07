using System;
using UnityEngine;

namespace Andreas.Scripts
{
    public class PlayerConstrainer : MonoBehaviour
    {
        [SerializeField] private GameObject _other;
        [SerializeField] private Rigidbody _body;

        private void FixedUpdate()
        {
            var vert = Input.GetAxisRaw("Horizontal");
            var hori = Input.GetAxisRaw("Vertical");
            
            var dir = new Vector3(vert, 0, hori);
            float speed = 4f;
            var pos = _body.position;
            var vel = dir * (speed * Time.fixedDeltaTime);
            var newPos = pos + vel;
            _body.MovePosition(newPos);
            // _body.AddForce(vel, ForceMode.VelocityChange);
        }
    }
}