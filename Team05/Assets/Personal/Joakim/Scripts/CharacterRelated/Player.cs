using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using AttackNamespace;
using FlowFieldSystem;
using Health;
using Personal.Andreas.Scripts;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour, Attack.IPlayerAttacker, HealthSystem.IDamagable {
    public Vector2 moveDirection;
    private Vector2 _lookDirection;
    private Rigidbody _rb;
    public float moveSpeed = 20;
    [CanBeNull] public PlayerAttackScheme playerAttackScheme;
    private int _playerNumber;
    private bool _switchedToCharacterMode = true;
    public GameObject otherPlayer;

    public Material HealthMaterial { get; set; }
    public HealthSystem Health { get; set; }
    [field: SerializeField] public int CurrentHealth { get; set; }
    [field: SerializeField] public float Energy { get; set; }
    [field: SerializeField] public float AttackSpeed { get; set; }

    //DEBUG ONLY
    public bool debugMode = false;

    private GameObject _debugObj;
    //END OF DEBUG

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
            debugMode = false;
        }
    }
#endif
    private void Awake() {
        Health = new HealthSystem();
        GameObject.Find("FlowFieldMap").GetComponent<FlowFieldManager>()
            .SetUnit(transform); //todo: flowfield accepts 2 players
        _playerNumber = PlayerJoinManager.Instance.playerNumber;
        gameObject.tag = _playerNumber == 1 ? "Player1": "Player2";
        GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        _rb = GetComponent<Rigidbody>();

#if UNITY_EDITOR
        EditorApplication.playmodeStateChanged += ModeChanged;
#endif
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void AssignPlayerToRole(Player.CharacterType type) {
        switch (type) {
            case CharacterType.Ranged:
                name = "RangedPlayer";
                GetComponent<PlayerAttackScheme>().characterType = PlayerAttackScheme.Character.Ranged;
                playerAttackScheme = GetComponent<PlayerAttackScheme>();
                if (playerAttackScheme != null) playerAttackScheme.characterType = PlayerAttackScheme.Character.Ranged;
                break;
            case CharacterType.Melee:
                name = "MeleePlayer)";
                GetComponent<PlayerAttackScheme>().characterType = PlayerAttackScheme.Character.Melee;
                playerAttackScheme = GetComponent<PlayerAttackScheme>();
                if (playerAttackScheme != null) playerAttackScheme.characterType = PlayerAttackScheme.Character.Melee;
                break;
        }
    }

    private void AssignPlayerHealthMaterial(int playerNumber) {
        if (playerNumber == 1) {
            HealthMaterial = Resources.Load<Material>("Player1Health");
        }
        else {
            HealthMaterial = Resources.Load<Material>("Player2Health");
        }
    }

    private void Update() {

        //DEBUG ONLY
        if (debugMode == true) {
            switch (cType) {
                case CharacterType.Melee: {
                    var objToFollow = GameObject.Find("RangedPlayer");
                    _debugObj = objToFollow;
                    break;
                }
                case CharacterType.Ranged: {
                    var objToFollow = GameObject.Find("MeleePlayer");
                    _debugObj = objToFollow;
                    break;
                }
            }

            if (_debugObj == null) return;
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(_debugObj.transform.position.x, _debugObj.transform.position.y,
                    _debugObj.transform.position.z), 2 * Time.deltaTime);
        }
        //END OF DEBUG


        if (CharacterManager.Instance.CheckIfAllLockedIn() && _switchedToCharacterMode) {
            GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            AssignPlayerSpecifics();
            otherPlayer = GameObject.FindWithTag(_playerNumber == 1 ? "Player2" : "Player1");
            CameraTopDown.Get.SetPlayers(transform);
            _switchedToCharacterMode = false;
        }
    }

    private void AssignPlayerSpecifics() {
        AssignPlayerToRole(cType);
        AssignPlayerHealthMaterial(_playerNumber);
        Health.ResetHealth(this);
    }

    private void FixedUpdate() {
        // Debug.Log(_moveDirection);
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

        if (context.canceled) {
            CancelInvoke(nameof(HoldBasic));
        }
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