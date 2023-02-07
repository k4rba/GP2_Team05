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
            var dir = new Vector3(0, 0, vert);
            float speed = 4f;
            var pos = _body.position;
            var newPos = pos + dir * (speed * Time.fixedDeltaTime);
            _body.MovePosition(newPos);
        }
    }
}