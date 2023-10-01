using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private int playerID;

    [Header("Sub Behaviours")]
    public PlayerMovementBehaviour playerMovementBehaviour;
    public PlayerJumpBehaviour playerJumpBehaviour;
    public PlayerDashBehaviour playerDashBehaviour;
    public PlayerAttackBehaviour playerAttackBehaviour;
    public PlayerRecallBehaviour playerRecallBehaviour;

    [Header("Input Settings")]
    public PlayerInput playerInput;
    public float movementSmoothingSpeed = 1f;
    public float dashSmoothingSpeed = 100f;
    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;
    private Vector2 lastInputMovement;
    private Vector3 smoothInputDash;

    //Action Maps
    private string actionMapPlayerControls = "Player Controls";
    private string actionMapMenuControls = "Menu Controls";
    private string currentControlScheme;

    // Camera control requierements
    private GameObject currentZoneTrigger;

    // PlayerProperties
    public PlayerProperties playerProperties;

    // Stored Values
    private Vector3 lastMovementDirection = Vector3.zero;
    public ArrayList jumpPodsPressedPos = new ArrayList();

    public bool isAttacked = false;
    public void setIsAttacked(bool val){
        isAttacked = val;
    }

    public void SetupPlayer(int newPlayerID) 
    {
        this.playerID = newPlayerID;
        this.currentControlScheme = this.playerInput.currentControlScheme;

        playerMovementBehaviour.SetupBehaviour();
        playerJumpBehaviour.SetupBehaviour();
        playerDashBehaviour.SetupBehaviour();
        playerAttackBehaviour.SetupBehaviour();
        playerRecallBehaviour.SetupBehaviour();
    }

    void Start()
    {
        playerProperties = this.GetComponent<PlayerProperties>();
    }


    // ------------------------------------------
    //       INPUT SYSTEM ACTION METHODS
    // ------------------------------------------

    // Movement Action
    public void OnMovement(InputAction.CallbackContext value)
    {
        if(playerProperties.Can(SkillType.Movement) || true) {
            Vector2 input = value.ReadValue<Vector2>();
            if(input != Vector2.zero) {
                lastInputMovement = input.normalized;
            }
            rawInputMovement = new Vector3(input.x, input.y, 0);
            lastMovementDirection = new Vector3(lastInputMovement.x, lastInputMovement.y, 0);
        }
    }

    // Jump Action
    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.started && jumpPodsPressedPos.Count > 0 && playerProperties.Can(SkillType.Jump))
        {
            // Do Jump
            playerJumpBehaviour.Jump(lastMovementDirection, jumpPodsPressedPos);
        }
    }

    // Dash Action
    public void OnDash(InputAction.CallbackContext value)
    {
        if(value.started && playerProperties.Can(SkillType.Dash) && !CooldownManager.Instance.IsOnCooldown(SkillType.Dash))
        {
            // Do Dash
            playerDashBehaviour.Dash();
        }
    }  

    // Attack Action
    public void OnAttack(InputAction.CallbackContext value)
    {
        if(value.started && playerProperties.Can(SkillType.Attack))
        {
            // Do Attack
            playerAttackBehaviour.Attack();
        }
    }

    // Recall Action
    public void OnRecall(InputAction.CallbackContext value)
    {
        if(value.started /*&& playerProperties.Can(SkillType.Recall)*/)
        {
            // Do Recall
            Debug.Log("Recall function");
            playerRecallBehaviour.StartRecall();
        }
    } 

    public void OnTogglePause(InputAction.CallbackContext value)
    {
        if(value.started)
        {
            GameManager.Instance.TogglePauseState(this);
        }
    }

    // PAUSE METHODS --------------

    public void EnableGameplayControls()
    {
        playerInput.SwitchCurrentActionMap(actionMapPlayerControls);  
    }

    public void EnablePauseMenuControls()
    {
        playerInput.SwitchCurrentActionMap(actionMapMenuControls);
    }

    public void SetInputActiveState(bool gameIsPaused)
    {
        switch (gameIsPaused)
        {
            case true:
                playerInput.DeactivateInput();
                break;

            case false:
                playerInput.ActivateInput();
                break;
        }
    }

    // INPUT SYSTEM AUTOMATIC CALLBACKS --------------
    public void OnControlsChanged()
    {
        if(playerInput.currentControlScheme != currentControlScheme)
        {
            currentControlScheme = playerInput.currentControlScheme;
        }
    }

    // Get Data ----
    public int GetPlayerID()
    {
        return playerID;
    }

    public InputActionAsset GetActionAsset()
    {
        return playerInput.actions;
    }

    public PlayerInput GetPlayerInput()
    {
        return playerInput;
    }

    // Update Loop - Frame-based data
    void Update()
    {
        CalculateMovementInputSmoothing();
        UpdatePlayerMovement();
        
        CalculateDashInputSmoothing();
        UpdatePlayerDash();
    }

    void CalculateMovementInputSmoothing()
    {
        smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
    }

    void UpdatePlayerMovement()
    {
        playerMovementBehaviour.UpdateMovementData(smoothInputMovement);
    }
    
    // Triggers
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("ZoneTrigger"))
        {
            currentZoneTrigger = other.gameObject;
        }
    }

    public GameObject GetCurrentZoneTrigger()
    {
        return currentZoneTrigger;
    }

    void CalculateDashInputSmoothing()
    {
        smoothInputDash = Vector3.Lerp(smoothInputDash, new Vector3(lastInputMovement.x, lastInputMovement.y, 0), Time.deltaTime * dashSmoothingSpeed);
    }

    void UpdatePlayerDash()
    {
        // Dash in the direction the player is facing when the dash button is pressed 
        playerDashBehaviour.UpdateDashData(new Vector3(lastInputMovement.x, lastInputMovement.y, 0));
    }
    public Vector3 GetLastMovementDirection()
    {
        return lastMovementDirection;
    }

    // JUMP METHODS --------------

    public void AddPodPressed(Vector3 podPos)
    {
        jumpPodsPressedPos.Add(podPos);
    }

    public void RemovePodPressed(Vector3 podPos)
    {
        jumpPodsPressedPos.Remove(podPos);
    }
}
    