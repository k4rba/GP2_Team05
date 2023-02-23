using System.Collections.Generic;
using Andreas.Scripts.PlayerData;
using AudioSystem;
using UnityEngine;

public class PlayerAttackScheme : MonoBehaviour {
    public delegate void BasicAttacks();

    public readonly List<BasicAttacks> BasicAttacksList = new List<BasicAttacks>();
    private GameObject _basicAttack, _specialBAbility, _specialXAbility, _specialAAbility;
    public GameObject _rangedBAbilityFX;
    private GameObject _specialXAbility2;
    public List<GameObject> ActiveProjectiles = new List<GameObject>();

    public enum Character {
        Ranged,
        Melee
    }

    public Character characterType;
    public PlayerSfxData SfxData;

    public void BasicRangedAttack() {
        var playerTransform = transform;
        if (ActiveProjectiles.Count == 0) {
            var basicAttack =
                Instantiate(_basicAttack,
                    playerTransform.position + (playerTransform.forward * 2), playerTransform.rotation);
            ActiveProjectiles.Add(basicAttack);
            AudioManager.PlaySfx(SfxData.BasicAttack.name, playerTransform.position);
            // AudioManager.PlaySfx("attack_basic_attack_ranged", playerTransform.position);
        }
    }

    //  TETHER BLAST
    public void RangedAbilityB() {
        var playerTransform = transform;
        if (ActiveProjectiles.Count <= 1) {
            var tetherAttack =
            Instantiate(_specialBAbility,
                playerTransform.position + (playerTransform.forward * 2), playerTransform.rotation);
            AudioManager.PlaySfx(SfxData.BasicAttack.name, playerTransform.position);
            ActiveProjectiles.Add(tetherAttack);
        }
    }

    //  STUN SHOT
    public void RangedAbilityA() {
        var playerTransform = GetComponent<Player>().feetPos.transform;
        Instantiate(_specialAAbility,
            playerTransform.position + (playerTransform.forward * 1) + Vector3.up * 0.3f,  playerTransform.rotation);
            AudioManager.PlaySfx(SfxData.BasicAttack.name, playerTransform.position);
    }

    
    //  BRANK BASIC
    public void BasicMeleeAttack() {
        var playerTransform = transform;
        var basicAttack =
            Instantiate(_basicAttack,
                playerTransform.position + (playerTransform.forward * 1), playerTransform.rotation);
        AudioManager.PlaySfx(SfxData.BasicAttack.name, playerTransform.position);
    }

    //  SHIELD DOME
    public void MeleeAbilityB() {
        var playerTransform = transform;
        var shieldDome = Instantiate(_specialBAbility,
            new Vector3(transform.position.x, transform.position.y - 1.53f, transform.position.z), Quaternion.identity);
        AudioManager.PlaySfx(SfxData.SpecialAttack.name, playerTransform.position);
    }

    //  SHIELD SLAM
    public void MeleeAbilityA() {
        var playerTransform = transform;
        var shieldSlam = Instantiate(_specialAAbility,
            playerTransform.position + (playerTransform.forward), playerTransform.rotation);
        AudioManager.PlaySfx(SfxData.SecondaryAttack.name, playerTransform.position);
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