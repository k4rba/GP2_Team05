using System;
using System.Collections;
using Andreas.Scripts.CheckpointSystem;
using AudioSystem;
using Personal.Andreas.Scripts;
using Personal.Andreas.Scripts.Actors;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace Andreas.Scripts {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }

        public EnemyManager EnemyManager;
        public CharacterManager CharacterManager;
        public PlayerJoinManager PlayerJoinManager;

        public PlayerHudUI PlayerHudUi;

        public CameraTopDownController CameraController;
        public CheckpointManager CheckpointManager;

        // public WorldManager WorldManager;
        public RopeManager RopeManager;
        public DollyCamManager DollyManager;

        public GameObject gameOverScreen;

        private void Awake() {
            // InputSystem.DisableDevice(Keyboard.current);

            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
                return;
            }
        }

        private void Start() {
            var msuic = new GameObject("Music");
            var source = msuic.AddComponent<AudioSource>();
            source.clip = AudioManager.GetSoundClip("skalar_banan_men_bananen_blev_till_kiseloxid");
            source.loop = true;
            source.volume = 0.18f;
            source.Play();
        }

        public IEnumerator PlayEnemySfxCheer(Enemy enemy) {
            yield return new WaitForSeconds(Rng.NextF(1.5f));
            enemy.Data.Sfx.OnKill.Play(enemy.Position);
        }

        public void PlayersDead() {
            var enemies = EnemyManager.Enemies;
            int maxCheers = 3;
            for (int i = 0; i < enemies.Count; i++) {
                var e = enemies[i];
                if (e.EnteredCombat) {
                    StartCoroutine(PlayEnemySfxCheer(e));
                    maxCheers++;
                    if (maxCheers <= 0)
                        break;
                }
            }

            foreach (var player in CharacterManager.Players) {
                player.isDead = true;
                var input = player.GetComponent<PlayerInput>();
                if(input != null && input.enabled)
                {
                    input.SwitchCurrentActionMap("UI");
                    if(input.currentActionMap != null)
                        input.currentActionMap.Enable();
                }
                player.GetComponent<CharacterSelection>().gameOver = true;
                player.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            gameOverScreen.SetActive(true);
        }
    }
}