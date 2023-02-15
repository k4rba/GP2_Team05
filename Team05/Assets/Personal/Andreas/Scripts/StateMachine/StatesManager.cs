using System.Collections.Generic;

namespace Andreas.Scripts.StateMachine
{
    public class StatesManager
    {
        public List<State> States;

        public StatesManager()
        {
            States = new();
        }

        public void AddState(State state)
        {
            state.Init();
            States.Add(state);
        }

        private void ExitState(State state)
        {
            if(!state.IsExited)
            {
                state.Exit();
            }

            RemoveState(state);
        }

        private void RemoveState(State state)
        {
            States.Remove(state);
        }

        public void Update(float dt)
        {
            for(int i = 0; i < States.Count; i++)
            {
                var state = States[i];
                
                if(!state.IsStarted)
                    state.Start();
                
                if(state.IsExited)
                {
                    ExitState(state);
                    continue;
                }

                state.Update(dt);
            }
        }

        public void FixedUpdate(float dt)
        {
            for(int i = 0; i < States.Count; i++)
            {
                var state = States[i];
                state.FixedUpdate(dt);
            }
        }
    }
}