using Andreas.Scripts.EnemyStuff;
using Andreas.Scripts.EnemyStuff.EnemyAttacks;
using UnityEngine;

namespace Andreas.Scripts.EnemyStates
{
    public abstract class EnemyStateTarget : EnemyState
    {
        protected GameObject Target;
        protected EnemyAttackBase Attack;

        protected EnemyStateTarget(GameObject target)
        {
            Target = target;
        }

        protected EnemyStateTarget(GameObject target, EnemyAttackBase attack)
        {
            Target = target;
            Attack = attack;
        }

        private EnemyAttackBase CreateAttack(EnemyAttackData data)
        {
            EnemyAttackBase attack = null;
            
            switch(data.Type)
            {
                case Attacks.BasicMelee: 
                    attack = new EnemyAttackBasicMelee();
                    break;
                
                case Attacks.BasicRanged:  
                    attack = new EnemyAttackBasicRanged();
                    break;
                
                case Attacks.Leap:
                    attack = new EnemyAttackLeap();
                    break;
                
                case Attacks.StunGrenade:
                    attack = new EnemyAttackStunGun();
                    break;
            }

            if(attack == null)
            {
                Debug.LogError("ATTACK WAS NULL !!");
                return null;
            }

            attack.Target = Target.transform;
            attack.Data = data;
            attack.Enemy = Enemy;

            return attack;
        }
        
        protected virtual void RandomizeAttack()
        {
            var attackData = Enemy.Data.AttackLibrary.GetRandomValidAttack();
            Attack = CreateAttack(attackData); 
        }
        
        protected bool IsCloseForAttack()
        {
            float attackRange = Attack.Data.Range;
            // var target = Enemy.FlowAgent.Target;
            var target = Target;

            var distance = Vector3.Distance(Enemy.transform.position, target.transform.position);
            return distance < attackRange;
        }
    }
}