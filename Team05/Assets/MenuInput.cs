using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInput : MonoBehaviour {
    public GameObject cSelection;
    public GameObject playerJoinManager;

    public void OnStart(InputAction.CallbackContext context) {
        if (context.performed) {
            StartGame();
        }
    }

    public void OnExit(InputAction.CallbackContext context) {
        if (context.performed) {
            Application.Quit();
        }
    }

    private void StartGame()
    {
        cSelection.SetActive(true);
        transform.parent.gameObject.SetActive(false);
        playerJoinManager.SetActive(true);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.F2)) {
            StartGame();
        }
    }
}