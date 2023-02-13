using AudioSystem;
using UnityEngine;
using Util;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateHunt : EnemyStateTarget
    {
        private Timer _destinationInterval = 0.2f;

        public EnemyStateHunt(GameObject target) : base(target)
        {
        }
        
        public override void Start()
        {
            base.Start();
            RandomizeAttack();
            // Enemy.FlowAgent.IsEnabled = true;
            UpdateDestination();
        }

        private void UpdateDestination()
        {
            // Enemy.NavAgent.destination = Enemy.FlowAgent.Target.transform.position;
            Enemy.NavAgent.isStopped = false;
            Enemy.NavAgent.destination = Target.transform.position;
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            if(_destinationInterval.UpdateTick())
            {
                UpdateDestination();
            }

            if(IsCloseForAttack())
            {
                EnterAttack();
            }
        }

        public override void Exit()
        {
            base.Exit();
            Enemy.NavAgent.isStopped = true;
            // Enemy.FlowAgent.IsEnabled = false;
        }

        private void EnterAttack()
        {
            Enemy.StateManager.SetState(
                new EnemyStateWait(0.5f, new EnemyStateAttack(Target, Attack)));
        }


    }
}