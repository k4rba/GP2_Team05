using System;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.UIElements;
using TMPro;

public class CharacterSelection : MonoBehaviour {
    public Vector2 selectDir;
    public int selectedDir;
    public int playerNumber = 0;
    public GameObject selectionObject;
    public GameObject meleeNode, rangedNode;
    private PlayerInput _playerInput;

    private void Awake() {
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
                selectionObject.transform.DOMove(rangedNode.transform.position, 0.3f, false);
                break;
            case 1:
                selectionObject.transform.DOMove(meleeNode.transform.position, 0.3f, false);
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
                CharacterManager.Instance.rangedLockedIn = true;
                GetComponent<Player>().cType = Player.CharacterType.Ranged;
                GetComponentInChildren<PlayerAttackScheme>().characterType = PlayerAttackScheme.Character.Ranged;
                name = "RangedPlayer";
                GetComponentInChildren<PlayerAttackScheme>().InitializeAttack();
                // CharacterManager.Instance.DistributeCharacter(playerNumber, "Ranged");
                if (CharacterManager.Instance.CheckIfAllLockedIn()) {

                }
                break;
            case 1:
                CharacterManager.Instance.meleeLockedIn = true;
                GetComponent<Player>().cType = Player.CharacterType.Melee;
                GetComponentInChildren<PlayerAttackScheme>().characterType = PlayerAttackScheme.Character.Melee;
                name = "MeleePlayer";
                GetComponentInChildren<PlayerAttackScheme>().InitializeAttack();
                // CharacterManager.Instance.DistributeCharacter(playerNumber, "Melee");
                if (CharacterManager.Instance.CheckIfAllLockedIn()) {

                }
                break;
        }
    }
}