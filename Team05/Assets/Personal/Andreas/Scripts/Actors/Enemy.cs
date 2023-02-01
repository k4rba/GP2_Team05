using System;
using Andreas.Scripts.EnemyData;
using AudioSystem;
using FlowFieldSystem;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Personal.Andreas.Scripts.Actors
{
    [Serializable]
    public class PlaySoundEvent : UnityEvent<string>
    {
    }
    
    public class Enemy : Actor, IFlowAgent
    {
        public Vector3 FlowDirection { get; set; }
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        [SerializeField] private EnemyData _data;

        private Rigidbody _body;

        private void Awake()
        {
            _body = gameObject.GetComponent<Rigidbody>();
        }

        private void OnDeath()
        {
        }

        private void Start()
        {
            AudioManager.PlaySfx(_data.OnDeath.name);
        }

        private void Confused()
        {
            var rotationDir = new Vector3(0, Random.Range(200, 1000), 0);
            var rotationEuler = Quaternion.Euler(rotationDir * Time.fixedDeltaTime);
            _body.MoveRotation(_body.rotation * rotationEuler);
        }

        public void Move(Vector2 direction)
        {
            var tf = _body.transform;

            if(direction == Vector2.zero)
            {
                Confused();
                return;
            }

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