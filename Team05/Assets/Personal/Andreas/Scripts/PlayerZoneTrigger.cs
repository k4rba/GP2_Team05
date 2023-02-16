using UnityEngine;
using UnityEngine.Events;

namespace Andreas.Scripts
{
    public class PlayerZoneTrigger : MonoBehaviour
    {
        public UnityEvent OnEnterZone;
        public UnityEvent OnExitZone;

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<Player>())
            {
                OnEnterZone?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.GetComponent<Player>())
            {
                OnExitZone?.Invoke();
            }
        }
    }
}
