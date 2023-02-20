using System;
using System.Collections;
using System.Collections.Generic;
using Andreas.Scripts;
using Andreas.Scripts.RopeSystem;
using Andreas.Scripts.RopeSystem.RopeStates;
using Andreas.Scripts.StateMachine;
using Andreas.Scripts.StateMachine.States;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using AttackNamespace;
using Health;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour, Attack.IPlayerAttacker, HealthSystem.IDamagable {
    public Vector2 moveDirection;
    private Vector2 _lookDirection;
    public Vector3 lookDirV3;
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
    [field: SerializeField] public float AbilityBCooldown { get; set; }

    public List<Material> allMats = new List<Material>();

    public float currentAbilityACooldown;
    public float currentAbilityBCooldown;

    private bool _bOnCd, _aOnCd, _xOnCd;

    public event Action OnInteracted;

    public StatesManager StatesManager;

    private ProjectileReceiver _projectileReceiver;

    public Animator _animController;

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
        StatesManager = new();
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

    private void HealthOnOnDamageTaken() {
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
                GameManager.Instance.PlayerHudUi.Players.Add(this);

                name = "RangedPlayer";
                AbilityBCooldown = 8;
                AbilityACooldown = 3;
                if (playerAttackScheme != null) {
                    playerAttackScheme.characterType = PlayerAttackScheme.Character.Ranged;
                }

                _model = transform.Find("Jose").gameObject;
                _model.SetActive(true);
                _animController = _model.GetComponent<Animator>();
                break;
            case CharacterType.Melee:
                GameManager.Instance.PlayerHudUi.Players.Add(this);
                name = "MeleePlayer)";
                AbilityBCooldown = 5;
                AbilityACooldown = 15;
                if (playerAttackScheme != null) {
                    playerAttackScheme.characterType = PlayerAttackScheme.Character.Melee;
                }

                _model = transform.Find("Bronk").gameObject;
                _model.SetActive(true);
                _animController = _model.GetComponent<Animator>();
                break;
        }
    }

    private void Update() {
        StatesManager.Update(Time.deltaTime);

        if (_aOnCd) {
            currentAbilityACooldown -= Time.deltaTime;
        }

        if (_bOnCd) {
            currentAbilityBCooldown -= Time.deltaTime;
        }

        //  test        
        if (Input.GetKeyDown(KeyCode.F)) {
            TestHealth();
            // Interact();
        }
    }

    private void HoldBasic() {
        if (playerAttackScheme != null) {
            playerAttackScheme.BasicAttacksList[0]();
            _animController.SetTrigger("BasicAttackTrig");
        }
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
                _animController.SetTrigger("ShieldDomeTrig");
                _aOnCd = !_aOnCd;
                StartCoroutine(StartAbilityACooldown());
            }
        }
    }

    IEnumerator StartAbilityACooldown() {
        currentAbilityACooldown = AbilityACooldown;
        GameManager.Instance.PlayerHudUi.SetCooldown(cType, 1);
        yield return new WaitForSeconds(AbilityACooldown);
        _aOnCd = !_aOnCd;
    }

    public void OnAbilityB(InputAction.CallbackContext context) {
        if (context.performed && !_bOnCd) {
            if (playerAttackScheme != null) {
                playerAttackScheme.BasicAttacksList[2]();
                _animController.SetTrigger("ShieldSlamTrig");
                _bOnCd = !_bOnCd;
                StartCoroutine(StartAbilityBCooldown());
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if (context.performed) {
            OnInteracted?.Invoke();
        }
    }

    private void Interact() {
        OnInteracted?.Invoke();
    }

    IEnumerator StartAbilityBCooldown() {
        currentAbilityBCooldown = AbilityBCooldown;
        GameManager.Instance.PlayerHudUi.SetCooldown(cType, 0);
        yield return new WaitForSeconds(AbilityBCooldown);
        _bOnCd = !_bOnCd;
    }


    private void FixedUpdate() {
        lookDirV3 = new Vector3(_lookDirection.x, 0, _lookDirection.y);
        StatesManager.Update(Time.fixedDeltaTime);
        _rb.velocity = new Vector3(moveDirection.x * moveSpeed, _rb.velocity.y, moveDirection.y * moveSpeed);
        var look = new Vector3(_lookDirection.x, 0, _lookDirection.y);
        if (_lookDirection.x != 0 && _lookDirection.y != 0) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(look), 0.15f);
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
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

        var filbert = transform.TransformDirection(new Vector3(moveDirection.x, 0, moveDirection.y));


        _animController.SetFloat("VelX", filbert.x);
        _animController.SetFloat("VelY", filbert.z);
    }

    public void OnLook(InputAction.CallbackContext context) {
        if (context.performed) {
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
            TestHealth();
        }
    }

    void TestHealth() {
        Health.TransferHealth(this, otherPlayer.GetComponent<Player>());
        var rope = GameManager.Instance.RopeManager.Rope;
        if (rope != null)
            rope.GetComponent<RopeVisualThingy>().AddState(new RopeStateFlow());
    }
}