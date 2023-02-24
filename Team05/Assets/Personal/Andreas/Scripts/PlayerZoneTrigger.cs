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
            var player = other.GetComponent<Player>();
            if(player != null)
            {
                OnEnterZone?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var player = other.GetComponent<Player>();
            if(player != null)
            {
                OnExitZone?.Invoke();
            }
        }
    }
}