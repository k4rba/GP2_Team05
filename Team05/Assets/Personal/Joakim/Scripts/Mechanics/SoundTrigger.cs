using AudioSystem;
using UnityEngine;

public class SoundTrigger : MonoBehaviour {
    public AudioClip sound;
    private bool _soundPlayed;

    private void OnTriggerEnter(Collider other) {
        if (!_soundPlayed) {
            AudioManager.PlaySfx(sound.name, transform.position);
            _soundPlayed = !_soundPlayed;
        }
        return;
    }
}