using Andreas.Scripts.EnemyStuff.EnemyAttacks;
using UnityEngine;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateAttack : EnemyStateTarget
    {
        public EnemyStateAttack(GameObject target, EnemyAttackBase attack) 
            : base(target, attack)
        {
        }

        public override void Start()
        {
            base.Start();
            // Debug.Log("enter attack");

            Attack.Start();
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
            // Debug.Log("exit attack");
            Enemy.StateManager.SetState(new EnemyStateWait(0.5f));
            // Enemy.CancelAttack();
            // Enemy.StateManager.SetState(new EnemyStateHunt());
        }
    }
}