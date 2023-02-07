using System;
using System.Collections;
using Andreas.Scripts;
using Andreas.Scripts.Flowfield;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using AttackNamespace;
using FlowFieldSystem;
using Health;
using Personal.Andreas.Scripts;
using Personal.Andreas.Scripts.Actors;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour, Attack.IPlayerAttacker, HealthSystem.IDamagable {
    public Vector2 moveDirection;
    private Vector2 _lookDirection;
    private Rigidbody _rb;
    public float moveSpeed = 20;
    [CanBeNull] public PlayerAttackScheme playerAttackScheme;
    public int _playerNumber;
    private bool _switchedToCharacterMode = true;
    public GameObject otherPlayer;

    [field: SerializeField] public Material HealthMaterial { get; set; }
    public HealthSystem Health { get; set; }
    [field: SerializeField] public int CurrentHealth { get; set; }
    [field: SerializeField] public float Energy { get; set; }
    [field: SerializeField] public float AttackSpeed { get; set; }

    [field: SerializeField] public float AbilityACooldown { get; set; }
    [field: SerializeField] public float AbilityXCooldown { get; set; }
    [field: SerializeField] public float AbilityBCooldown { get; set; }
    private bool _bOnCd, _aOnCd, _xOnCd;

    public enum CharacterType {
        Ranged,
        Melee
    }

    public CharacterType cType;

#if UNITY_EDITOR
    private void ModeChanged() {
        if (!EditorApplication.isPlayingOrWillChangePlaymode &&
            EditorApplication.isPlaying) {
            Debug.Log("Exiting playmode.");
        }
    }
#endif
    private void Awake() {
        Health = new HealthSystem();
        _playerNumber = PlayerJoinManager.Instance.playerNumber;
        gameObject.tag = _playerNumber == 1 ? "Player1" : "Player2";
        GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        _rb = GetComponent<Rigidbody>();


#if UNITY_EDITOR
        EditorApplication.playmodeStateChanged += ModeChanged;
#endif
    }

    private void Start() {
        var grounds = GameManager.Instance.WorldManager.Grounds;
        var obstacles = GameManager.Instance.WorldManager.Obstacles;
        var playerFlowFieldManager = GetComponentInChildren<FlowFieldManager>();
        playerFlowFieldManager.SetupFromPlayer(grounds, obstacles, transform);
    }

    public void AssignPlayerToRole(Player.CharacterType type) {
        playerAttackScheme = GetComponent<PlayerAttackScheme>();
        switch (type) {
            case CharacterType.Ranged:
                name = "RangedPlayer";
                AbilityBCooldown = 8;
                AbilityXCooldown = 15;
                AbilityACooldown = 10;
                if (playerAttackScheme != null) {
                    playerAttackScheme.characterType = PlayerAttackScheme.Character.Ranged;
                }

                break;
            case CharacterType.Melee:
                name = "MeleePlayer)";
                if (playerAttackScheme != null) {
                    playerAttackScheme.characterType = PlayerAttackScheme.Character.Melee;
                }

                break;
        }
    }

    private void FixedUpdate() {
        _rb.velocity = new Vector3(moveDirection.x * moveSpeed, 0, moveDirection.y * moveSpeed);
        var look = new Vector3(_lookDirection.x, 0, _lookDirection.y);
        if (_lookDirection.x != 0 && _lookDirection.y != 0) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(look), 0.15f);
        }
    }

    private void HoldBasic() {
        if (playerAttackScheme != null) playerAttackScheme.BasicAttacksList[0]();
    }

    public void OnBasicAttack(InputAction.CallbackContext context) {
        if (context.performed) {
            InvokeRepeating(nameof(HoldBasic), 0, AttackSpeed);
        }
        else if (context.canceled) {
            CancelInvoke(nameof(HoldBasic));
        }
    }

    public void OnAbilityB(InputAction.CallbackContext context) {
        if (context.performed && !_bOnCd) {
            if (playerAttackScheme != null) {
                playerAttackScheme.BasicAttacksList[1]();
                _bOnCd = !_bOnCd;
                StartCoroutine(StartAbilityBCooldown());
            }
        }
    }

    IEnumerator StartAbilityBCooldown() {
        yield return new WaitForSeconds(AbilityBCooldown);
        _bOnCd = !_bOnCd;
    }

    public void OnAbilityX(InputAction.CallbackContext context) {
        if (context.performed && !_xOnCd) {
            if (playerAttackScheme != null) {
                playerAttackScheme.BasicAttacksList[2]();
                _xOnCd = !_xOnCd;
                StartCoroutine(StartAbilityXCooldown());
            }

        }
    }

    IEnumerator StartAbilityXCooldown() {

        yield return new WaitForSeconds(AbilityXCooldown);
        _xOnCd = !_xOnCd;
    }

    public void OnAbilityA(InputAction.CallbackContext context) {
        if (context.performed && !_aOnCd) {
            if (playerAttackScheme != null) {
                playerAttackScheme.BasicAttacksList[3]();
                _aOnCd = !_aOnCd;
                StartCoroutine(StartAbilityACooldown());
            }
        }
    }

    IEnumerator StartAbilityACooldown() {
        yield return new WaitForSeconds(AbilityACooldown);
        _aOnCd = !_aOnCd;
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context) {
        if (context.performed)
            _lookDirection = context.ReadValue<Vector2>();
    }

    public void OnGiveHealth(InputAction.CallbackContext context) {
        if (context.performed) {
            Health.TransferHealth(this, otherPlayer.GetComponent<Player>());
        }
    }
}