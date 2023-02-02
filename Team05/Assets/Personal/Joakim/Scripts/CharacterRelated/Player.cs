using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using AttackNamespace;
using FlowFieldSystem;
using Health;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour, Attack.IPlayerAttacker, HealthSystem.IDamagable {
    public Vector2 _moveDirection;
    private Vector2 _lookDirection;
    private Rigidbody _rb;
    public float moveSpeed = 20;
    [CanBeNull] public PlayerAttackScheme playerAttackScheme;
    public GameObject characterTypeHolder;
    private float playerNumber;

    [field: SerializeField] public float Health { get; set; }
    [field: SerializeField] public float Energy { get; set; }

    private bool switchedToCharacterMode = true;
    [field: SerializeField] public float AttackSpeed { get; set; }

    public bool debugMode = false;
    private GameObject debugObj;

    public enum CharacterType {
        Ranged,
        Melee
    }

    public CharacterType cType;

#if UNITY_EDITOR
    private void ModeChanged ()
    {
        if (!EditorApplication.isPlayingOrWillChangePlaymode &&
            EditorApplication.isPlaying ) 
        {
            Debug.Log("Exiting playmode.");
            debugMode = false;
            
        }
    }
#endif
    
    private void Awake() {
#if UNITY_EDITOR
        EditorApplication.playmodeStateChanged += ModeChanged;
#endif
        
        playerNumber = PlayerJoinManager.Instance.playerNumber;
        name = "Player" + playerNumber;
        GameObject.Find("FlowFieldMap").GetComponent<FlowFieldManager>().SetUnit(transform);
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

    public void MoveTowardsBetterHalf(Vector3 pos) {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(pos.x, pos.y, pos.z), 2);
    }

    private void Update() {
        
        //DEBUG ONLY
        if (debugMode == true) {
            switch (cType) {
                case CharacterType.Melee: {
                    var objToFollow = GameObject.Find("RangedPlayer");
                    debugObj = objToFollow;
                    break;
                }
                case CharacterType.Ranged: {
                    var objToFollow = GameObject.Find("MeleePlayer");
                    debugObj = objToFollow;
                    break;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(debugObj.transform.position.x, debugObj.transform.position.y,
                    debugObj.transform.position.z), 2 * Time.deltaTime);

        }
        //END OF DEBUG
        
        
        if (CharacterManager.Instance.CheckIfAllLockedIn() && switchedToCharacterMode) {
            GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            switchedToCharacterMode = false;
        }
    }

    private void FixedUpdate() {
        // Debug.Log(_moveDirection);
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
        Debug.Log("movement: " + _moveDirection);
    }

    public void OnLook(InputAction.CallbackContext context) {
        if (context.performed)
            _lookDirection = context.ReadValue<Vector2>();
    }


}