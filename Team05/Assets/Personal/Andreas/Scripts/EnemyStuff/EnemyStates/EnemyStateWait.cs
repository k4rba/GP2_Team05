using UnityEngine;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateWait : EnemyState
    {
        private float _timer;
        private bool _exited;
        private EnemyState _nextState;
        
        public EnemyStateWait(float waitTime) : this(waitTime, null){ }
        public EnemyStateWait(float waitTime, EnemyState nextState)
        {
            _timer = waitTime;
            _nextState = nextState;
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            _timer -= dt;

            if(!_exited && _timer <= 0)
            {
                Exit();
            }
        }

        public override void Exit()
        {
            base.Exit();
            _exited = true;
            if(_nextState != null)
            {
                Enemy.StateManager.SetState(_nextState);
            }
        }
    }
}