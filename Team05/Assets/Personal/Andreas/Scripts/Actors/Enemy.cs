using System;
using Andreas.Scripts;
using Andreas.Scripts.EnemyData;
using Andreas.Scripts.EnemyStates;
using AudioSystem;
using FlowFieldSystem;
using UnityEngine;

namespace Personal.Andreas.Scripts.Actors
{
    public class Enemy : Actor, IFlowAgent
    {
        public FlowFieldManager FlowManager { get; set; }
        
        [NonSerialized] public Vector2 FlowDirection;
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        [NonSerialized] public Rigidbody Body;
        public EnemyData Data;
        public EnemyStateManager StateManager;
        
        private void Awake()
        {
            StateManager = new(this);
            Body = gameObject.GetComponent<Rigidbody>();
        }

        private void Start()
        {
            AudioManager.PlaySfx(Data.OnDeath.name);
            StateManager.SetState(new EnemyStateHunt(this));
        }

        public void Die() {
            FlowManager.AgentManager.RemoveAgent(this);
        }
        
        private void Update()
        {
            StateManager.Update(Time.deltaTime);
        }

        private void FixedUpdate() {
            
            StateManager.FixedUpdate(Time.fixedDeltaTime);
        }

        public void FlowDirectionUpdated(Vector2 direction)
        {
            FlowDirection = direction;
        }

        private GameObject _hitBox;
        
        public void DoAttack(GameObject hitBox) {
            _hitBox = hitBox;
            float attackSpeed = 2f;
            InvokeRepeating(nameof(AttackInvoke), 0, attackSpeed);
        }

        private void AttackInvoke() {
                        
            var enPos = transform.position;
            var unit = FlowManager.GetUnit();
            var dir = (unit.transform.position - enPos).normalized;

            var boxOffsetRange = 1.5f;

            var boxPosition = enPos + dir * boxOffsetRange;

            var box = Instantiate(_hitBox, boxPosition, Quaternion.identity);
            var hitCheck = box.GetComponent<HitCheck>();
            hitCheck.Set(unit.gameObject);
            
        }

        public void CancelAttack() {
            CancelInvoke(nameof(AttackInvoke));
        }
    }
}