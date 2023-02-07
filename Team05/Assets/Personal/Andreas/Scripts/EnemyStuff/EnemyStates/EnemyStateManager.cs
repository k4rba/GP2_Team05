using Andreas.Scripts.StateMachine;
using Personal.Andreas.Scripts.Actors;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateManager : StateManager
    {
        public Enemy Enemy;
        
        public EnemyStateManager(Enemy enemy)
        {
            Enemy = enemy;
        }

        public override void SetState(State state)
        {
            if(state is EnemyState enState)
            {
                enState.InitEnemyState(Enemy);
            }

            base.SetState(state);
        }

        public override void Update(float dt)
        {
            //  TODO - set a default state
            if(Current == null)
            {
                SetState(new EnemyStateIdle());
            }
            
            base.Update(dt);
            
        }
    }
}