using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using AttackNamespace;
using Health;

public class Player : MonoBehaviour, Attack.IPlayerAttacker, HealthSystem.IDamagable {
    public Vector2 _moveDirection;
    private Vector2 _lookDirection;
    private Rigidbody _rb;
    public float moveSpeed = 20;
    [CanBeNull] public PlayerAttackScheme playerAttackScheme;
    public GameObject characterTypeHolder;

    public float Health { get; set; }
    public float Energy { get; set; }

    private bool switchedToCharacterMode = true;
    [field: SerializeField] public float AttackSpeed { get; set; }

    public enum CharacterType {
        Ranged,
        Melee
    }

    public CharacterType cType;

    private void Awake() {
        GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        _rb = GetComponent<Rigidbody>();
        switch (cType) {
            case CharacterType.Ranged:
                characterTypeHolder.GetComponent<PlayerAttackScheme>().characterType =
                    PlayerAttackScheme.Character.Ranged;
                playerAttackScheme = characterTypeHolder.GetComponent<PlayerAttackScheme>();
                if (playerAttackScheme != null) playerAttackScheme.characterType = PlayerAttackScheme.Character.Ranged;
                break;
            case CharacterType.Melee:
                characterTypeHolder.GetComponent<PlayerAttackScheme>().characterType =
                    PlayerAttackScheme.Character.Melee;
                playerAttackScheme = characterTypeHolder.GetComponent<PlayerAttackScheme>();
                if (playerAttackScheme != null) playerAttackScheme.characterType = PlayerAttackScheme.Character.Melee;
                break;
            default:
                Debug.Log("default");
                break;
        }
    }

    private void Update() {
        if (CharacterManager.Instance.CheckIfAllLockedIn() && switchedToCharacterMode) {
            GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            switchedToCharacterMode = false;
        }
    }

    private void FixedUpdate() {
        Debug.Log(_moveDirection);
        _rb.velocity = new Vector3(_moveDirection.x * moveSpeed, 0, _moveDirection.y * moveSpeed);
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

        if (context.canceled) {
            CancelInvoke(nameof(HoldBasic));
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        _moveDirection = context.ReadValue<Vector2>();
        Debug.Log(_moveDirection);
    }

    public void OnLook(InputAction.CallbackContext context) {
        if (context.performed)
            _lookDirection = context.ReadValue<Vector2>();
    }


}