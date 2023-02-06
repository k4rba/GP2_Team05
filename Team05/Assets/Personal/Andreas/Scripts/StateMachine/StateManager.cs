using System.Collections.Generic;

namespace Andreas.Scripts.StateMachine
{
    public class StateManager
    {
        public State Current;
        private Queue<State> _queue;

        //  add default state

        public StateManager()
        {
            _queue = new();
        }

        public virtual void SetState(State state)
        {
            Current = state;
            Current.Init();
        }

        private void ExitState(State state)
        {
            if(!state.IsExited)
                state.Exit();

            if(_queue.Count > 0)
            {
                SetState(_queue.Dequeue());
            }
        }

        public virtual void Update(float dt)
        {
            
            //  add default state
            if(Current == null)
            {
                return;
            }

            if(!Current.IsStarted)
            {
                Current.Start();
            }

            Current.Update(dt);

            if(Current.IsExited)
            {
                ExitState(Current);
            }
        }
        
        public virtual  void FixedUpdate(float fixedDt)
        {
            //  add default state
            if(Current == null)
            {
                return;
            }
            
            Current.FixedUpdate(fixedDt);
        }
    }
}