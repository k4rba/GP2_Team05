using System.Collections.Generic;
using FlowFieldSystem;
using UnityEngine;

namespace Andreas.Scripts
{
    public class FlowAgentUpdater : MonoBehaviour
    {
        [SerializeField] private bool _agentsEnabled = true;

        private List<IFlowAgent> _flowAgents;

        private void Awake()
        {
            _flowAgents = new();
        }

        public void AddAgent(IFlowAgent agent, FlowFieldManager ff)
        {
            agent.FlowField = ff;
            _flowAgents.Add(agent);
        }

        public void RemoveAgent(IFlowAgent agent)
        {
            _flowAgents.Remove(agent);
        }

        private void Update()
        {
            if(_agentsEnabled)
            {
                for(int i = 0; i < _flowAgents.Count; i++)
                {
                    var agent = _flowAgents[i];
                    var direction = agent.FlowField.GetDirection(agent.Position);
                    agent.FlowDirectionUpdated(direction);
                }
            }
        }
    }
}