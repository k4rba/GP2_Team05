using System;
using Andreas.Scripts;
using Andreas.Scripts.EnemyData;
using Andreas.Scripts.EnemyStates;
using Andreas.Scripts.EnemyStuff;
using AudioSystem;
using UnityEngine;
using UnityEngine.AI;

namespace Personal.Andreas.Scripts.Actors
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : Actor
    {
        [NonSerialized] public NavMeshAgent NavAgent;

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        [NonSerialized] public Rigidbody Body;

        public EnemyData Data;
        public EnemyStateManager StateManager;

        public string State;

        private void Awake()
        {
            Health.Health = 3;

            StateManager = new(this);
            Body = gameObject.GetComponent<Rigidbody>();

            NavAgent = GetComponent<NavMeshAgent>();
            UpdateNavAgentStats();
        }

        public void UpdateNavAgentStats()
        {
            NavAgent.speed = Data.MoveSpeed;
            NavAgent.acceleration = Data.MoveSpeedAcceleration;
            NavAgent.angularSpeed = Data.TurnSpeed;
        }

        private void Start()
        {
            //  temp
            if(Data.Name.Equals("Rat"))
            {
                StateManager.SetState(new EnemyStateRatRoam());
            }
            else if(Data.Name.Equals("Seagull"))
            {
                StateManager.SetState(new EnemyStateSeagull(transform.position));
            }
            else
            {
                StateManager.SetState(new EnemyStateIdle());
            }
        }

        public void TakeDamage(int damage)
        {
            Health.Health -= damage;
            if(Health.Health <= 0)
            {
                Die();
            }
        }

        private void RotateTowardsDirection()
        {
            var direction = Body.velocity;
            if(direction == Vector3.zero)
                return;
            // transform.Rotate(direction);
            transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            Data.AttackLibrary.Update();
            StateManager.Update(Time.deltaTime);
            RotateTowardsDirection();
        }

        private void FixedUpdate()
        {
            StateManager.FixedUpdate(Time.fixedDeltaTime);
        }
    }
}