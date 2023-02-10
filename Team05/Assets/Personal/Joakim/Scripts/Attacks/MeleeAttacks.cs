using System;
using System.Collections;
using System.Collections.Generic;
using Andreas.Scripts;
using Andreas.Scripts.EnemyStates;
using UnityEngine;
using AttackNamespace;
using Personal.Andreas.Scripts.Actors;
using Unity.VisualScripting;

public class MeleeAttacks : MonoBehaviour, Attack.IAttack {
    [field: SerializeField] public float BasicDamage { get; set; } //todo: implement something to actually damage
    [field: SerializeField] public float SpecialDamage { get; set; } //todo: implement special attack possibility

    [field: SerializeField]
    public float ProjectileSpeed { get; set; } //todo: Only available under specific powerup circumstances

    [field: SerializeField] public float AttackSize { get; set; }

    public void DoDamage(float value) {
        //todo: call this on collision with enemy
    }


    public enum MeleeAttackType {
        Basic,
        Special,
        MeleeDome,
        ShieldSlam,
        ShieldDash
    }

    public MeleeAttackType meleeAttackType;
    
    public GameObject player;

    IEnumerator DestroyAttackArea() {
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }

    private void Awake() {
        player = GameObject.Find("MeleePlayer");
    }

    private void Start() {
        transform.localScale = new Vector3(AttackSize, AttackSize, AttackSize);
        switch (meleeAttackType) {
            case MeleeAttackType.Basic:
                StartCoroutine(DestroyAttackArea());
                break;
            case MeleeAttackType.MeleeDome:
                ShieldDome();
                break;
            case MeleeAttackType.ShieldSlam:
                ShieldSlam();
                break;
            case MeleeAttackType.ShieldDash:
                ShieldDash();
                break;
        }
    }
    
    public void ShieldSlam() {
        StartCoroutine(ShieldSlamDuration());
    }

    public void ShieldDome() {
        StartCoroutine(ShieldDomeDuration());
    }

    public void ShieldDash() {
        StartCoroutine(DashHitBoxDuration());
    }

    IEnumerator DashHitBoxDuration() {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    IEnumerator ShieldDomeDuration() {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    IEnumerator ShieldSlamDuration() {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null && meleeAttackType != MeleeAttackType.MeleeDome) {
            //enemy.Die();
            //Destroy(enemy.gameObject);
        }

        if (enemy != null && meleeAttackType == MeleeAttackType.ShieldDash) {
            var dir = (enemy.Body.position - other.transform.position).normalized;
            enemy.StateManager.SetState(new EnemyStateKnock(dir, 10f));
            Debug.Log("Stunned");
        }
    }
}