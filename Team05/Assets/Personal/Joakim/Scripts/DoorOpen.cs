using AudioSystem;
using UnityEngine;
using DG.Tweening;

public class DoorOpen : MonoBehaviour {
    private float targetPos;
    public float timeToOpen;

    [SerializeField] private AudioClip _sound;
    
    private void Awake() {
        targetPos = transform.position.y - 5.5f;
    }
    public void DoorSlideOpen() {
        transform.DOMoveY(targetPos, timeToOpen, false);
        AudioManager.PlaySfx(_sound.name, transform.position);
    }
}