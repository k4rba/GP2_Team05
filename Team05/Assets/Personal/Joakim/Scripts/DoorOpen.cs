using System;
using UnityEngine;
using DG.Tweening;

public class DoorOpen : MonoBehaviour {
    private float targetPos;
    public float timeToOpen;
    
    private void Awake() {
        targetPos = transform.position.y - 7f;
    }
    public void DoorSlideOpen() {
        transform.DOMoveY(targetPos, timeToOpen, false);
    }
}