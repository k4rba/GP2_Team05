using System;
using System.Collections.Generic;
using Andreas.Scripts.HealingZone;
using AudioSystem;
using Health;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using DG.Tweening;
using Util;

namespace Joakim.Scripts.Mechanics {
    public class PowerCoil : MonoBehaviour {
        private HealthSystem.IDamagable affectedPlayer;

        public int requiredTicsToExecuteEvent;
        private int _currentTic = 0;
        public GameObject objectToDisableUponExecute;
        private GameObject _lightningTrailEffect;
        private Transform _affectedPlayerPos;
        public float damagePerTic;
        public HealingZone coupledHealingStation;
        public GameObject coupledFuseBox;
        private LineRenderer _lr;
        private int _currentLineIndex;
        public bool done;

        [Header(
            "Check this only if more than one coil needs to be charged to progress. Do the same for other coils connected to this event.")]
        public bool multipleCoils;

        private Vector3 _lineStartPos, _lineEndPos;

        private void Awake() {
            _lightningTrailEffect = FastResources.Load<GameObject>("TeslaLightningTrail");
            var fuseBoxPos = coupledFuseBox.transform.position;
            _lineStartPos = transform.position;
            _lineEndPos = fuseBoxPos;
        }

        private void Start() {
            _lr = GetComponentInChildren<LineRenderer>();
            _lr.transform.position = Vector3.zero;
            _lr.SetPosition(0, _lineStartPos); //endpos
            _lr.SetPosition(1, _lineStartPos); //origin
        }

        private void OnTriggerEnter(Collider other) {
            if (affectedPlayer == null && !done) {
                affectedPlayer = other.GetComponent<HealthSystem.IDamagable>();
                _affectedPlayerPos = other.transform;
                InvokeRepeating(nameof(OfferHealth), 0, 2);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.GetComponent<HealthSystem.IDamagable>() == affectedPlayer) {
                if (!done) {
                    _lr.SetPosition(0, _lineStartPos);
                }

                _currentLineIndex = 0;
                _currentTic = 0;
                affectedPlayer = null;
                CancelInvoke(nameof(OfferHealth));
            }
        }

        private void OfferHealth() {
            if (_currentTic != requiredTicsToExecuteEvent && coupledHealingStation.IsOccupied) {
                _currentLineIndex++;
                AudioManager.PlaySfx("LighningCoilZap");
                var distance = Vector3.Distance(_lineStartPos, _lineEndPos);
                var dir = (_lineEndPos - _lineStartPos).normalized;
                var step = distance / requiredTicsToExecuteEvent;
                var totalSteps = step * _currentLineIndex;
                var position = _lineStartPos + dir * totalSteps;
                _lr.SetPosition(0, position);
                var lightningEffect = Instantiate(_lightningTrailEffect, transform.position, Quaternion.identity);
                lightningEffect.transform.DOMove(_affectedPlayerPos.position, 0.25f)
                    .OnComplete(() => Destroy(lightningEffect));
                affectedPlayer?.Health.InstantDamage(affectedPlayer, damagePerTic);
                _currentTic++;
            }
            else if (_currentTic == requiredTicsToExecuteEvent) {
                done = !done;
                ExecuteEvent();
                CancelInvoke(nameof(OfferHealth));
            }
        }

        public void ExecuteEvent() {
            if (!multipleCoils) {
                objectToDisableUponExecute.SetActive(false);
            }
            else {
                coupledFuseBox.GetComponent<FuseBoxMultipleCoilsOnly>().CheckIfFinished();
            }
        }
    }
}