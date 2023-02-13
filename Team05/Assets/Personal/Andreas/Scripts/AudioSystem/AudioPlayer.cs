using System;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.Audio;
using Util;

namespace AudioSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        [NonSerialized] public bool IsMusic;
        private AudioSource _source;
        private AudioMixerGroup _mixerGroup;

        private void Awake()
        {
            _mixerGroup = FastResources.Load<AudioMixerGroup>("Sound/AudioMixer");
            _source = GetComponent<AudioSource>();

            if(IsMusic)
            {
                _mixerGroup = _mixerGroup.audioMixer.FindMatchingGroups("Music")[0];
                _source.outputAudioMixerGroup = _mixerGroup;
                _mixerGroup.audioMixer.GetFloat("Music", out float value);
                // AudioManager.MusicVolume = value;
            }
            else
            {
                _mixerGroup = _mixerGroup.audioMixer.FindMatchingGroups("Sfx")[0];
                _source.outputAudioMixerGroup = _mixerGroup;
                _mixerGroup.audioMixer.GetFloat("Sfx", out float value);
                // AudioManager.SfxVolume = value;
            }
        }

        public void SetAudioClip(string clipName)
        {
            _source.clip = AudioManager.GetSoundClip(clipName);
        }

        public AudioSource GetSource()
        {
            gameObject.SetActive(true);
            return _source;
        }

        public void ReturnPlayer()
        {
            gameObject.SetActive(false);
            AudioManager.ReturnAudioPlayer(this);
        }

        private void Update()
        {
            if(!_source.isPlaying)
            {
                ReturnPlayer();
            }
        }

        public void UpdateVolume()
        {
            _source.volume = IsMusic ? AudioManager.MusicVolume : AudioManager.SfxVolume;
        }
    }
}