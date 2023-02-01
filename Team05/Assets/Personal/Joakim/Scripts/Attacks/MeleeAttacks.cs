using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackNamespace;

public class MeleeAttacks : MonoBehaviour, Attack.IAttack {
    [field: SerializeField] public float BasicDamage { get; set; } //todo: implement something to actually damage
    [field: SerializeField] public float SpecialDamage { get; set; } //todo: implement special attack possibility
    [field: SerializeField] public float ProjectileSpeed { get; set; } //todo: Only available under specific powerup circumstances
    [field: SerializeField] public float AttackSize { get; set; }
    public void DoDamage(float value) {
        //todo: call this on collision with enemy
    }

    public enum MeleeAttackType {
        Basic,
        Special
    }

    IEnumerator DestroyAttackArea() {
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }

    public MeleeAttackType meleeAttackType;

    private void Start() {
        transform.localScale = new Vector3(AttackSize, AttackSize, AttackSize);
        switch (meleeAttackType) {
            case MeleeAttackType.Basic:
                StartCoroutine(DestroyAttackArea());
                break;
        }
    }
}