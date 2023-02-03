using Personal.Andreas.Scripts.Actors;
using UnityEngine;
using Util;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateAttack : EnemyState {
        private static GameObject _prefAttackBox;

        private float _waitTime = 1.5f;
        
        public override void Init() {
            base.Init();

            if (_prefAttackBox == null) {
                LoadPrefab();
            Debug.Log("Loaded prefab boxattack thignue");
            }
        }

        private void LoadPrefab() {
            _prefAttackBox = Resources.Load<GameObject>("EnemyAttackBox");
        }

        public override void Start() {
            base.Start();
            Enemy.DoAttack(_prefAttackBox);
        }

        public override void Update(float dt) {
            base.Update(dt);

            _waitTime -= dt;
            
            if (_waitTime < 0f && !IsCloseForAttack()) {
                Exit();
            }
        }
        
        private bool IsCloseForAttack() {
            float attackRange = 1.1f;

            var unit = Enemy.Manager.GetUnit();

            var distance = Vector3.Distance(Enemy.transform.position, unit.transform.position);
            return distance < attackRange;
        }

        public override void Exit() {
            base.Exit();
            Enemy.CancelAttack();
            Debug.Log("EXITED ATTACK");
            
            Enemy.StateManager.SetState(new EnemyStateHunt(Enemy));
        }

        public EnemyStateAttack(Enemy enemy) : base(enemy) {
        }
    }
}