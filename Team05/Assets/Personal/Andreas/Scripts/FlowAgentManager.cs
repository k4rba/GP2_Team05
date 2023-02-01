using System.Collections.Generic;
using Personal.Andreas.Scripts.Actors;
using Personal.Andreas.Scripts.Flowfield;
using UnityEngine;

namespace Personal.Andreas.Scripts
{
    public class FlowAgentManager : MonoBehaviour
    {
        [SerializeField] private FlowFieldManager _ffManager;
        [SerializeField] private bool _agentsEnabled = true;
        [SerializeField] private List<GameObject> _agents;

        private List<IFlowAgent> _flowAgents;

        private void Awake()
        {
            _flowAgents = new();

            for(int i = 0; i < _agents.Count; i++)
            {
                var comp = _agents[i].GetComponent<IFlowAgent>();
                _flowAgents.Add(comp);
            }
            
            if(_ffManager == null)
            {
                Debug.LogWarning("FlowFieldAgentManager is missing a FlowFieldManager");
            }
        }

        private void Update()
        {
            if(_agentsEnabled)
            {
                for(int i = 0; i < _flowAgents.Count; i++)
                {
                    var agent = _flowAgents[i];
                    var direction = _ffManager.GetDirection(agent.Position);
                    agent.Move(direction);
                }
            }
        }
    }
}