using Andreas.Scripts;
using Andreas.Scripts.PlayerData;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Unity.Mathematics;
using Util;

public class CharacterSelection : MonoBehaviour
{
    public Vector2 selectDir;
    public int selectedDir;
    public int playerNumber = 0;
    private GameObject selectionObject;
    private GameObject meleeNode, rangedNode;
    private PlayerInput _playerInput;

    private void Start()
    {
        playerNumber = PlayerJoinManager.Instance.playerNumber;
        switch(playerNumber)
        {
            case 1:
                selectionObject = GameObject.Find("Player1Selector");
                meleeNode = GameObject.Find("P1-MeleeNode");
                rangedNode = GameObject.Find("P1-RangedNode");
                break;
            case 2:
                selectionObject = GameObject.Find("Player2Selector");
                meleeNode = GameObject.Find("P2-MeleeNode");
                rangedNode = GameObject.Find("P2-RangedNode");
                break;
        }
    }

    public void SwitchSelectionVisual()
    {
        SelectedCharacter(selectDir);
        switch(selectedDir)
        {
            case -1:
                if(selectionObject != null)
                {
                    selectionObject.transform.DOMove(rangedNode.transform.position, 0.3f, false);
                }

                break;
            case 1:
                if(selectionObject != null)
                {
                    selectionObject.transform.DOMove(meleeNode.transform.position, 0.3f, false);
                }

                break;
        }
    }

    private void SelectedCharacter(Vector2 vec)
    {
        if(vec.x < -0.2f)
        {
            selectedDir = -1;
        }

        if(vec.x > 0.2f)
        {
            selectedDir = 1;
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        selectDir = context.ReadValue<Vector2>();
        SwitchSelectionVisual();
    }

    public void OnChoose(InputAction.CallbackContext context)
    {
        if(!context.performed) return;
        var player = GetComponent<Player>();
        switch(selectedDir)
        {
            case -1:
                if(!GameManager.Instance.CharacterManager.rangedLockedIn)
                {
                    GameManager.Instance.CharacterManager.rangedLockedIn = true;
                    GetComponent<Player>().cType = Player.CharacterType.Ranged;
                    GetComponentInChildren<PlayerAttackScheme>().characterType = PlayerAttackScheme.Character.Ranged;
                    GetComponentInChildren<PlayerAttackScheme>().InitializeAttack();
                    player.SfxData = FastResources.Load<PlayerSfxData>("Data/JoseSfxData");
                    GameManager.Instance.CharacterManager.Players.Add(GetComponent<Player>());
                    GameManager.Instance.CharacterManager.CheckIfAllAreLockedIn();
                }
                else
                {
                    Debug.Log("Already Picked!");
                }

                break;
            case 1:
                if(!GameManager.Instance.CharacterManager.meleeLockedIn)
                {
                    GameManager.Instance.CharacterManager.meleeLockedIn = true;
                    GetComponent<Player>().cType = Player.CharacterType.Melee;
                    GetComponentInChildren<PlayerAttackScheme>().characterType = PlayerAttackScheme.Character.Melee;
                    GetComponentInChildren<PlayerAttackScheme>().InitializeAttack();
                    player.SfxData = FastResources.Load<PlayerSfxData>("Data/BronkSfxData");
                    GameManager.Instance.CharacterManager.Players.Add(GetComponent<Player>());
                    GameManager.Instance.CharacterManager.CheckIfAllAreLockedIn();
                }
                else
                {
                    Debug.Log("Already Picked!");
                }

                break;
        }
        
        var attackScheme = GetComponentInChildren<PlayerAttackScheme>();
        attackScheme.SfxData = player.SfxData;
    }

    private void Update()
    {
        const KeyCode debugKey = KeyCode.F1;

        if(Input.GetKeyDown(debugKey))
        {
            AutoEnter();
        }
    }

    private Player SetupAutoPlayer(Player player)
    {
        player.SfxData = FastResources.Load<PlayerSfxData>("Data/BronkSfxData");
        var attackScheme = GetComponentInChildren<PlayerAttackScheme>();
        attackScheme.SfxData = player.SfxData;
        return player;
    }
    
    private void AutoEnter()
    {
        var cm = GameManager.Instance.CharacterManager;
        cm.meleeLockedIn = true;
        cm.rangedLockedIn = true;
        
        //  add player1
        cm.Players.Add(SetupAutoPlayer(GetComponent<Player>()));
        
        //  add player2
        var playerPrefab = FastResources.Load<GameObject>("Player");
        var playerObj = Instantiate(playerPrefab, Vector3.zero, quaternion.identity);
        var player2 = SetupAutoPlayer(playerObj.GetComponent<Player>());
        cm.Players.Add(player2);
        
        cm.CheckIfAllAreLockedIn();
        
        GetComponentInChildren<PlayerAttackScheme>().InitializeAttack();
        GetComponent<PlayerInput>().SwitchCurrentActionMap("TestingMap");

        playerObj.GetComponent<PlayerInput>().enabled = false;
        var targetFollower = playerObj.AddComponent<PlayerDebugComponent>();
        targetFollower.SetFollow(cm.Players[0].transform);

        //  assign to eachtoher
        cm.Players[1].otherPlayer = cm.Players[0].gameObject;
        cm.Players[0].otherPlayer = cm.Players[1].gameObject;
        
    }
    
    
    
}