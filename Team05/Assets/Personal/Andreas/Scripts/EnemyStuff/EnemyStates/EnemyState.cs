using Andreas.Scripts.EnemyStates.EnemyModes;
using Andreas.Scripts.StateMachine;
using Personal.Andreas.Scripts.Actors;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyState : State
    {
        protected Enemy Enemy;

        public EnemyState() {
        }

        public void InitEnemyState(Enemy enemy)
        {
            Enemy = enemy;
        }

        protected void SetCommand(EnemyCommand com)
        {
            // Enemy.StateCommandManager.SetState(com);
        }

    }
}