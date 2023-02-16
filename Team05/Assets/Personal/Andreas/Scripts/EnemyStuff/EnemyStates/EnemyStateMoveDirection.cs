using UnityEngine;
using UnityEngine.AI;
using Util;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateMoveDirection : EnemyState
    {
        private Vector3 _position;
        private Vector3 _destination;


        private Timer _exitTimer = 2f;

        private float _minDistance = 1f;

        public EnemyStateMoveDirection(Vector3 position)
        {
            _position = position;
        }

        private void SetDestination()
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(_position, out hit, 10f, 1);
            Enemy.NavAgent.isStopped = false;
            Enemy.NavAgent.destination = _destination = hit.position;
        }
        
        public override void Start()
        {
            base.Start();
            SetDestination();
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            var distance = Enemy.transform.position.FastDistance(_destination);

            if(distance <= _minDistance || _exitTimer.UpdateTick())
            {
                Exit();
            }

        }

        public override void Exit()
        {
            base.Exit();
            Enemy.NavAgent.isStopped = true;
            
        }
    }
}