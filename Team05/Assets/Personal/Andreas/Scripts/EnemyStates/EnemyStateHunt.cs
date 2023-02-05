using Personal.Andreas.Scripts.Actors;
using UnityEngine;
using Util;

namespace Andreas.Scripts.EnemyStates {
    public class EnemyStateHunt : EnemyState {
        
        public EnemyStateHunt(Enemy enemy) : base(enemy) {
        }
        
        public override void Update(float dt) {
            base.Update(dt);
            if (IsCloseForAttack()) {
                EnterAttack();
            }
        }

        public override void FixedUpdate(float fixedDt) {
            base.FixedUpdate(fixedDt);
            Move();
        }

        private void EnterAttack() {
            Enemy.StateManager.SetState(new EnemyStateAttack(Enemy));
        }

        private bool IsCloseForAttack() {

            // float attackRange = 1.1f;
            float attackRange = Enemy.Data.AttackRange;

            var unit = Enemy.Manager.GetUnit();

            var distance = Vector3.Distance(Enemy.transform.position, unit.transform.position);
            return distance < attackRange;
        }

        private void Confused() {
            var rotationDir = new Vector3(0, Random.Range(100, 2000), 0);
            var rotationEuler = Quaternion.Euler(rotationDir * Time.fixedDeltaTime);
            Enemy.Body.MoveRotation(Enemy.Body.rotation * rotationEuler);
        }

        private void Move() {
            var body = Enemy.Body;
            var tf = body.transform;

            var direction = Enemy.FlowDirection;
            if (direction == Vector2.zero) {
                Confused();
                return;
            }

            //  todo - take speed from some stat collection
            // float speed = 5f;
            float speed = Enemy.Data.MoveSpeed;

            var pos = tf.position;
            var dir3 = direction.ToVector3XZ();
            var velocity = dir3 * (speed * Time.deltaTime);
            body.MovePosition(pos + velocity);
        }


    }
}