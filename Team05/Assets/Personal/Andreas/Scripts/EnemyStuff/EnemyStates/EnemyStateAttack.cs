using Andreas.Scripts.EnemyStuff.EnemyAttacks;
using UnityEngine;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateAttack : EnemyStateTarget
    {
        public EnemyStateAttack(GameObject target, EnemyAttackBase attack) 
            : base(target, attack) { }

        public override void Start()
        {
            base.Start();
            Attack.Start();
            var dirToTarget = (Target.transform.position - Enemy.transform.position).normalized;
            Enemy.LookDirection = dirToTarget;
            // Enemy._animator.SetTarget();
            Enemy._animator.SetTrigger("Hit");
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            Attack.Update();
            if(Attack.Attacked)
            {
                Exit();
            }
        }

        public override void Exit()
        {
            base.Exit();
            Enemy.StateManager.SetState(new EnemyStateWait(0.25f));
        }
    }
}