using UnityEngine;

namespace Personal.Andreas.Scripts.Flowfield
{
    public interface IFlowAgent
    {
        public Vector3 Position { get; set; }
        void Move(Vector2 direction);
    }
}