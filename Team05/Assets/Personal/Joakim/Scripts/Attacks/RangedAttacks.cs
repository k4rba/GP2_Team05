using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Andreas.Scripts.EnemyStates;
using UnityEngine;
using AttackNamespace;
using AudioSystem;
using Personal.Andreas.Scripts.Actors;
using Unity.VisualScripting;
using Util;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Serialization;

public class RangedAttacks : MonoBehaviour, Attack.IAttack {
    private Rigidbody _rb;

    [field: SerializeField] public float BasicDamage { get; set; } //todo: implement something to actually damage
    [field: SerializeField] public float SpecialDamage { get; set; } //todo: implement special attack possibility
    [field: SerializeField] public float ProjectileSpeed { get; set; }
    [field: SerializeField] public float AttackSize { get; set; }
    public GameObject playerObj;
    private Player _player;
    private Vector3 _playerPos;
    public float distToPlayer;
    private bool _moveBackToPlayer;
    private bool _rotateAroundPlayer;
    public List<Enemy> stunBallNearbyEnemies = new List<Enemy>();
    private GameObject _stunBallHitFX;
    public int numberOfBounces = 6;

    public void DoDamage(float value) {
        //todo: call this on collision with enemy
    }

    public enum RangedAttackType {
        BasicAttack,
        TetherBlast,
        StunBall
    }

    public RangedAttackType rangedAttackType;

    private void Awake() {
        playerObj = GameObject.Find("RangedPlayer");
        _player = playerObj.GetComponent<Player>();
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
        }
    }

    private void Update() {
        if (_moveBackToPlayer) {
            transform.position =
                Vector3.MoveTowards(transform.position, playerObj.transform.position, 15 * Time.deltaTime);
        }

        if (_rotateAroundPlayer) {
            _rb.transform.RotateAround(playerObj.transform.position, Vector3.up, 720 * Time.deltaTime);
        }
    }

    private void FixedUpdate() {
        distToPlayer = Vector3.Distance(playerObj.transform.position, transform.position);
    }

    public void BasicAttack() {
        StartCoroutine(BasicAttackRetract());
    }

    public void TetherBlast() {
        StartCoroutine(TetherBlasted());
    }

    IEnumerator TetherBlasted() {
        var forward = transform.forward;
        _rb.AddForce(forward * ProjectileSpeed, ForceMode.Impulse);
        yield return new WaitUntil(() => distToPlayer >= 10 || CheckIfBulletCollided());
        playerObj.GetComponent<PlayerAttackScheme>().ActiveProjectiles.Remove(gameObject);
        AudioManager.PlaySfx(_player.SfxData.SpecialAttack.name, _player.transform.position);
        Instantiate(playerObj.GetComponent<PlayerAttackScheme>()._rangedBAbilityFX, transform.position,
            Quaternion.identity);
        Destroy(gameObject);
    }


    IEnumerator BasicAttackRetract() {
        var forward = transform.forward;
        _rb.AddForce(forward * ProjectileSpeed, ForceMode.Impulse);
        yield return new WaitUntil(() => distToPlayer >= 10 || CheckIfBulletCollided());
        _moveBackToPlayer = true;
        _rb.AddForce(forward * ProjectileSpeed * -1, ForceMode.Impulse);
        yield return new WaitUntil(() => distToPlayer <= 1.5f);
        playerObj.GetComponent<PlayerAttackScheme>().ActiveProjectiles.Remove(gameObject);
        Destroy(gameObject);
    }


    bool CheckIfBulletCollided() {
        Physics.SphereCast(transform.position, 0.2f, playerObj.GetComponent<Player>().lookDirV3, out RaycastHit hit, 1);
        if (hit.collider != null) {
            return true;
        }

        return false;
    }


    private void CheckForNearbyEnemies() {
        var distance = 10;
        var enemyList = FindObjectsOfType(typeof(Enemy))
            .Select(enemy => enemy.GetComponent<Enemy>())
            .Where(t => Vector3.Distance(t.transform.position, transform.position) < distance)
            .ToList();
        foreach (var enemy in enemyList) {
            if (enemy.transform != null) {
                stunBallNearbyEnemies.Add(enemy);
            }
        }

        int targetCount = (int)MathF.Min(6, stunBallNearbyEnemies.Count);
        for (int i = 0; i < targetCount; i++) {
            Instantiate(_stunBallHitFX,
                new Vector3(stunBallNearbyEnemies[i].transform.position.x,
                    stunBallNearbyEnemies[i].transform.position.y + 2.5f,
                    stunBallNearbyEnemies[i].transform.position.z), Quaternion.identity);
            stunBallNearbyEnemies[i].StateManager.SetState(new EnemyStateStunned(2));

            var enemy = stunBallNearbyEnemies[i];
            int finalDamage = (int)Mathf.Min(1, BasicDamage);
            enemy._animator.SetTrigger("Stun");
            enemy.TakeDamage(finalDamage);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (BasicDamage < 1) {
            return;
        }

        var enemy = other.gameObject.GetComponent<Enemy>();

        if (enemy == null) {
            return;
        }


        if (rangedAttackType != RangedAttackType.StunBall) {
            if (enemy != null) {
                int finalDamage = (int)Mathf.Min(1, BasicDamage);
                enemy._animator.SetTrigger("GetHit");
                enemy.TakeDamage(finalDamage);
            }
        }
        else if (rangedAttackType == RangedAttackType.StunBall) {
            GetComponent<Collider>().enabled = false;
            CheckForNearbyEnemies();
            AudioManager.PlaySfx(_player.SfxData.SecondaryAttack.name, _player.transform.position);
        }
    }
}