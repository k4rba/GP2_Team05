using System.Collections.Generic;
using FlowFieldSystem;
using Personal.Andreas.Scripts;
using UnityEngine;

namespace Andreas.Scripts
{
    public class FlowAgentManagerNew : MonoBehaviour
    {
        [SerializeField] private bool _agentsEnabled = true;

        private List<IFlowAgent> _flowAgents;

        private void Awake()
        {
            _flowAgents = new();
        }

        public void AddAgent(IFlowAgent agent, FlowFieldManager ff)
        {
            agent.FlowManager = ff;
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
                var spawner = EnemyManager.Get.Spawner;
                var ffs = spawner.Fields;

                for(int j = 0; j < ffs.Count; j++)
                {
                    var field = ffs[j];

                    for(int i = 0; i < _flowAgents.Count; i++)
                    {
                        var agent = _flowAgents[i];

                        if(agent.FlowManager != field)
                            continue;

                        var direction = field.GetDirection(agent.Position);
                        agent.FlowDirectionUpdated(direction);
                    }
                }
            }
        }
    }
}