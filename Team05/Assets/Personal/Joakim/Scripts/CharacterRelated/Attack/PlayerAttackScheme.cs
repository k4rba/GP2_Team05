using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScheme : MonoBehaviour {
    public delegate void BasicAttacks();

    public readonly List<BasicAttacks> BasicAttacksList = new List<BasicAttacks>();
    private GameObject _basicAttack, _specialBAbility, _specialXAbility, _specialAAbility;
    private GameObject _specialXAbility2;
    public List<GameObject> ActiveProjectiles = new List<GameObject>();
    private bool StunChargedProjectile;
    public float DashAreaZ = 1;

    public enum Character {
        Ranged,
        Melee
    }

    public Character characterType;

    public void BasicRangedAttack() {
        var playerTransform = transform;
        if (ActiveProjectiles.Count == 0) {
            switch (StunChargedProjectile) {
                case false: {
                    var basicAttack =
                        Instantiate(_basicAttack,
                            playerTransform.position + (playerTransform.forward * 2), playerTransform.rotation);
                    ActiveProjectiles.Add(basicAttack);
                    break;
                }
                case true:
                    Instantiate(_specialAAbility,
                        playerTransform.position + (playerTransform.forward * 2), playerTransform.rotation);
                    StunChargedProjectile = !StunChargedProjectile;
                    break;
            }
        }
    }

    public void RangedAbilityB() {
        var playerTransform = transform;
        if (ActiveProjectiles.Count == 1) {
            var currentProjectilePosition = ActiveProjectiles[0].transform.position;
            var tetherBlast =
                Instantiate(_specialBAbility, currentProjectilePosition, Quaternion.identity);
        }
    }

    public void RangedAbilityX() {
        var playerTransform = transform;
        if (ActiveProjectiles.Count == 0) {
            var tetherFlail = Instantiate(_specialXAbility,
                playerTransform.position + (playerTransform.forward * 2), playerTransform.rotation);
            ActiveProjectiles.Add(tetherFlail);
        }
    }

    public void RangedAbilityA() {
        StunChargedProjectile = !StunChargedProjectile;
    }

    public void BasicMeleeAttack() {
        var playerTransform = transform;
        var basicAttack =
            Instantiate(_basicAttack,
                playerTransform.position + (playerTransform.forward * 2), playerTransform.rotation);
    }

    public void MeleeAbilityB() {
        var playerTransform = transform;
        var shieldDome = Instantiate(_specialBAbility,
            playerTransform.position, Quaternion.identity);
    }

    public void MeleeAbilityX() {
        var playertransform = transform;
        var rotation = playertransform.rotation;
        var dashHitBox = Instantiate(_specialXAbility2, playertransform.position, rotation, playertransform.transform);
    }
    
    public void MeleeAbilityA() {
        var playerTransform = transform;
        var shieldSlam = Instantiate(_specialAAbility,
            playerTransform.position  + (playerTransform.forward), playerTransform.rotation);
    }

    public void InitializeAttack() {
        switch (characterType) {
            case Character.Ranged:
                _basicAttack = Resources.Load<GameObject>("RangedBasicProjectile");
                _specialBAbility = Resources.Load<GameObject>("RangedSpecialB");
                _specialXAbility = Resources.Load<GameObject>("RangedSpecialX");
                _specialAAbility = Resources.Load<GameObject>("RangedSpecialA");
                BasicAttacksList.Add(BasicRangedAttack);
                BasicAttacksList.Add(RangedAbilityB);
                BasicAttacksList.Add(RangedAbilityX);
                BasicAttacksList.Add(RangedAbilityA);
                break;
            case Character.Melee:
                _basicAttack = Resources.Load<GameObject>("MeleeHit");
                _specialBAbility = Resources.Load<GameObject>("MeleeShieldDome");
                _specialXAbility = Resources.Load<GameObject>("DashArea");
                _specialXAbility2 = Resources.Load<GameObject>("DashHitBox");
                _specialAAbility = Resources.Load<GameObject>("MeleeShieldSlam");
                BasicAttacksList.Add(BasicMeleeAttack);
                BasicAttacksList.Add(MeleeAbilityB);
                BasicAttacksList.Add(MeleeAbilityX);
                BasicAttacksList.Add(MeleeAbilityA);
                break;
        }
    }
}