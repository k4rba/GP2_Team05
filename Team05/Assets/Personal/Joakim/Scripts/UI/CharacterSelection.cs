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
    public bool debugMode;

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

    private void Update() {
        
        //DEBUG ONLY
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1)) {
            selectedDir = -1;
            SwitchSelectionVisual();
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2)) {
            selectedDir = 1;
            SwitchSelectionVisual();
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Space)) {
            switch (selectedDir) {
                case -1:
                    CharacterManager.Instance.rangedLockedIn = true;
                    GetComponent<Player>().cType = Player.CharacterType.Ranged;
                    GetComponentInChildren<PlayerAttackScheme>().characterType = PlayerAttackScheme.Character.Ranged;
                    name = "RangedPlayer";
                    GetComponentInChildren<PlayerAttackScheme>().InitializeAttack();
                    GameObject.Find("CharacterSelection").SetActive(false);
                    GameObject otherPlayerMelee = Resources.Load<GameObject>("Player");
                    Instantiate(otherPlayerMelee, new Vector3(0, 0, 0), Quaternion.identity);
                    otherPlayerMelee.transform.Find("AttackTypeHolder").GetComponent<PlayerAttackScheme>().characterType =
                        PlayerAttackScheme.Character.Melee;
                    otherPlayerMelee.GetComponent<Player>().cType = Player.CharacterType.Melee;
                    otherPlayerMelee.GetComponent<Player>().debugMode = true;
                    break;
                case 1:
                    CharacterManager.Instance.meleeLockedIn = true;
                    GetComponent<Player>().cType = Player.CharacterType.Melee;
                    GetComponentInChildren<PlayerAttackScheme>().characterType = PlayerAttackScheme.Character.Melee;
                    name = "MeleePlayer";
                    GetComponentInChildren<PlayerAttackScheme>().InitializeAttack();
                    GameObject.Find("CharacterSelection").SetActive(false);
                    GameObject otherPlayerRanged = Resources.Load<GameObject>("Player");
                    Instantiate(otherPlayerRanged, new Vector3(0, 0, 0), Quaternion.identity);
                    otherPlayerRanged.transform.Find("AttackTypeHolder").GetComponent<PlayerAttackScheme>().characterType =
                        PlayerAttackScheme.Character.Ranged;
                    otherPlayerRanged.GetComponent<Player>().cType = Player.CharacterType.Ranged;
                    otherPlayerRanged.GetComponent<Player>().debugMode = true;
                    break;
            }
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.F1)) {
            Debug.Log("Debug Mode Enabled");
            GetComponent<PlayerInput>().SwitchCurrentActionMap("TestingMap");
        }
        //END OF DEBUG
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
                break;
            case 1:
                CharacterManager.Instance.meleeLockedIn = true;
                GetComponent<Player>().cType = Player.CharacterType.Melee;
                GetComponentInChildren<PlayerAttackScheme>().characterType = PlayerAttackScheme.Character.Melee;
                name = "MeleePlayer";
                GetComponentInChildren<PlayerAttackScheme>().InitializeAttack();
                break;
        }
    }
}