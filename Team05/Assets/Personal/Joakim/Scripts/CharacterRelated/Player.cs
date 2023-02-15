using System;
using System.Collections;
using System.Linq;
using Andreas.Scripts;
using Andreas.Scripts.Flowfield;
using Andreas.Scripts.StateMachine;
using Andreas.Scripts.StateMachine.States;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using AttackNamespace;
using AudioSystem;
using FlowFieldSystem;
using Health;
using DG.Tweening;
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

    private GameObject _model;

    [field: SerializeField] public Material HealthMaterial { get; set; }
    public HealthSystem Health { get; set; }
    [field: SerializeField] public int CurrentHealth { get; set; }
    [field: SerializeField] public float Energy { get; set; }
    [field: SerializeField] public float AttackSpeed { get; set; }

    [field: SerializeField] public float AbilityACooldown { get; set; }
    [field: SerializeField] public float AbilityXCooldown { get; set; }
    [field: SerializeField] public float AbilityBCooldown { get; set; }
    private bool _bOnCd, _aOnCd, _xOnCd;

    private bool _shieldDashHold;
    private float _shieldDashTime;
    private float shieldDashButtonActive;

    private GameObject dashArea;
    public event Action OnInteracted;

    public StatesManager StatesManager;
    
    private ProjectileReceiver _projectileReceiver;

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
    private void Awake()
    {
        StatesManager = new();
        dashArea = Resources.Load<GameObject>("DashArea");
        Health = new HealthSystem();
        Health.OnDamageTaken += HealthOnOnDamageTaken;
        _playerNumber = PlayerJoinManager.Instance.playerNumber;
        gameObject.tag = _playerNumber == 1 ? "Player1" : "Player2";
        GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        _rb = GetComponent<Rigidbody>();

        _projectileReceiver = GetComponent<ProjectileReceiver>();
        _projectileReceiver.OnHit += Projectile_OnHit;


#if UNITY_EDITOR
        EditorApplication.playmodeStateChanged += ModeChanged;
#endif
    }

    private void HealthOnOnDamageTaken()
    {
        StatesManager.AddState(new StateColorFlash(_model, Color.red));
    }

    private void Projectile_OnHit(Projectile proj) {
        Health.InstantDamage(this, proj.Damage);
    }

    private void Start() {
        // var grounds = GameManager.Instance.WorldManager.Grounds;
        // var obstacles = GameManager.Instance.WorldManager.Obstacles;
        // var playerFlowFieldManager = GetComponentInChildren<FlowFieldManager>();
        // playerFlowFieldManager.SetupFromPlayer(grounds, obstacles, transform);
    }

    public void AssignPlayerToRole(Player.CharacterType type) {
        playerAttackScheme = GetComponent<PlayerAttackScheme>();
        switch (type) {
            case CharacterType.Ranged:
                name = "RangedPlayer";
                AbilityBCooldown = 8;
                AbilityACooldown = 10;
                if (playerAttackScheme != null) {
                    playerAttackScheme.characterType = PlayerAttackScheme.Character.Ranged;
                }

                _model = transform.Find("Jose").gameObject; 
                _model.SetActive(true);
                break;
            case CharacterType.Melee:
                name = "MeleePlayer)";
                AbilityBCooldown = 15;
                AbilityACooldown = 5;
                if (playerAttackScheme != null) {
                    playerAttackScheme.characterType = PlayerAttackScheme.Character.Melee;
                }

                _model = transform.Find("Bronk").gameObject; 
                _model.SetActive(true);
                break;
        }
    }

    private void Update() {
        StatesManager.Update(Time.deltaTime);
        if (_shieldDashHold) {
            var dashBox = GameObject.Find("DashArea(Clone)");
            dashBox.transform.rotation = transform.rotation;
            dashBox.GetComponent<Rigidbody>().AddForce(transform.forward * (1.2f * Time.deltaTime), ForceMode.Impulse);
            _shieldDashTime += Time.deltaTime;
            if (_shieldDashTime >= 2.5f) {
                playerAttackScheme.BasicAttacksList[2]();
                Dash();
                _shieldDashHold = false;
            }
        }
        
        //  test
        
        if(Input.GetKeyDown(KeyCode.T))
        {
            Health.InstantDamage(this, 0.01f);
        }
        
        if(Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }

    public void Dash() {
        var dashBox = GameObject.Find("DashArea(Clone)");
        transform.DOMove(dashBox.transform.position, 0.5f).OnComplete(() => {
            var dashBox = GameObject.Find("DashArea(Clone)");
            Destroy(dashBox);
            _shieldDashTime = 0;
        });
        AudioManager.PlaySfx("attack_shield_dash", transform.position);
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


    public void OnAbilityA(InputAction.CallbackContext context) {
        if (context.performed && !_aOnCd) {
            if (playerAttackScheme != null) {
                playerAttackScheme.BasicAttacksList[1]();
                _aOnCd = !_aOnCd;
                StartCoroutine(StartAbilityACooldown());
            }
        }
    }

    IEnumerator StartAbilityACooldown() {
        yield return new WaitForSeconds(AbilityACooldown);
        _aOnCd = !_aOnCd;
    }

    public void OnAbilityB(InputAction.CallbackContext context) {
        if (context.performed && !_bOnCd) {
            if (playerAttackScheme != null) {
                playerAttackScheme.BasicAttacksList[2]();
                _bOnCd = !_bOnCd;
                StartCoroutine(StartAbilityBCooldown());
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if(context.performed)
        {
            OnInteracted?.Invoke();
        }
    }

    private void Interact()
    {
        OnInteracted?.Invoke();
    }

    IEnumerator StartAbilityBCooldown() {
        yield return new WaitForSeconds(AbilityBCooldown);
        _bOnCd = !_bOnCd;
    }


    private void FixedUpdate() {
        StatesManager.Update(Time.fixedDeltaTime);
        _rb.velocity = new Vector3(moveDirection.x * moveSpeed, _rb.velocity.y, moveDirection.y * moveSpeed);
        var look = new Vector3(_lookDirection.x, 0, _lookDirection.y);
        if (_lookDirection.x != 0 && _lookDirection.y != 0) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(look), 0.15f);
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        if (!_shieldDashHold) {
            var cam = Camera.main.transform;
            var input = context.ReadValue<Vector2>();

            var forward = cam.forward;
            var right = cam.right;

            forward.y = 0;
            right.y = 0;

            forward = forward.normalized;
            right = right.normalized;

            var fwInput = input.y * forward;
            var riInput = input.x * right;

            var camMove = fwInput + riInput;

            moveDirection = new Vector2(camMove.x, camMove.z);
        }
    }

    public void OnLook(InputAction.CallbackContext context) {
        if (context.performed && !_shieldDashHold) {
            _lookDirection = context.ReadValue<Vector2>();

            var cam = Camera.main.transform;
            var input = context.ReadValue<Vector2>();

            var forward = cam.forward;
            var right = cam.right;

            forward.y = 0;
            right.y = 0;

            forward = forward.normalized;
            right = right.normalized;

            var fwInput = input.y * forward;
            var riInput = input.x * right;

            var camMove = fwInput + riInput;

            _lookDirection = new Vector2(camMove.x, camMove.z);
        }
    }

    public void OnGiveHealth(InputAction.CallbackContext context) {
        if (context.performed) {
            Health.TransferHealth(this, otherPlayer.GetComponent<Player>());
        }
    }
}