using System;
using System.Collections;
using System.Collections.Generic;
using Andreas.Scripts;
using Andreas.Scripts.EnemyStates;
using UnityEngine;
using AttackNamespace;
using AudioSystem;
using Personal.Andreas.Scripts.Actors;
using Unity.VisualScripting;
using UnityEngine.Serialization;

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
    }

    public MeleeAttackType meleeAttackType;

    private Player _player;
    public GameObject playerObj;

    IEnumerator DestroyAttackArea() {
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }

    private void Awake() {
        playerObj = GameObject.Find("MeleePlayer");
        _player = GetComponent<Player>();
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
        }
    }
    
    public void ShieldSlam() {
        StartCoroutine(ShieldSlamDuration());
    }

    public void ShieldDome() {
        StartCoroutine(ShieldDomeDuration());
    }

    IEnumerator ShieldDomeDuration() {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    IEnumerator ShieldSlamDuration() {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        //  ignore skill if low dmg
        if(BasicDamage < 1)
            return;
        
        var enemy = other.gameObject.GetComponent<Enemy>();

        if(enemy == null)
            return;

        if(meleeAttackType == MeleeAttackType.Basic)
        {
            var sfxHit = _player.SfxData.AttackHit;
            AudioManager.PlaySfx(sfxHit.name, _player.transform.position);
        }
        
        if (meleeAttackType == MeleeAttackType.ShieldSlam) {
            enemy.StateManager.SetState(new EnemyStateStunned(2f));
        }

        int finalDamage = (int)Mathf.Min(1, BasicDamage);
        enemy.TakeDamage(finalDamage);
        enemy._animator.SetTrigger("GetHit");
    }
}