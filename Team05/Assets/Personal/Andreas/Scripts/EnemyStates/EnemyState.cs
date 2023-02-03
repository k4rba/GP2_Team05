using Andreas.Scripts.EnemyStates.EnemyModes;
using Andreas.Scripts.StateMachine;
using Personal.Andreas.Scripts.Actors;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyState : State
    {
        private Enemy _enemy;

        protected void SetCommand(EnemyCommand com)
        {
            _enemy.StateCommandManager.SetState(com);
        }

    }
}