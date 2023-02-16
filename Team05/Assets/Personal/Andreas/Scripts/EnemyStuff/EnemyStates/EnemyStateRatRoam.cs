using UnityEngine;
using UnityEngine.AI;
using Util;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateRatRoam : EnemyState
    {
        private float _walkCooldown = 3f;
        private float _walkCooldownTimer = 1f;

        public override void Start()
        {
            base.Start();
            SetDestinationRandom();
        }

        private void SetDestinationRandom()
        {
            const float walkRadiusRange = 1.5f;

            float roll = Rng.NextF(0.1f, 1f);
            float radius = walkRadiusRange * roll;
            _walkCooldownTimer = 0.5f + _walkCooldown * roll;

            var enemyPosition = Enemy.transform.position;
            var rndScanDirection = enemyPosition + Random.insideUnitSphere * radius;
            NavMeshHit hit;
            NavMesh.SamplePosition(rndScanDirection, out hit, radius, 1);
            Enemy.NavAgent.destination = hit.position;
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            _walkCooldownTimer -= dt;
            if(_walkCooldownTimer <= 0f)
            {
                SetDestinationRandom();
            }
        }
    }
}