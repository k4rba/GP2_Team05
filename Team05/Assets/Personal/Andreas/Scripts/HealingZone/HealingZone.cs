using UnityEngine;
using Util;

namespace Andreas.Scripts.HealingZone
{
    public class HealingZone : MonoBehaviour
    {
        public Player CurrentPlayer;
        public bool IsOccupied => CurrentPlayer != null;

        [SerializeField] private HealingZoneData _data;

        public GameObject healingVFX;

        private Timer _tickTimer;
        private int _ticks;
        private bool _activated;

        private AudioSource _sfxIdle;
        private AudioSource _sfxActive;

        public bool IsHanSolo;

        private void Awake()
        {
            _tickTimer = _data.TickRate;
            _ticks = _data.MaxTicks;
        }

        private void Start()
        {
            _sfxIdle = _data.Idle.Play(transform.position);
            _sfxIdle.loop = true;
            _sfxIdle.SetMaxDistance(50f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_ticks <= 0)
                return;

            if(CurrentPlayer != null)
                return;

            var player = other.gameObject.GetComponent<Player>();
            if(player == null)
                return;

            healingVFX.SetActive(true);
            _sfxActive = _data.Active.Play(transform.position);
            CurrentPlayer = player;
            _activated = true;
            
            if(!IsHanSolo)
            {
                player.SfxData.GoToPowerCoil.Play(player.transform.position);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var player = other.gameObject.GetComponent<Player>();
            if(player == CurrentPlayer)
            {
                CurrentPlayer = null;
                StopStuff();
            }
        }

        private void StopStuff()
        {
            healingVFX.SetActive(false);
            _sfxActive?.Stop();
            _activated = false;
        }

        private void Update()
        {
            if(!_activated)
                return;
            if(_ticks <= 0)
                return;

            if(_tickTimer.UpdateTick())
            {
                Heal();
                _ticks--;
                if(_ticks <= 0)
                {
                    Deplete();
                }
            }
        }

        private void Deplete()
        {
            StopStuff();
            Debug.Log("HEALING PAD DEPLETED");
        }

        private void Heal()
        {
            if(CurrentPlayer == null)
                return;

            CurrentPlayer.Health.InstantHealing(CurrentPlayer, _data.HealAmount);
        }
    }
}