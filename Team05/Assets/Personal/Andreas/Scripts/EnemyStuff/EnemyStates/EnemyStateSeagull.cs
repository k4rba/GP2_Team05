using UnityEngine;
using Util;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateSeagull : EnemyState
    {
        public float MaxVelocity = 5f;
        
        private Vector3 _center;
        private Vector3 _destination;

        private Timer _destinationChangeTimer = 2f;
        private Timer _randomPushTimer = 3f;


        public EnemyStateSeagull(Vector3 transform)
        {
            _center = transform;
        }

        public override void Start()
        {
            base.Start();

            UpdateDestination();
            var pos = Random.insideUnitCircle * Rng.NextF(1f, 3f);
            Enemy.transform.position = new Vector3(pos.x, Enemy.transform.position.y, pos.y);
        }

        private void UpdateDestination()
        {
            _destination = _center + Random.insideUnitSphere * Rng.NextF(2f);
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            if(_destinationChangeTimer.UpdateTick())
            {
                UpdateDestination();
            }

            if(_randomPushTimer.UpdateTick())
            {
                var pushVel = Random.insideUnitSphere * Rng.NextF(1f, 2f);
                Enemy.Body.AddForce(pushVel, ForceMode.VelocityChange);
            }

        }

        public override void FixedUpdate(float fixedDt)
        {
            base.FixedUpdate(fixedDt);
            var center = _destination;
            var enemyPos = Enemy.transform.position;

            var dir = (center - enemyPos).normalized;
            var distance = Vector3.Distance(center, enemyPos);
            var speed = Enemy.Data.MoveSpeed * distance * 0.05f;
            var vel = (dir + Random.insideUnitSphere * 0.5f).normalized * (speed * Time.fixedDeltaTime);

            
            Enemy.Body.AddForce(vel, ForceMode.VelocityChange);
            Enemy.Body.velocity = Vector3.ClampMagnitude(Enemy.Body.velocity, MaxVelocity);
        }
    }
}