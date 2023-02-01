using System;
using UnityEngine;

public class PlayerJoinManager : MonoBehaviour {

    public static PlayerJoinManager Instance = null;
    public int playerNumber;
    private void Awake() {
        playerNumber = 0;
        if (Instance == null)
            Instance = this;
        else if(Instance != this)
            Destroy(gameObject);
    }

    public void OnJoin() {
        playerNumber += 1;
        Debug.Log(playerNumber);
    }

    public void OnLeave() {
        playerNumber -= 1;
    }
}
