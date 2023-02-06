using System.Collections.Generic;
using Andreas.Scripts;
using Personal.Andreas.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour {
    public bool rangedLockedIn, meleeLockedIn;
    public GameObject characterSelectionScreen;
    public GameObject mainGameUI;
    public List<Player> Players = new List<Player>();

    public static CharacterManager Instance = null;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public void CheckIfAllAreLockedIn() {
        if (rangedLockedIn && meleeLockedIn) {
            characterSelectionScreen.SetActive(false);
            mainGameUI.SetActive(true);
            foreach (var player in Players) {
                player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
                player.AssignPlayerToRole(player.cType);
                player.HealthMaterial = Resources.Load<Material>("Player" + player._playerNumber + "Health");
                player.Health.ResetHealth(player);
                if (player._playerNumber == 1) {
                    player.otherPlayer = GameObject.FindWithTag("Player2");
                }
                else if (player._playerNumber == 2) {
                    player.otherPlayer = GameObject.FindWithTag("Player1");
                }
                CameraTopDown.Get.SetPlayers(player.transform);
                EdgeViewEnemySpawner.Get.EnableSpawning(true);
            }
        }
    }
}
