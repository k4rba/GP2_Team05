using UnityEngine;
using Util;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateHunt : EnemyStateTarget
    {
        private Timer _destinationInterval = 0.2f;
        private Timer _sfxHintTimer = 2f;

        public EnemyStateHunt(GameObject target) : base(target)
        {
        }

        public override void Start()
        {
            base.Start();
            Enemy._animator.SetBool("Run", true);
            RandomizeAttack();
            UpdateDestination();

            if(!Enemy.EnteredCombat)
                Enemy.Data.Sfx.OnAggro.Play(Enemy.transform.position);
            Enemy.EnteredCombat = true;
        }

        private void UpdateDestination()
        {
            Enemy.NavAgent.isStopped = false;
            Enemy.NavAgent.destination = Target.transform.position;
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            if(_sfxHintTimer.UpdateTick())
            {
                Enemy.Data.Sfx.OnHint.Play(Enemy.transform.position);
            }

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
            Enemy._animator.SetBool("Run", false);
            Enemy.NavAgent.isStopped = true;
        }

        private void EnterAttack()
        {
            Enemy.StateManager.SetState(
                new EnemyStateWait(0.25f, new EnemyStateAttack(Target, Attack)));
        }
    }
}