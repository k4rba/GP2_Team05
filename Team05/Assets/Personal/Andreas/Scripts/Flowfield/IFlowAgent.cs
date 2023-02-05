using UnityEngine;

namespace FlowFieldSystem
{
    public interface IFlowAgent
    {
        Vector3 Position { get; set; }
        FlowFieldManager Manager { get; set; }
        void FlowDirectionUpdated(Vector2 direction);
    }
}