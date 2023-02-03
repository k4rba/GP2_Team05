using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour {
    public bool rangedLockedIn, meleeLockedIn;
    public GameObject characterSelectionScreen;
    public GameObject mainGameUI;

    public static CharacterManager Instance = null;
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else if(Instance != this)
            Destroy(gameObject);
    }
    
    public bool CheckIfAllLockedIn() {
        if (rangedLockedIn && meleeLockedIn) {
            characterSelectionScreen.SetActive(false);
            mainGameUI.SetActive(true);
            return true;
        }
        else {
            return false;
        }
    }
}
