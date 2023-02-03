using Andreas.Scripts.EnemyStates.EnemyModes;
using Personal.Andreas.Scripts.Actors;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateIdle : EnemyState
    {
        public override void Start()
        {
            base.Start();
            
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            
        }

        public EnemyStateIdle(Enemy enemy) : base(enemy) {
        }
    }
}