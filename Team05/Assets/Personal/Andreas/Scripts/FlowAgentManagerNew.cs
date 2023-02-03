using System.Collections.Generic;
using FlowFieldSystem;
using Personal.Andreas.Scripts;
using UnityEngine;

namespace Andreas.Scripts
{
    public class FlowAgentManagerNew : MonoBehaviour
    {
        [SerializeField] private FlowFieldManager _ffManager;
        [SerializeField] private bool _agentsEnabled = true;
        [SerializeField] private EnemyManager _enemyManager;

        private List<IFlowAgent> _flowAgents;

        private void Awake()
        {
            _flowAgents = new();
            
            if(_ffManager == null)
            {
                Debug.LogWarning("FlowFieldAgentManager is missing a FlowFieldManager");
            }

            _enemyManager.OnEnemyAdded += EnemyManagerOnOnEnemyAdded;
        }


        private void EnemyManagerOnOnEnemyAdded(GameObject enemyObj)
        {
            var agentComp = enemyObj.GetComponent<IFlowAgent>();

            if(agentComp == null)
            {
                Debug.LogError("No IFlowAgent component found in Enemy on spawn");
                return;
            }

            AddAgent(agentComp);
        }

        public void AddAgent(IFlowAgent agent)
        {
            agent.Manager = _ffManager;
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
                    var direction = _ffManager.GetDirection(agent.Position);
                    agent.Move(direction);
                }
            }
        }
    }
}