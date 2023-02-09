using UnityEngine;
using Util;

namespace Andreas.Scripts.EnemyStuff.EnemyAttacks
{
    public class EnemyAttackLeap : EnemyAttackBase
    {
        private float _jumpTime;
        private float _timer;
        private float _height = 5f;

        private Vector3 _startPos;
        private Vector3 _endPos;

        public override void Start()
        {
            base.Start();

            _jumpTime = CastTimer;

            _startPos = Enemy.transform.position;

            //  leap destination random offset
            float radius = Rng.NextF(0.5f, 1.5f);
            Vector3 dir = Rng.RandomDirection;
            var endOffset = radius * dir;
            
            _endPos = Target.position + endOffset;
        }

        public override void Update()
        {
            base.Update();

            var dt = Time.deltaTime;
            _timer += dt;

            var lerpValue = _timer / _jumpTime;
            var pos = Vector3.Lerp(_startPos, _endPos, lerpValue);
            pos.y = Mathf.Lerp(_startPos.y, _endPos.y, lerpValue) + Mathf.Sin(lerpValue * Mathf.PI) * _height;

            Enemy.Position = pos;

            if(_timer <= 0)
                OnAttacked();
        }

        private void OnImpact()
        {
            // var impactArea = ;

        }
        
        public override void OnAttacked()
        {
            base.OnAttacked();
            OnImpact();
        }
    }
}