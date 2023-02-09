using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Andreas.Scripts
{
    public class PlayerTestScript : MonoBehaviour
    {
        [SerializeField] private Rigidbody _body;

        [SerializeField] private bool _enabledInput;

        [SerializeField] private bool _playerOne;

        [SerializeField] private float _speed= 15f;

        private void FixedUpdate()
        {

            // _body.velocity *= 0.1f;
            
            if(_enabledInput)
            {
                if(_playerOne)
                    UpdateInputP1();
                if(!_playerOne)
                    UpdateInputP2();
            }
        }

        void Move(Vector3 dir)
        {
            var pos = _body.position;
            var vel = dir * (_speed * Time.fixedDeltaTime);
            var newPos = pos + vel;

            // _body.MovePosition(newPos);
            _body.AddForce(vel, ForceMode.VelocityChange);
        }

        
        
        
        
        //  DONT LOOK
        
        void UpdateInputP1()
        {
            if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) ||
               Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                Debug.Log("returning");   
                return;
            }
            
            var dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            Move(dir);
        }
        
        void UpdateInputP2()
        {
            float hori = Input.GetKey(KeyCode.LeftArrow) ? -1f : Input.GetKey(KeyCode.RightArrow) ? 1f : 0f;
            float vert = Input.GetKey(KeyCode.UpArrow) ? 1f : Input.GetKey(KeyCode.DownArrow) ? -1f : 0f;
            var dir = new Vector3(hori, 0, vert).normalized;
            Move(dir);
        }
    }
}