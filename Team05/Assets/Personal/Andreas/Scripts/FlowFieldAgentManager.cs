using System.Collections.Generic;
using UnityEngine;

namespace Personal.Andreas.Scripts
{
    public class FlowFieldAgentManager : MonoBehaviour
    {
        [SerializeField] private FlowFieldManager _ffManager;
        [SerializeField] private bool _agentsEnabled = true;
        [SerializeField] private List<GameObject> _agents;


        private void Awake()
        {
            if(_ffManager == null)
            {
                Debug.LogWarning("FlowFieldAgentManager is missing a FlowFieldManager");
            }
        }

        private void Update()
        {
            if(_agentsEnabled)
            {
                for(int i = 0; i < _agents.Count; i++)
                {
                    var agent = _agents[i];
                    MoveAgent(agent.transform);
                }
            }
        }

        private void MoveAgent(Transform tf)
        {
            float speed = 5f;
            var pos = tf.position;
            var direction = _ffManager.GetDirection(pos);
            var dir3 = new Vector3(direction.x, 0, direction.y);
            var velocity = dir3 * (speed * Time.deltaTime);
            pos += velocity;
            // tf.gameObject.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Acceleration);
            tf.gameObject.GetComponent<Rigidbody>().MovePosition(pos);
        }
    }
}