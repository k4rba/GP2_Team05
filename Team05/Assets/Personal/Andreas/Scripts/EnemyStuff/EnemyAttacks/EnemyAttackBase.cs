namespace Andreas.Scripts.EnemyStuff.EnemyAttacks
{
    public class EnemyAttackBase
    {
        public float CastTime;

        protected bool Attacked;

        public virtual void Start()
        {
            
        }
        
        public virtual void Update()
        {
        }

        public virtual void OnAttacked()
        {
            Attacked = true;
        }
    }
}