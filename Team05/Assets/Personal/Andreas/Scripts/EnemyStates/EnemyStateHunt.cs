using Unity.VisualScripting;
using Health;
using Personal.Andreas.Scripts.Actors;
using UnityEngine;

namespace Andreas.Scripts.EnemyStates {
    public class EnemyStateHunt : EnemyState {
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
            Debug.Log("ATTACK ENTER");
        }

        private bool IsCloseForAttack() {
            float attackRange = 1.1f;

            var unit = Enemy.Manager.GetUnit();

            var distance = Vector3.Distance(Enemy.transform.position, unit.transform.position);
            return distance < attackRange;
        }

        private void Confused() {
            var rotationDir = new Vector3(0, Random.Range(200, 1000), 0);
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
            float speed = 5f;

            var pos = tf.position;
            var dir3 = new Vector3(direction.x, 0, direction.y);
            var velocity = dir3 * (speed * Time.deltaTime);
            pos += velocity;
            body.MovePosition(pos);
        }

        public EnemyStateHunt(Enemy enemy) : base(enemy) {
        }
    }
}