using System;
using Andreas.Scripts.HealingZone;
using Health;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using DG.Tweening;
using Util;

namespace Joakim.Scripts.Mechanics {
    public class PowerCoil : MonoBehaviour {
        private HealthSystem.IDamagable affectedPlayer;

        public int requiredTicsToExecuteEvent;
        private int _currentTic;
        public GameObject objectToDisableUponExecute;
        private GameObject _lightningTrailEffect;
        private Transform _affectedPlayerPos;
        public float damagePerTic;
        public HealingZone coupledHealingStation;

        private void Awake() {
            _lightningTrailEffect = FastResources.Load<GameObject>("TeslaLightningTrail");
        }

        private void OnTriggerEnter(Collider other) {
            _affectedPlayerPos = other.transform;
            affectedPlayer = other.GetComponent<HealthSystem.IDamagable>();
            Debug.Log(affectedPlayer);
            InvokeRepeating(nameof(OfferHealth), 0, 2);
        }

        private void OfferHealth() {
            if (_currentTic != requiredTicsToExecuteEvent) {
                var lightningEffect = Instantiate(_lightningTrailEffect, transform.position, Quaternion.identity);
                lightningEffect.transform.DOMove(_affectedPlayerPos.position, 0.25f).OnComplete(()=> Destroy(lightningEffect));
                affectedPlayer.Health.InstantDamage(affectedPlayer, damagePerTic);
                _currentTic++;
            }
            else {
                ExecuteEvent();
                CancelInvoke(nameof(OfferHealth));
            }
        }

        public void ExecuteEvent() {
            objectToDisableUponExecute.SetActive(false);
        }
    }
}
