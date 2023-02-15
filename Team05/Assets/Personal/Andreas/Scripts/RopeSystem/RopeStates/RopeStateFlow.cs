using Andreas.Scripts.RopeSystem.SegmentStates;
using Util;

namespace Andreas.Scripts.RopeSystem.RopeStates
{
    public class RopeStateFlow : RopeStateBase
    {
        private int _startIndex;
        private int _endIndex;

        private int _index;
        private Timer _timer;

        public override void Start()
        {
            base.Start();
            _timer = Rope.Data.TransferSpeed / Rope.Segments.Count;
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            foreach(var t in _timer.UpdateTicks())
            {
                SetRopeCool();
                _index++;
                if(_index >= Rope.Segments.Count)
                {
                    Exit();
                    break;
                }
            }
        }

        public void SetRopeCool()
        {
            var segment = Rope.Segments[_index].GetComponent<RopeSegment>();
            segment.AddState(new RopeSegmentStateColor());
            segment.AddState(new RopeSegmentStateSize());
        }
        
    }
}