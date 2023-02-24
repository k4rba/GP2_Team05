using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    public static class AudioManager
    {
        private static Dictionary<string, AudioClip> AudioClips
        {
            get
            {
                if(_audioClips == null)
                {
                    Load();
                }

                return _audioClips;
            }
        }

        public static float SfxVolume = 1f;
        public static float MusicVolume = 1f;

        private static Dictionary<string, AudioClip> _audioClips;
        private static AudioPlayer _musicPlayer;
        private static GameObject _audioSourceContainer;
        private static Stack<AudioPlayer> _sources;
        private static List<AudioPlayer> _allPlayers;

        public static void Load()
        {
            _sources = new Stack<AudioPlayer>();
            _allPlayers = new List<AudioPlayer>();
            _audioClips = new Dictionary<string, AudioClip>();

            //  audio pool object
            _audioSourceContainer = new GameObject("AudioSourcePool");
            Object.DontDestroyOnLoad(_audioSourceContainer);

            var loadedAudioClips = Resources.LoadAll<AudioClip>("Sound/");

            const string musicPrefix = "music_";

            for(int i = 0; i < loadedAudioClips.Length; i++)
            {
                var clip = loadedAudioClips[i];
                var finalName = clip.name.StartsWith(musicPrefix)
                    ? clip.name.Remove(0, musicPrefix.Length)
                    : clip.name;

                _audioClips.Add(finalName, clip);
            }

            Debug.Log($"Loaded {_audioClips.Count} audio clips");

            // FileLoader.Get.MasterVolume.OnChangedEvent += OnAnyVolumeChangedEvent;
            // FileLoader.Get.SfxVolume.OnChangedEvent += OnAnyVolumeChangedEvent;
            // FileLoader.Get.MusicVolume.OnChangedEvent += OnAnyVolumeChangedEvent;
        }

        private static void OnAnyVolumeChangedEvent()
        {
            for(int i = 0; i < _allPlayers.Count; i++)
            {
                _allPlayers[i].UpdateVolume();
            }
        }

        internal static void ReturnAudioPlayer(AudioPlayer player)
        {
            player.GetSource().Stop();
            player.gameObject.SetActive(false);
            _sources.Push(player);
            Debug.Log($"returned ({_sources.Count})");
        }

        public static AudioClip GetSoundClip(string name)
        {
            if(AudioClips.TryGetValue(name, out var clip))
            {
                return clip;
            }

            return null;
        }

        /// <summary>
        /// Get (or create) a poolable AudioPlayer
        /// </summary>
        /// <returns></returns>
        private static AudioPlayer GetAudioPlayer()
        {
            AudioPlayer retSource;
            if(_sources.Count <= 0)
            {
                var go = new GameObject($"AudioPlayer{_allPlayers.Count}", typeof(AudioPlayer));
                go.transform.parent = _audioSourceContainer.transform;
                retSource = go.GetComponent<AudioPlayer>();
                _allPlayers.Add(retSource);
                // Debug.Log("created new source");
            }
            else
            {
                retSource = _sources.Pop();
                // Debug.Log($"popped source ({_sources.Count})");
            }

            retSource.GetSource().loop = false;
            return retSource;
        }

        private static AudioPlayer GetAudioPlayerSource(string name)
        {
            if(AudioClips.TryGetValue(name, out var clip))
            {
                var player = GetAudioPlayer();
                player.gameObject.name = name;
                player.IsMusic = false;
                var source = player.GetSource();
                source.clip = clip;
                source.loop = false;
                source.maxDistance = 100f;
                return player;
            }

            return null;
        }

        /// <summary>
        /// Plays a song from the Resources/Audio/Music/ folder 
        /// </summary>
        /// <param name="musicName">Name of music file on Resources/Audio/Music/</param>
        public static void PlayMusic(string musicName)
        {
            if(_musicPlayer == null)
            {
                _musicPlayer = GetAudioPlayerSource(musicName);
                _musicPlayer.IsMusic = true;
            }
            else
            {
                _musicPlayer.SetAudioClip(musicName);
            }

            var source = _musicPlayer.GetSource();
            source.volume = MusicVolume;
            source.loop = true;
            source.Play();
        }

        /// <summary>
        /// Plays a SFX from the Resources/Audio/Sfx/ folder 
        /// </summary>
        /// <param name="sfxName">Name of SFX file on Resources/Audio/SFX/</param>
        /// <returns></returns>
        public static AudioSource PlaySfx(string sfxName)
            => PlaySfx(sfxName, Vector3.zero);

        /// <summary>
        /// Plays a SFX from the Resources/Audio/Sfx/ folder 
        /// </summary>
        /// <param name="sfxName">Name of SFX file on Resources/Audio/SFX/</param>
        /// <param name="position">World location of the sound</param>
        /// <returns></returns>
        public static AudioSource PlaySfx(string sfxName, Vector3 position)
        {
            var player = GetAudioPlayerSource(sfxName);

            if(player == null)
            {
                Debug.LogWarning($"sfx '{sfxName}' does not exit");
                return null;
            }

            player.transform.position = position;
            var source = player.GetSource();
            source.volume = SfxVolume;
            source.Play();
            return source;
        }
    }
}