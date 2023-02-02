using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAttackScheme : MonoBehaviour {
    public delegate void BasicAttacks();

    public readonly List<BasicAttacks> BasicAttacksList = new List<BasicAttacks>();
    
    private GameObject _basicAttack;

    public enum Character {
        Ranged,
        Melee
    }

    public Character characterType;

    public void BasicRangedAttack() {
        var playerTransform = transform;
        var basicAttack =
            Instantiate(_basicAttack,
                playerTransform.position + (playerTransform.forward * 2), playerTransform.rotation);
    }

    public void BasicMeleeAttack() {
        var playerTransform = transform;
        var basicAttack =
            Instantiate(_basicAttack,
                playerTransform.position + (playerTransform.forward * 2), playerTransform.rotation);
    }

    public void InitializeAttack() {
        switch (characterType) {
            case Character.Ranged:
                _basicAttack = Resources.Load<GameObject>("RangedBasicProjectile");
                BasicAttacksList.Add(BasicRangedAttack);
                break;
            case Character.Melee:
                _basicAttack = Resources.Load<GameObject>("MeleeHit");
                BasicAttacksList.Add(BasicMeleeAttack);
                break;
        }
    }
}