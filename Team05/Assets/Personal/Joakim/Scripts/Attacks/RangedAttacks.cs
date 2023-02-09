using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Andreas.Scripts.EnemyStates;
using UnityEngine;
using AttackNamespace;
using Personal.Andreas.Scripts.Actors;
using Unity.VisualScripting;
using Util;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine.ProBuilder.MeshOperations;

public class RangedAttacks : MonoBehaviour, Attack.IAttack {
    private Rigidbody _rb;

    [field: SerializeField] public float BasicDamage { get; set; } //todo: implement something to actually damage
    [field: SerializeField] public float SpecialDamage { get; set; } //todo: implement special attack possibility
    [field: SerializeField] public float ProjectileSpeed { get; set; }
    [field: SerializeField] public float AttackSize { get; set; }
    public GameObject player;
    private Vector3 _playerPos;
    public float distToPlayer;
    private bool _moveBackToPlayer;
    private bool _rotateAroundPlayer;
    public List<Transform> stunBallNearbyEnemies = new List<Transform>();
    private GameObject _stunBallHitFX;
    private bool _triggered;
    public int numberOfBounces = 6;

    public void DoDamage(float value) {
        //todo: call this on collision with enemy
    }

    public enum RangedAttackType {
        BasicAttack,
        TetherBlast,
        TetherFlail,
        StunBall
    }

    public RangedAttackType rangedAttackType;

    private void Awake() {
        player = GameObject.Find("RangedPlayer");
        _rb = GetComponent<Rigidbody>();
        _stunBallHitFX = Resources.Load<GameObject>("StunBallHitFX");
    }

    private void Start() {
        transform.localScale = new Vector3(AttackSize, AttackSize, AttackSize);
        switch (rangedAttackType) {
            case RangedAttackType.BasicAttack:
                BasicAttack();
                break;
            case RangedAttackType.TetherBlast:
                TetherBlast();
                break;
            case RangedAttackType.TetherFlail:
                TetherFlail();
                break;
            case RangedAttackType.StunBall:
                StunBall();
                _rb.AddForce(transform.forward * ProjectileSpeed, ForceMode.Impulse);
                break;
        }
    }

    private void Update() {
        if (_moveBackToPlayer) {
            transform.position =
                Vector3.MoveTowards(transform.position, player.transform.position, 15 * Time.deltaTime);
        }

        if (_rotateAroundPlayer) {
            _rb.transform.RotateAround(player.transform.position, Vector3.up, 720 * Time.deltaTime);
        }
    }

    private void FixedUpdate() {
        distToPlayer = Vector3.Distance(player.transform.position, transform.position);
    }

    public void BasicAttack() {
        StartCoroutine(BasicAttackRetract());
    }

    public void TetherBlast() {
        StartCoroutine(TetherBlasted());
    }

    public void TetherFlail() {
        StartCoroutine(TetherFlailRotateAround());
    }

    public void StunBall() {
        //  StartCoroutine(StunBallBounce());
    }

    IEnumerator TetherBlasted() {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    IEnumerator TetherFlailRotateAround() {
        _rotateAroundPlayer = !_rotateAroundPlayer;
        yield return new WaitForSeconds(5);
        _rotateAroundPlayer = !_rotateAroundPlayer;
        player.GetComponent<PlayerAttackScheme>().ActiveProjectiles.Remove(gameObject);
        Destroy(gameObject);
    }


    IEnumerator BasicAttackRetract() {
        var forward = transform.forward;
        _rb.AddForce(forward * ProjectileSpeed, ForceMode.Impulse);
        yield return new WaitUntil(() => distToPlayer >= 10);
        _moveBackToPlayer = true;
        _rb.AddForce(forward * ProjectileSpeed * -1, ForceMode.Impulse);
        yield return new WaitUntil(() => distToPlayer <= 1.5f);
        player.GetComponent<PlayerAttackScheme>().ActiveProjectiles.Remove(gameObject);
        Destroy(gameObject);
    }

    IEnumerator StunBallBounce() {
        for (int i = 0; i < stunBallNearbyEnemies.Count; i++) {
            numberOfBounces++;
            Instantiate(_stunBallHitFX, stunBallNearbyEnemies[i].position, Quaternion.identity);
            stunBallNearbyEnemies[i].gameObject.GetComponent<Enemy>().StateManager.SetState(new EnemyStateStunned(1));
            Debug.Log("Swag: " + i);
            if (i + 1 >= stunBallNearbyEnemies.Count || numberOfBounces == 6) {
                Destroy(gameObject);
                StopCoroutine(StunBallBounce());
            }
            else {
                transform.DOMove(stunBallNearbyEnemies[i + 1].position, 0.5f);
            }

            if (i+1 >= stunBallNearbyEnemies.Count) {
                StopCoroutine(StunBallBounce());
                yield return null;
            }
            else {
                yield return new WaitUntil(() =>
                    Vector3.Distance(transform.position, stunBallNearbyEnemies[i + 1].position) < 1.2f);
            }
        }
    }

    private void CheckForNearbyEnemies() {
        var distance = 10;
        var enemyList = FindObjectsOfType(typeof(Enemy))
            .Select(enemy => enemy.GetComponent<Enemy>())
            .Where(t => Vector3.Distance(t.transform.position, transform.position) < distance)
            .ToList();
        foreach (var enemy in enemyList) {
            if (enemy.transform != null) {
                stunBallNearbyEnemies.Add(enemy.transform);
            }
        }
        StartCoroutine(StunBallBounce());
    }

    private void OnTriggerEnter(Collider other) {
        if (rangedAttackType != RangedAttackType.StunBall) {
            var enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.Die();
                Destroy(enemy.gameObject);
            }
        }
        else if (rangedAttackType == RangedAttackType.StunBall && !_triggered) {
            Debug.Log("Entered Triggerino");
            _triggered = !_triggered;
            GetComponent<Collider>().enabled = false;
            CheckForNearbyEnemies();
        }
    }
}