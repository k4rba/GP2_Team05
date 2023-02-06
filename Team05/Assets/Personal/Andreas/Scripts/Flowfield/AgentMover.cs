using System;
using UnityEngine;

namespace Andreas.Scripts.Flowfield
{
    [Serializable]
    public abstract class AgentMover : MonoBehaviour
    {
        public FlowFollowerAgent Agent { get; set; }

        private void Awake()
        {
            Agent = GetComponent<FlowFollowerAgent>();
        }

        public virtual void Move()
        {
        }
    }
}