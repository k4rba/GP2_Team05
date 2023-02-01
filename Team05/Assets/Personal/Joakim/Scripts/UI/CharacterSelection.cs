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
    public TextMeshProUGUI playerNumberText;
    public bool allLockedIn, rangedLockedIn, meleeLockedIn;

    private void Awake() {
        playerNumberText = GetComponentInChildren<TextMeshProUGUI>();
        selectionObject = this.gameObject;
        selectionObject.transform.parent = GameObject.Find("CharacterSelection").transform;
        playerNumber = PlayerJoinManager.Instance.playerNumber;
        playerNumberText.text = playerNumber.ToString();
        switch (playerNumber) {
            case 1:
                name = "Player1Selector";
                meleeNode = GameObject.Find("P1-MeleeNode");
                rangedNode = GameObject.Find("P1-RangedNode");
                break;
            case 2:
                name = "Player2Selector";
                meleeNode = GameObject.Find("P2-MeleeNode");
                rangedNode = GameObject.Find("P2-RangedNode");
                break;
        }
    }

    private void Update() {
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
    }

    public void OnChoose(InputAction.CallbackContext context) {
        if (!context.performed) return;
        switch (selectedDir) {
            case -1:
                rangedLockedIn = true;
                break;
            case 1:
                meleeLockedIn = true;
                break;
        }
    }
}