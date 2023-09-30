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
    private Vector3 rawInputDash;
    private Vector3 smoothInputDash;

    //Action Maps
    private string actionMapPlayerControls = "Player Controls";
    private string actionMapMenuControls = "Menu Controls";

    private string currentControlScheme;

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


    // ------------------------------------------
    //       INPUT SYSTEM ACTION METHODS
    // ------------------------------------------

    // Movement Action
    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 input = value.ReadValue<Vector2>();
        if(input != Vector2.zero) {
            lastInputMovement = input.normalized;
        }
        rawInputMovement = new Vector3(input.x, input.y, 0);
    }

    // Jump Action
    public void OnJump(InputAction.CallbackContext value)
    {
        if(value.started)
        {
            // Do Jump
            playerJumpBehaviour.Jump();
        }
    }

    // Dash Action
    public void OnDash(InputAction.CallbackContext value)
    {
        if(value.started)
        {
            // Do Dash
            playerDashBehaviour.Dash();
        }
    }  

    // Attack Action
    public void OnAttack(InputAction.CallbackContext value)
    {
        if(value.started)
        {
            // Do Attack
            playerAttackBehaviour.Attack();
        }
    }

    // Recall Action
    public void OnRecall(InputAction.CallbackContext value)
    {
        if(value.started)
        {
            // Do Recall
            playerRecallBehaviour.Recall();
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

     void CalculateDashInputSmoothing()
    {
        smoothInputDash = Vector3.Lerp(smoothInputDash, new Vector3(lastInputMovement.x, lastInputMovement.y, 0), Time.deltaTime * dashSmoothingSpeed);
    }

    void UpdatePlayerDash()
    {
        // Dash in the direction the player is facing when the dash button is pressed 
        playerDashBehaviour.UpdateDashData(new Vector3(lastInputMovement.x, lastInputMovement.y, 0));
    }
}
    