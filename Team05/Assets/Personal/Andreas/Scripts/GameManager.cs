using System;
using Andreas.Scripts.CheckpointSystem;
using AudioSystem;
using Personal.Andreas.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Andreas.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public EnemyManager EnemyManager;
        public CharacterManager CharacterManager;
        public PlayerJoinManager PlayerJoinManager;

        public CameraTopDownController CameraController;
        public CheckpointManager CheckpointManager;

        // public WorldManager WorldManager;
        public RopeManager RopeManager;
        public DollyCamManager DollyManager;

        private void Awake()
        {
            // InputSystem.DisableDevice(Keyboard.current);

            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            var msuic = new GameObject("Music");
            var source = msuic.AddComponent<AudioSource>();
            source.clip = AudioManager.GetSoundClip("skalar_banan_men_bananen_blev_till_kiseloxid");
            source.loop = true;
            source.volume = 0.25f;
            source.Play();
        }

    }
}