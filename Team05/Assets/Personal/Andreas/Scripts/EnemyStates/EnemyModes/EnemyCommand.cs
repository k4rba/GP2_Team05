using Andreas.Scripts.StateMachine;
using Personal.Andreas.Scripts.Actors;

namespace Andreas.Scripts.EnemyStates.EnemyModes
{
    public class EnemyCommand : State
    {
        protected EnemyState State;
        protected Enemy Enemy;

        public override void Init()
        {
            base.Init();
            
        }
        
    }
}