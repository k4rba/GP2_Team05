using System;
using System.Collections;
using Andreas.Scripts.CheckpointSystem;
using AudioSystem;
using Personal.Andreas.Scripts;
using Personal.Andreas.Scripts.Actors;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;
using DG.Tweening;

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

        public GameObject gameOverScreen, pauseScreen;

        public GameObject healthBar;

        public bool paused;

        [Header(
            "This is the place the players jump to in the ending.")]
        public GameObject endJumpTarget;

        public GameObject credits;


        public void TogglePause() {
            
            if(!CharacterManager.IsAllLockedIn())
                return;
            
            paused = !paused;
            if (paused) {
                pauseScreen.SetActive(true);
                foreach (var player in CharacterManager.Players) {
                    player.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
                }

                Time.timeScale = 0;
            }
            else {
                Time.timeScale = 1;
                foreach (var player in CharacterManager.Players) {
                    player.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
                }

                pauseScreen.SetActive(false);
            }
        }

        private void Awake() {
            InputSystem.DisableDevice(Keyboard.current);
            InputSystem.DisableDevice(Mouse.current);

            if (Instance == null) {
                Instance = this;
            //    DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
                return;
            }
        }

        private void Start() {
            // var msuic = new GameObject("Music");
            // var source = msuic.AddComponent<AudioSource>();
            // source.clip = AudioManager.GetSoundClip("main_game_music");
            // source.loop = true;
            // source.volume = 0.6f;
            // source.Play();
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
                if (input != null && input.enabled) {
                    input.SwitchCurrentActionMap("UI");
                    if (input.currentActionMap != null)
                        input.currentActionMap.Enable();
                }

                player.GetComponent<CharacterSelection>().gameOver = true;
                player.transform.rotation = Quaternion.Euler(0, 90, 0);
            }

            gameOverScreen.SetActive(true);
        }

        public void Ending() {
            foreach (var player in CharacterManager.Players) {
                player.gameObject.GetComponent<PlayerInput>().actions.Disable();
                player.gameObject.transform.DOJump(endJumpTarget.transform.position, 12, 1, 4, false);
            }
            Invoke(nameof(ShowCredits), 4);
        }

        private void ShowCredits() {
            credits.SetActive(true);
            PlayerHudUi.gameObject.SetActive(false);
            healthBar.SetActive(false);
        }

        private void Update()
        {
            // if(Input.GetKeyDown(KeyCode.R))
            // {
                // AudioManager.PlaySfx("bronk_breakable");
            // }
        }
    }
    
}