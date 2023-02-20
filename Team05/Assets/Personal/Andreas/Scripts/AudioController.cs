using AudioSystem;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip Sound;
    public void PlaySfx()
    {
        AudioManager.PlaySfx(Sound.name);
    }
}
