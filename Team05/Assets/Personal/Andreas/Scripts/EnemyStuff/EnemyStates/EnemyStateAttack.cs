using Andreas.Scripts.EnemyStuff.EnemyAttacks;
using UnityEngine;
using Util;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateAttack : EnemyState
    {
        private static GameObject _prefAttackBox;
        private float _waitTime = 1.5f;

        private EnemyAttackBase _attack;

        public EnemyStateAttack() { }
        public EnemyStateAttack(EnemyAttackBase attack)
        {
            _attack = attack;
        }

        public override void Init()
        {
            base.Init();

            if(_prefAttackBox == null)
            {
                LoadPrefab();
            }
        }

        private void LoadPrefab()
        {
            _prefAttackBox = FastResources.Load<GameObject>("EnemyAttackBox");
        }

        public override void Start()
        {
            base.Start();
            _attack.Start();
            Enemy.DoAttack(_prefAttackBox);
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            _attack.Update();

            _waitTime -= dt;

            if(_waitTime < 0f && !IsCloseForAttack())
            {
                Exit();
            }
        }

        private bool IsCloseForAttack()
        {
            float attackRange = Enemy.Data.AttackRange;
            var unit = Enemy.FlowAgent.Target;

            var distance = Enemy.transform.position.FastDistance(unit.transform.position);
            return distance < attackRange;
        }

        public override void Exit()
        {
            base.Exit();
            Enemy.CancelAttack();
            Enemy.StateManager.SetState(new EnemyStateHunt());
        }
    }
}