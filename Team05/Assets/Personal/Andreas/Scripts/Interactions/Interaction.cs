using UnityEngine;
using UnityEngine.Events;

namespace Andreas.Scripts.Interactions
{
    public class Interaction : MonoBehaviour
    {
        public UnityEvent OnTrigger;

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<PlayerInteractController>(); 
            if(player is IInteractor interactor)
            {
                interactor.OnInteract += InteractorOnOnInteract;
                Debug.Log("player enter");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var player = other.GetComponent<PlayerInteractController>(); 
            if(player is IInteractor interactor)
            {
                interactor.OnInteract -= InteractorOnOnInteract;
                Debug.Log("player exit");
            }
        }

        private void InteractorOnOnInteract()
        {
            Debug.Log("interactor trigger");
            OnTrigger?.Invoke();
        }
    }
}