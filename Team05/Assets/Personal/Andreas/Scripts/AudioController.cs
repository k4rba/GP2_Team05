using AudioSystem;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public void PlaySfx()
    {
        AudioManager.PlaySfx(name);
    }
}
