using System.Collections;
using System.Collections.Generic;
using Andreas.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseInput : MonoBehaviour {
    
    public void OnResume(InputAction.CallbackContext context) {
        if (context.performed) {
            GameManager.Instance.TogglePause();
        }
    }

    public void OnQuit(InputAction.CallbackContext context) {
        if (context.performed) {
        }
    }
}