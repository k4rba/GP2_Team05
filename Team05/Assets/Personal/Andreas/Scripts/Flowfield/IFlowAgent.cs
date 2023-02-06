using UnityEngine;

namespace FlowFieldSystem
{
    public interface IFlowAgent
    {
        Vector3 Position { get; set; }
        FlowFieldManager FlowManager { get; set; }
        void FlowDirectionUpdated(Vector2 direction);
    }
}