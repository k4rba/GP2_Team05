using UnityEngine;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateHunt : EnemyState
    {
        public override void Start()
        {
            base.Start();
            Enemy.FlowAgent.IsEnabled = true;
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            if(IsCloseForAttack())
            {
                EnterAttack();
            }
        }

        public override void Exit()
        {
            base.Exit();
            Enemy.FlowAgent.IsEnabled = false;
        }

        private void EnterAttack()
        {
            Enemy.StateManager.SetState(new EnemyStateAttack());
        }

        private bool IsCloseForAttack()
        {
            // float attackRange = 1.1f;
            float attackRange = Enemy.Data.AttackRange;
            var target = Enemy.FlowAgent.Target;

            var distance = Vector3.Distance(Enemy.transform.position, target.transform.position);
            return distance < attackRange;
        }
    }
}