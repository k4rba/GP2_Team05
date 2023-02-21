using System;
using System.Collections;
using Andreas.Scripts;
using Andreas.Scripts.EnemyData;
using Andreas.Scripts.EnemyStates;
using Andreas.Scripts.EnemyStuff;
using Andreas.Scripts.StateMachine;
using Andreas.Scripts.StateMachine.States;
using AudioSystem;
using UnityEngine;
using UnityEngine.AI;

namespace Personal.Andreas.Scripts.Actors
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : Actor
    {
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        [NonSerialized] public NavMeshAgent NavAgent;
        [NonSerialized] public Rigidbody Body;
        [NonSerialized] public Vector3 LookDirection;

        public EnemyData Data;
        public EnemyStateManager StateManager;
        private StatesManager _statesManager;
        public string State;

        [SerializeField] private GameObject _model;

        public Transform ProjectileSpawnPosition;

        public Animator _animator;

        public bool EnteredCombat;

        private void Awake()
        {
            Health.Health = 3;

            StateManager = new(this);
            _statesManager = new();
            Body = gameObject.GetComponent<Rigidbody>();

            NavAgent = GetComponent<NavMeshAgent>();

            _animator = GetComponentInChildren<Animator>();
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
            else
            {
                _statesManager.AddState(new StateColorFlash(_model, Color.red));
            }
        }

        private void RotateTowardsDirection()
        {
            if(NavAgent.isOnNavMesh && !NavAgent.isStopped)
                LookDirection = NavAgent.velocity;

            if(LookDirection == Vector3.zero)
                return;
            
            // transform.Rotate(direction);
            // var rotation = transform.rotation;
            // var to = Quaternion.Euler(direction);
            // transform.rotation = Quaternion.RotateTowards(rotation, to, 90f * Time.fixedDeltaTime);
            transform.rotation = Quaternion.LookRotation(LookDirection.normalized);
        }

        public void Die()
        {
            Data.Sfx.OnDeath.Play(transform.position);
            _animator.SetTrigger("Die");
            StartCoroutine(WaitThenDieForAnimationsSake());
        }

        IEnumerator WaitThenDieForAnimationsSake() {
            yield return new WaitForSeconds(1);
            Destroy(gameObject);
        }

        private void Update()
        {
            Data.AttackLibrary.Update();
            StateManager.Update(Time.deltaTime);
            _statesManager.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            StateManager.FixedUpdate(Time.fixedDeltaTime);
            _statesManager.FixedUpdate(Time.fixedDeltaTime);
            RotateTowardsDirection();
        }
    }
}