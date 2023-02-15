using UnityEngine;

namespace Andreas.Scripts.RopeSystem.SegmentStates
{
    public class RopeSegmentStateSize : RopeSegmentStateBase
    {
        private Vector3 startScale;
        private Vector3 endScale;

        private float _time = 0.25f;
        private float _timer;

        public override void Start()
        {
            base.Start();

            _time = Rope.Data.SegmentTime;
            _timer = 0f;

            endScale = Segment.BaseScale;
            startScale = Segment.BaseScale * Rope.Data.SegmentScale;
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            _timer += dt;
            var p = _timer / _time;
            Segment.transform.localScale = Vector3.Lerp(startScale, endScale, p);

            if(_timer >= _time)
                Exit();
        }

        public override void Exit()
        {
            base.Exit();
            Segment.transform.localScale = endScale;
        }
    }
}