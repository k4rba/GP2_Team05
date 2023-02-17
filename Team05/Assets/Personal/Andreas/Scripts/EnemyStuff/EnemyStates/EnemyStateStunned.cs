namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateStunned : EnemyState
    {
        private float _timer;
        
        public EnemyStateStunned(float duration)
        {
            _timer = duration;
        }

        public override void Start()
        {
            base.Start();
            Enemy._animator.SetTrigger("Stun");
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            _timer -= dt;

            if(_timer <= 0)
            {
                Exit();
            }
        }
    }
}