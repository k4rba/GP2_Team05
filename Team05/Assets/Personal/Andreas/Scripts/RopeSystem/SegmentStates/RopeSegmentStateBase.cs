using Andreas.Scripts.StateMachine;

namespace Andreas.Scripts.RopeSystem.SegmentStates
{
    public abstract class RopeSegmentStateBase : State
    {
        public RealRope Rope;
        public RopeSegment Segment;
    }
}