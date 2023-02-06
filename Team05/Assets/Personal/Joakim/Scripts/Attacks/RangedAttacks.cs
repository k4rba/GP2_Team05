using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using AttackNamespace;
using Personal.Andreas.Scripts.Actors;
using Unity.VisualScripting;
using Util;
using DG.Tweening;

public class RangedAttacks : MonoBehaviour, Attack.IAttack {
    private Rigidbody _rb;

    [field: SerializeField] public float BasicDamage { get; set; } //todo: implement something to actually damage
    [field: SerializeField] public float SpecialDamage { get; set; } //todo: implement special attack possibility
    [field: SerializeField] public float ProjectileSpeed { get; set; }
    [field: SerializeField] public float AttackSize { get; set; }
    public GameObject player;
    private Vector3 playerPos;
    public float distToPlayer;
    private bool moveBackToPlayer;

    public void DoDamage(float value) {
        //todo: call this on collision with enemy
    }

    public enum RangedAttackType {
        BasicAttack,
        Special
    }

    public RangedAttackType rangedAttackType;

    private void Awake() {
        player = GameObject.Find("RangedPlayer");
        _rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        transform.localScale = new Vector3(AttackSize, AttackSize, AttackSize);
        switch (rangedAttackType) {
            case RangedAttackType.BasicAttack:
                BasicAttack();
                break;
        }
    }

    private void Update() {
        if (moveBackToPlayer) {
            Debug.Log("gotoplayer");
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 15 * Time.deltaTime);
        }
    }

    private void FixedUpdate() {
        distToPlayer = Vector3.Distance(player.transform.position, transform.position);
    }

    public void BasicAttack() {
        StartCoroutine(BasicAttackRetract());
    }

    IEnumerator BasicAttackRetract() {
        var forward = transform.forward;
        _rb.AddForce(forward * ProjectileSpeed, ForceMode.Impulse);
        yield return new WaitUntil(() => distToPlayer >= 10);
        moveBackToPlayer = true;
        _rb.AddForce(forward * ProjectileSpeed * -1, ForceMode.Impulse);
        yield return new WaitUntil(() => distToPlayer <= 1.5f);
        player.GetComponent<PlayerAttackScheme>().ActiveProjectiles.Remove(gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.Die();
            Destroy(enemy.gameObject);
        }
    }
}