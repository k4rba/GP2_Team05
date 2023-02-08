using UnityEngine;
using Util;

namespace Andreas.Scripts.HealingZone
{
    public class HealingZone : MonoBehaviour
    {
        [SerializeField] private HealingZoneData _data;

        private Timer _tickTimer;
        private int _ticks;
        private bool _activated;

        private void Awake()
        {
            _tickTimer = _data.TickRate;
            _ticks = _data.MaxTicks;
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponent<Player>();
            if(player != null)
            {
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
            var players = GameManager.Instance.CharacterManager.Players;

            for(int i = 0; i < players.Count; i++)
            {
                var p = players[i];
                p.Health.InstantHealing(p, _data.HealAmount);
            }
        }
    }
}