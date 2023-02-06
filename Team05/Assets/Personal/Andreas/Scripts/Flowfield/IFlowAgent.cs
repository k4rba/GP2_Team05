using UnityEngine;

namespace FlowFieldSystem
{
    public interface IFlowAgent
    {
        Vector3 Position { get; set; }
        FlowFieldManager FlowField { get; set; }
        void FlowDirectionUpdated(Vector2 direction);
    }
}