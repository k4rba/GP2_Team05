using System;
using Andreas.Scripts;
using Andreas.Scripts.EnemyData;
using Andreas.Scripts.EnemyStates;
using Andreas.Scripts.StateMachine;
using AudioSystem;
using FlowFieldSystem;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Personal.Andreas.Scripts.Actors
{
    public class Enemy : Actor, IFlowAgent
    {
        public FlowFieldManager Manager { get; set; }
        
        public Vector2 FlowDirection;
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public void Die() {
            Manager.AgentManager.RemoveAgent(this);
        }

        [SerializeField] private EnemyData _data;

        public Rigidbody Body;
        public StateManager StateManager;
        
        private void Awake()
        {
            StateManager = new();
            Body = gameObject.GetComponent<Rigidbody>();
        }

        private void OnDeath()
        {
        }

        private void Start()
        {
            AudioManager.PlaySfx(_data.OnDeath.name);
            StateManager.SetState(new EnemyStateHunt(this));
        }

        private void Update()
        {
            StateManager.Update(Time.deltaTime);
        }

        private void FixedUpdate() {
            
            StateManager.FixedUpdate(Time.fixedDeltaTime);
        }

        private void Confused()
        {
            var rotationDir = new Vector3(0, Random.Range(200, 1000), 0);
            var rotationEuler = Quaternion.Euler(rotationDir * Time.fixedDeltaTime);
            Body.MoveRotation(Body.rotation * rotationEuler);
        }

        public void Move(Vector2 direction)
        {
            FlowDirection = direction;

            // var tf = _body.transform;

            // var direction = Enemy.FlowDirection;
            
            // if(direction == Vector2.zero)
            // {
            //     //  flow is zero
            //     Confused();
            //     return;
            // }
            //
            // //  todo - take speed from some stat collection
            // float speed = 5f;
            //
            // var pos = tf.position;
            // var dir3 = new Vector3(direction.x, 0, direction.y);
            // var velocity = dir3 * (speed * Time.deltaTime);
            // pos += velocity;
            // _body.MovePosition(pos);
            //
        }

        private GameObject _hitBox;
        
        public void DoAttack(GameObject hitBox) {
            _hitBox = hitBox;
            float attackSpeed = 2f;
            InvokeRepeating(nameof(AttackInvoke), 0, attackSpeed);
        }

        private void AttackInvoke() {
                        
            var enPos = transform.position;
            var unit = Manager.GetUnit();
            var dir = (unit.transform.position - enPos).normalized;

            var boxOffsetRange = 1.5f;

            var boxPosition = enPos + dir * boxOffsetRange;

            var box = Instantiate(_hitBox, boxPosition, Quaternion.identity);
            var hitCheck = box.GetComponent<HitCheck>();
            hitCheck.Set(unit.gameObject);
            
            Debug.Log("ATTACKED!!!!!");
        }

        public void CancelAttack() {
            CancelInvoke(nameof(AttackInvoke));
        }
    }
}