using UnityEngine;

namespace Andreas.Scripts.EnemyStates {
    public class EnemyStateKnock : EnemyState {
        private Vector3 _direction;
        private float _power;
        private float _waitTimer = 1f;

        public EnemyStateKnock(Vector3 direction, float power) {
            _direction = direction;
            _power = power;
        }

        public override void Start() {
            base.Start();
            Enemy.Body.AddForce(_direction * _power, ForceMode.VelocityChange);
        }

        public override void Update(float dt) {
            base.Update(dt);
            _waitTimer -= dt;
            if(_waitTimer<=0)
                Exit();
        }
    }
}