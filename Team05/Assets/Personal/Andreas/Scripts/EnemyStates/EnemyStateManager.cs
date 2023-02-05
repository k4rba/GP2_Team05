using Andreas.Scripts.StateMachine;
using Personal.Andreas.Scripts.Actors;
using UnityEngine;

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
    }
}