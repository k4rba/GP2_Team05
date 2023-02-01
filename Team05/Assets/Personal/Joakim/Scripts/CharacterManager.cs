using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour {
    public bool rangedLockedIn, meleeLockedIn;
    public GameObject characterSelectionScreen;

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
            return true;
        }
        else {
            return false;
        }
    }

    public void DistributeCharacter(int playerNumber, string cType) {
        if (cType == "Ranged") {
            Debug.Log(playerNumber + " is ranged");
        }

        if (cType == "Melee") {
            Debug.Log(playerNumber + " is melee");
        }
    }
}
