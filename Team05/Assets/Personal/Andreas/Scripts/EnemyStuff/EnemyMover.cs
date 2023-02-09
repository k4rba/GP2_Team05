using Andreas.Scripts.Flowfield;
using UnityEngine;
using Util;

namespace Andreas.Scripts.EnemyStuff
{
    public class EnemyMover : AgentMover
    {
        public float Speed = 3f;

        private void Confused()
        {
            var rotationDir = new Vector3(0, Rng.Next(100, 2000), 0);
            var rotationEuler = Quaternion.Euler(rotationDir * Time.fixedDeltaTime);
            Agent.Body.MoveRotation(Agent.Body.rotation * rotationEuler);
        }

        public override void Move()
        {
            if(Agent.FlowDirection == Vector2.zero)
            {
                // Confused();
                return;
            }

            var body = Agent.Body;
            var bodyPos = body.position;
            var dir = Agent.FlowDirection.ToVector3XZ();

            if(dir == Vector3.zero)
                return;
            
            var velocity = dir * (Speed * Time.deltaTime);
            body.MovePosition(bodyPos + velocity);

            if(velocity == Vector3.zero)
                return;
            
            body.transform.rotation = Quaternion.LookRotation(velocity);
        }
    }
}