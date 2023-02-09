
namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateHunt : EnemyStateTarget
    {
        
        public override void Start()
        {
            base.Start();
            RandomizeAttack();
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
            Enemy.StateManager.SetState(
                new EnemyStateWait(0.5f, new EnemyStateAttack(Attack)));
        }


    }
}