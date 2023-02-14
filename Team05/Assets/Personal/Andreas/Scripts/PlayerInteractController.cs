using System;
using UnityEngine;

namespace Andreas.Scripts
{
    public class PlayerInteractController : MonoBehaviour, IInteractor
    {
        public event Action OnInteract;

        private void Start()
        {
            var player = GetComponent<Player>();
            player.OnInteracted += PlayerOnOnInteracted;
        }

        private void PlayerOnOnInteracted()
        {
            Interact();
        }

        public void Interact()
        {
            Debug.Log("controller interact");
            OnInteract?.Invoke();
        }
    }
}