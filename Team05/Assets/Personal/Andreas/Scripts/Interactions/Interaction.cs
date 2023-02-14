using System;
using UnityEngine;
using UnityEngine.Events;

namespace Andreas.Scripts.Interactions
{
    public class Interaction : MonoBehaviour
    {
        public UnityEvent OnTrigger;

        private void OnTriggerEnter(Collider other)
        {
            if(other is IInteractor interactor)
            {
                interactor.OnInteract += InteractorOnOnInteract;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other is IInteractor interactor)
            {
                interactor.OnInteract -= InteractorOnOnInteract;
            }
        }

        private void InteractorOnOnInteract()
        {
            OnTrigger?.Invoke();
        }
    }
}