using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows.WebCam;

public class MenuInput : MonoBehaviour {
    public GameObject cSelection;
    public GameObject playerJoinManager;

    public void OnStart(InputAction.CallbackContext context) {
        if (context.performed) {
            cSelection.SetActive(true);
            transform.parent.gameObject.SetActive(false);
            playerJoinManager.SetActive(true);
        }
    }

    public void OnExit(InputAction.CallbackContext context) {
        if (context.performed) {
            Application.Quit();
        }
    }
}