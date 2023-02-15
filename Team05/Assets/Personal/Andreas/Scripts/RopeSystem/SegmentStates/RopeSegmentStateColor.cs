using UnityEngine;

namespace Andreas.Scripts.RopeSystem.SegmentStates
{
    public class RopeSegmentStateColor : RopeSegmentStateBase
    {
        private Color _startColor;
        private Color _endColor;

        private float _time = 0.25f;
        private float _timer;

        private Material _material;
        
        public override void Start()
        {
            base.Start();
            _time = Rope.Data.SegmentTime;
            
            _startColor = Color.red;
            _endColor = Segment.BaseColor;
            _material = Segment.gameObject.GetComponent<MeshRenderer>().material;
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            _timer += dt;
            
            var p = _timer / _time;
            _material.color = Color.Lerp(_startColor, _endColor, p);

            if(_timer >= _time)
            {
                Exit();
            }
            
        }

        public override void Exit()
        {
            base.Exit();
            _material.color = _endColor;
        }
    }
}