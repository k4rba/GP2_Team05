using System;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UIElements.Image;

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
        Debug.Log("JOINED");
        playerNumber += 1;
        switch (playerNumber) {
            case 1:
                GameObject.Find("Player1Selector").GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
                break;
            case 2:
                GameObject.Find("Player2Selector").GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
                break;
        }
    }

    public void OnLeave() {
        playerNumber -= 1;
    }
}
