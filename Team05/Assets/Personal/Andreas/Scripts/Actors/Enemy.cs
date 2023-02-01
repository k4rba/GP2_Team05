using Personal.Andreas.Scripts.Flowfield;
using UnityEngine;

namespace Personal.Andreas.Scripts.Actors
{
    public class Enemy : Actor, IFlowAgent
    {
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
        
        public Vector3 FlowDirection { get; set; }

        private Rigidbody _body;

        private void Awake()
        {
            _body = gameObject.GetComponent<Rigidbody>();
        }

        public void Move(Vector2 direction)
        {
            var tf = _body.transform;
            
            //  todo - take speed from some stat collection
            float speed = 5f;
            
            var pos = tf.position;
            var dir3 = new Vector3(direction.x, 0, direction.y);
            var velocity = dir3 * (speed * Time.deltaTime);
            pos += velocity;
            _body.MovePosition(pos);
        }
    }
}