using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private int playerID;

    [Header("Sub Behaviours")]
    public PlayerMovementBehaviour playerMovementBehaviour;

    [Header("Input Settings")]
    public PlayerInput playerInput;
    public float movementSmoothingSpeed = 1f;
    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;

    //Action Maps
    private string actionMapPlayerControls = "Player Controls";
    private string actionMapMenuControls = "Menu Controls";

    private string currentControlScheme;

    public void SetupPlayer(int newPlayerID) 
    {
        this.playerID = newPlayerID;
        this.currentControlScheme = this.playerInput.currentControlScheme;

        playerMovementBehaviour.SetupBehaviour();
    }



    // INPUT SYSTEM ACTION METHODS --------------
    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        rawInputMovement = new Vector3(inputMovement.x, inputMovement.y, 0);
    }

    // Example of a button press
    public void OnAttack(InputAction.CallbackContext value)
    {
        if(value.started)
        {
            // Do Attack
        }
    }

    public void OnTogglePause(InputAction.CallbackContext value)
    {
        if(value.started)
        {
            // GameManager.Instance.TogglePauseState(this);
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
    }

    void CalculateMovementInputSmoothing()
    {
        smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
    }

    void UpdatePlayerMovement()
    {
        playerMovementBehaviour.UpdateMovementData(smoothInputMovement);
    }


}
