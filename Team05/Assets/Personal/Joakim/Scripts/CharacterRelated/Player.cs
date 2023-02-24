using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Andreas.Scripts;
using Andreas.Scripts.EnemyData;
using Andreas.Scripts.PlayerData;
using Andreas.Scripts.RopeSystem;
using Andreas.Scripts.RopeSystem.RopeStates;
using Andreas.Scripts.StateMachine;
using Andreas.Scripts.StateMachine.States;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using AttackNamespace;
using AudioSystem;
using Health;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using Util;

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

    private Light pl;

    public bool isDead = false;

    private GameObject _model;

    public GameObject feetPos;

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
    [NonSerialized] public PlayerSfxData SfxData;

    private ProjectileReceiver _projectileReceiver;

    private float _lowHealthWarningCooldownTimer;
    public Animator animController;

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
        pl = GetComponentInChildren<Light>();
        StatesManager = new();
        Health = new HealthSystem();
        Health.OnDamageTaken += HealthOnOnDamageTaken;
        Health.OnDie += HealthOnOnDie;
        _playerNumber = PlayerJoinManager.Instance.playerNumber;
        gameObject.tag = _playerNumber == 1 ? "Player1" : "Player2";
        GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        _rb = GetComponent<Rigidbody>();
        _projectileReceiver = GetComponent<ProjectileReceiver>();
        _projectileReceiver.OnHit.AddListener(OnHitRef);

#if UNITY_EDITOR
        EditorApplication.playmodeStateChanged += ModeChanged;
#endif
    }

    private void HealthOnOnDie() {
        SfxData.Die.Play(transform.position);
    }

    private void HealthOnOnDamageTaken() {
        StatesManager.AddState(new StateColorFlash(_model, Color.red));
        
        AudioData voiceLine = SfxData.WhenHit; 
        if(_lowHealthWarningCooldownTimer <= 0)
        {
            var lowHealth = Mathf.Lerp(HealthSystem.MinHp, HealthSystem.MaxHp, 0.25f);
            if(lowHealth <= Health.Health) {
                voiceLine = SfxData.LowHealth;
                const float LowHealthWarningVoiceCooldownTime = 3.5f; 
                _lowHealthWarningCooldownTimer = LowHealthWarningVoiceCooldownTime;
            }
        }
        
        voiceLine.Play(transform.position);
    }

    private void OnHitRef(Projectile proj) {
        Health.InstantDamage(this, proj.Damage);
        if(proj.StunDuration > 0f) {
            StartCoroutine(TemporaryDisableInput(proj.StunDuration));
            SfxData.Stunned.Play(transform.position);
        }
    }

    private IEnumerator TemporaryDisableInput(float duration) {
        var inputActions = GetComponent<PlayerInput>().actions; 
        inputActions.Disable();
        yield return new WaitForSeconds(duration);
        inputActions.Enable();
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
                pl.color = new Color(53, 93, 123);

                var modelJosePrefab = FastResources.Load<GameObject>("Prefabs/Player/Model/Jose");
                _model = Instantiate(modelJosePrefab, gameObject.transform);
                animController = _model.GetComponent<Animator>();
                break;
            case CharacterType.Melee:
                GameManager.Instance.PlayerHudUi.Players.Add(this);
                name = "MeleePlayer";
                AbilityBCooldown = 5;
                AbilityACooldown = 15;
                if (playerAttackScheme != null) {
                    playerAttackScheme.characterType = PlayerAttackScheme.Character.Melee;
                }
                pl.color = new Color(101, 183, 1);

                var modelBronkPrefab = FastResources.Load<GameObject>("Prefabs/Player/Model/Bronk");
                _model = Instantiate(modelBronkPrefab, gameObject.transform);
                animController = _model.GetComponent<Animator>();
                break;
        }
    }

    private void Update() {
        StatesManager.Update(Time.deltaTime);

        _lowHealthWarningCooldownTimer -= Time.deltaTime;
        
        if (_aOnCd) {
            currentAbilityACooldown -= Time.deltaTime;
        }

        if (_bOnCd) {
            currentAbilityBCooldown -= Time.deltaTime;
        }

        //  test       
#if UNITY_EDITOR
        // if (Input.GetKeyDown(KeyCode.F)) {
            // TestHealth();
            // Interact();
        // }
#endif
    }

    private void HoldBasic() {
        if (playerAttackScheme != null) {
            playerAttackScheme.BasicAttacksList[0]();
            animController.SetTrigger("BasicAttackTrig");
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
                animController.SetTrigger("ShieldDomeTrig");
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
                animController.SetTrigger("ShieldSlamTrig");
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


        animController.SetFloat("VelX", filbert.x);
        animController.SetFloat("VelY", filbert.z);
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
    
    public void OnPause(InputAction.CallbackContext context) {
        if (context.performed && !isDead) {
            GameManager.Instance.TogglePause();
        }
    }
    
    public void OnPauseMenuResume(InputAction.CallbackContext context) {
        if (context.performed && !isDead) {
            GameManager.Instance.TogglePause();
        }
    }
    
    public void OnPauseMenuQuit(InputAction.CallbackContext context) {
        if (context.performed && !isDead) {
            GameManager.Instance.TogglePause();
            SceneManager.LoadScene("MainScene");
        }
    }

    public void OnGiveHealth(InputAction.CallbackContext context) {
        if (context.performed) {
            TestHealth();
        }
    }

    void TestHealth() {
        Health.TransferHealth(this, otherPlayer.GetComponent<Player>());
        if(cType == CharacterType.Melee)
        {
            AudioManager.PlaySfx("BrankSharingLife_mixdown", transform.position);
        }
        else if(cType == CharacterType.Ranged)
        {
            AudioManager.PlaySfx("JosephineSharingLife_mixdown", transform.position);
        } 
        var rope = GameManager.Instance.RopeManager.Rope;
        if (rope != null)
            rope.GetComponent<RopeVisualThingy>().AddState(new RopeStateFlow());
    }
}