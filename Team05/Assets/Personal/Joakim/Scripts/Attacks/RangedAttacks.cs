using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackNamespace;
using Personal.Andreas.Scripts.Actors;
using Unity.VisualScripting;

public class RangedAttacks : MonoBehaviour, Attack.IAttack {
    private Rigidbody _rb;

    [field: SerializeField] public float BasicDamage { get; set; } //todo: implement something to actually damage
    [field: SerializeField] public float SpecialDamage { get; set; } //todo: implement special attack possibility
    [field: SerializeField] public float ProjectileSpeed { get; set; }
    [field: SerializeField] public float AttackSize { get; set; }
    public void DoDamage(float value) {
        //todo: call this on collision with enemy
    }

    public enum RangedAttackType {
        BasicAttack,
        Special
    }

    public RangedAttackType rangedAttackType;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        transform.localScale = new Vector3(AttackSize, AttackSize, AttackSize);
        switch (rangedAttackType) {
            case RangedAttackType.BasicAttack:
                _rb.AddForce(transform.forward * ProjectileSpeed, ForceMode.Impulse);
                break;
        }
    }

    private void OnTriggerEnter(Collider other) {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.Die();
            Destroy(enemy.gameObject);
        }
    }
}