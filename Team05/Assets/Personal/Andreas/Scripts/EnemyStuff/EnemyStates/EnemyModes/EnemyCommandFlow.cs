using UnityEngine;

namespace Andreas.Scripts.EnemyStates.EnemyModes
{
    public class EnemyCommandFlow : EnemyCommand
    {
        private Rigidbody _body;
        
        public override void Init()
        {
            base.Init();
        }

        public override void Update(float dt)
        {
            base.Update(dt);
        }

        private void Move()
        {
            var tf = _body.transform;

            var direction = Enemy.FlowAgent.FlowDirection;
            
            if(direction == Vector2.zero)
            {
                //  flow is zero
                //Confused();
                return;
            }

            //  todo - take speed from some stat collection
            float speed = 5f;

            var pos = tf.position;
            var dir3 = new Vector3(direction.x, 0, direction.y);
            var velocity = dir3 * (speed * Time.deltaTime);
            pos += velocity;
            _body.MovePosition(pos);
        }
        
    }
}