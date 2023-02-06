using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class CharacterSelection : MonoBehaviour {
    public Vector2 selectDir;
    public int selectedDir;
    public int playerNumber = 0;
    private GameObject selectionObject;
    private GameObject meleeNode, rangedNode;
    private PlayerInput _playerInput;


    private void Awake() {
    }

    private void Start() {
        playerNumber = PlayerJoinManager.Instance.playerNumber;
        switch (playerNumber) {
            case 1:
                selectionObject = GameObject.Find("Player1Selector");
                meleeNode = GameObject.Find("P1-MeleeNode");
                rangedNode = GameObject.Find("P1-RangedNode");
                break;
            case 2:
                selectionObject = GameObject.Find("Player2Selector");
                meleeNode = GameObject.Find("P2-MeleeNode");
                rangedNode = GameObject.Find("P2-RangedNode");
                break;
        }
    }

    public void SwitchSelectionVisual() {
        SelectedCharacter(selectDir);
        switch (selectedDir) {
            case -1:
                if (selectionObject != null) {
                    selectionObject.transform.DOMove(rangedNode.transform.position, 0.3f, false);
                }

                break;
            case 1:
                if (selectionObject != null) {
                    selectionObject.transform.DOMove(meleeNode.transform.position, 0.3f, false);
                }

                break;
        }
    }

    private void SelectedCharacter(Vector2 vec) {
        if (vec.x < -0.2f) {
            selectedDir = -1;
        }

        if (vec.x > 0.2f) {
            selectedDir = 1;
        }
    }

    public void OnSelect(InputAction.CallbackContext context) {
        selectDir = context.ReadValue<Vector2>();
        SwitchSelectionVisual();
    }

    public void OnChoose(InputAction.CallbackContext context) {
        if (!context.performed) return;
        switch (selectedDir) {
            case -1:
                if (!CharacterManager.Instance.rangedLockedIn) {
                    CharacterManager.Instance.rangedLockedIn = true;
                    GetComponent<Player>().cType = Player.CharacterType.Ranged;
                    GetComponentInChildren<PlayerAttackScheme>().characterType = PlayerAttackScheme.Character.Ranged;
                    GetComponentInChildren<PlayerAttackScheme>().InitializeAttack();
                    CharacterManager.Instance.Players.Add(GetComponent<Player>());
                }
                else {
                    Debug.Log("Already Picked!");
                }

                break;
            case 1:
                if (!CharacterManager.Instance.meleeLockedIn) {
                    CharacterManager.Instance.meleeLockedIn = true;
                    GetComponent<Player>().cType = Player.CharacterType.Melee;
                    GetComponentInChildren<PlayerAttackScheme>().characterType = PlayerAttackScheme.Character.Melee;
                    GetComponentInChildren<PlayerAttackScheme>().InitializeAttack();
                    CharacterManager.Instance.Players.Add(GetComponent<Player>());
                }
                else {
                    Debug.Log("Already Picked!");
                }

                break;
        }
        CharacterManager.Instance.CheckIfAllAreLockedIn();
    }
}