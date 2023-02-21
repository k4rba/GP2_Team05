using System;
using AudioSystem;
using UnityEngine;
using UnityEngine.Rendering.UI;
using Util;

namespace Andreas.Scripts.HealingZone
{
    public class HealingZone : MonoBehaviour
    {
        public Player CurrentPlayer;
        public bool IsOccupied => _activated;
        
        [SerializeField] private HealingZoneData _data;

        public GameObject healingVFX;

        private Timer _tickTimer;
        private int _ticks;
        private bool _activated;

        private AudioSource _sfxIdle;

        private void Awake()
        {
            _tickTimer = _data.TickRate;
            _ticks = _data.MaxTicks;
        }

        private void Start()
        {
            _sfxIdle = _data.Idle.Play(transform.position);
            _sfxIdle.SetMaxDistance(50f);
            _sfxIdle.loop = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(CurrentPlayer != null)
                return;
            
            var player = other.gameObject.GetComponent<Player>();
            if(player != null)
            {
                healingVFX.SetActive(true);
                _sfxIdle.Stop();
                _data.Active.Play(transform.position);
                CurrentPlayer = player;
                _activated = true;
                Debug.Log("HEALING PAD ACTIVATED");
            }
            else
            {
                Debug.Log("entered healing pad (BUT WAS NOT PLAYER)");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var player = other.gameObject.GetComponent<Player>();
            if(player == CurrentPlayer)
            {
                healingVFX.SetActive(false);
                CurrentPlayer = null;
                _activated = false;
            }
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