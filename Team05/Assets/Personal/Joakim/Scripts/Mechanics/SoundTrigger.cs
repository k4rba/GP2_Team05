using AudioSystem;
using UnityEngine;

public class SoundTrigger : MonoBehaviour {
    public AudioClip sound;
    [SerializeField] private bool _soundPlayed;

    private void OnTriggerEnter(Collider other) {
        
        var player = other.gameObject.GetComponent<Player>();
        if(player == null)
            return;
        
        if (!_soundPlayed) {
            AudioManager.PlaySfx(sound.name, transform.position);
            _soundPlayed = !_soundPlayed;
        }
        return;
    }
}