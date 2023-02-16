using System.Collections.Generic;
using AudioSystem;
using UnityEngine;

public class PlayerAttackScheme : MonoBehaviour {
    public delegate void BasicAttacks();

    public readonly List<BasicAttacks> BasicAttacksList = new List<BasicAttacks>();
    private GameObject _basicAttack, _specialBAbility, _specialXAbility, _specialAAbility;
    private GameObject _rangedBAbilityFX;
    private GameObject _specialXAbility2;
    public List<GameObject> ActiveProjectiles = new List<GameObject>();

    public enum Character {
        Ranged,
        Melee
    }

    public Character characterType;

    public void BasicRangedAttack() {
        var playerTransform = transform;
        if (ActiveProjectiles.Count == 0) {
            var basicAttack =
                Instantiate(_basicAttack,
                    playerTransform.position + (playerTransform.forward * 2), playerTransform.rotation);
            ActiveProjectiles.Add(basicAttack);
            AudioManager.PlaySfx("attack_basic_attack_ranged", playerTransform.position);
        }
    }

    public void RangedAbilityB() {
        var playerTransform = transform;
        if (ActiveProjectiles.Count == 1) {
            var currentProjectilePosition = ActiveProjectiles[0].transform.position;
            var tetherBlast =
                Instantiate(_specialBAbility, currentProjectilePosition, Quaternion.identity);
            Instantiate(_rangedBAbilityFX, currentProjectilePosition, Quaternion.identity);
            AudioManager.PlaySfx("attack_tether_explosion", playerTransform.position);
        }
    }

    public void RangedAbilityA() {
        var playerTransform = transform;
        Instantiate(_specialAAbility,
            playerTransform.position + (playerTransform.forward * 1), playerTransform.rotation);
        AudioManager.PlaySfx("attack_stun_shot", playerTransform.position);
    }

    public void BasicMeleeAttack() {
        var playerTransform = transform;
        var basicAttack =
            Instantiate(_basicAttack,
                playerTransform.position + (playerTransform.forward * 1), playerTransform.rotation);
        AudioManager.PlaySfx("attack_basic_swosh", playerTransform.position);
        AudioManager.PlaySfx("attack_basic_attack_melee", playerTransform.position);
    }

    public void MeleeAbilityB() {
        var playerTransform = transform;
        var shieldDome = Instantiate(_specialBAbility,
            new Vector3(transform.position.x, transform.position.y - 1.53f, transform.position.z), Quaternion.identity);
        AudioManager.PlaySfx("attack_shield_dome", playerTransform.position);
    }

    public void MeleeAbilityA() {
        var playerTransform = transform;
        var shieldSlam = Instantiate(_specialAAbility,
            playerTransform.position + (playerTransform.forward), playerTransform.rotation);
        AudioManager.PlaySfx("attack_shield_slam", playerTransform.position);
    }

    public void InitializeAttack() {
        switch (characterType) {
            case Character.Ranged:
                _basicAttack = Resources.Load<GameObject>("RangedBasicProjectile");
                _specialBAbility = Resources.Load<GameObject>("RangedSpecialB");
                _specialAAbility = Resources.Load<GameObject>("RangedSpecialA");
                _rangedBAbilityFX = Resources.Load<GameObject>("BallExplodeFX");
                BasicAttacksList.Add(BasicRangedAttack);
                BasicAttacksList.Add(RangedAbilityB);
                BasicAttacksList.Add(RangedAbilityA);
                break;
            case Character.Melee:
                _basicAttack = Resources.Load<GameObject>("MeleeHit");
                _specialBAbility = Resources.Load<GameObject>("MeleeShieldDome");
                _specialAAbility = Resources.Load<GameObject>("MeleeShieldSlam");
                BasicAttacksList.Add(BasicMeleeAttack);
                BasicAttacksList.Add(MeleeAbilityB);
                BasicAttacksList.Add(MeleeAbilityA);
                break;
        }
    }
}