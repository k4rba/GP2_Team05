using Andreas.Scripts.RopeSystem.RopeStates;
using Andreas.Scripts.StateMachine;
using UnityEngine;

namespace Andreas.Scripts.RopeSystem
{
    public class RopeVisualThingy : MonoBehaviour
    {
        private StatesManager _manager;

        private void Awake()
        {
            _manager = new();
        }

        public void AddState(RopeStateBase state)
        {
            state.Rope = GameManager.Instance.RopeManager.Rope;
            _manager.AddState(state);
        }

        private void Update()
        {
            _manager.Update(Time.deltaTime);

            if(Input.GetKeyDown(KeyCode.G))
            {
                AddState(new RopeStateFlow());
            }
            
        }

        private void FixedUpdate()
        {
            _manager.FixedUpdate(Time.fixedDeltaTime);
        }
    }
}