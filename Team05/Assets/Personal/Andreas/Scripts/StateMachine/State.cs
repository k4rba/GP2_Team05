namespace Andreas.Scripts.StateMachine
{
    public class State
    {
        public bool IsExited = false;
        public bool IsStarted = false;

        public virtual void Init() { }

        public virtual void Start()
        {
            IsStarted = true;
        }

        public virtual void Exit()
        {
            IsExited = true;
        }

        public virtual void Update(float dt) { }
        public virtual void FixedUpdate(float fixedDt) { }
    }
}