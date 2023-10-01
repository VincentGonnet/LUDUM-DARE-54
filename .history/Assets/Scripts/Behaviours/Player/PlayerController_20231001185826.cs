using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{

    //[Header("Sub Behaviours")]
    PlayerMovementBehaviour playerMovementBehaviour{
        get{
            return GetComponent<PlayerMovementBehaviour>();
        }
    }
    PlayerJumpBehaviour playerJumpBehaviour{
        get{
            return GetComponent<PlayerJumpBehaviour>();
        }
    }
    PlayerDashBehaviour playerDashBehaviour{
        get{
            return GetComponent<PlayerDashBehaviour>();
        }
    }
    PlayerAttackBehaviour playerAttackBehaviour{
        get{
            return GetComponent<PlayerAttackBehaviour>();
        }
    }
    PlayerRecallBehaviour playerRecallBehaviour{
        get{
            return GetComponent<PlayerRecallBehaviour>();
        }
    }
    PlayerPickUpBehaviour playerPickUpBehaviour{
        get{
            return GetComponent<PlayerPickUpBehaviour>();
        }
    }

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
    private string actionMapDialogControls = "Dialog Controls";
    private string currentControlScheme;

    // Camera control requierements
    private GameObject currentZoneTrigger;

    // PlayerProperties
    public PlayerProperties playerProperties;

    // Stored Values
    private Vector3 lastMovementDirection = Vector3.zero;
    public ArrayList jumpPodsPressedPos = new ArrayList();

    public bool isRecalling = false;
    public void setIsRecalling(bool val){
        isRecalling = val;
    }
    public bool isAttackedWhileRecall = false;
    public void setIsAttackedWhileRecall(bool val){
        isAttackedWhileRecall = val;
    }

    public void SetupPlayer() 
    {
        this.currentControlScheme = this.playerInput.currentControlScheme;

        playerMovementBehaviour.SetupBehaviour();
        playerJumpBehaviour.SetupBehaviour();
        playerDashBehaviour.SetupBehaviour();
        playerAttackBehaviour.SetupBehaviour();
        playerRecallBehaviour.SetupBehaviour();
        playerPickUpBehaviour.SetupBehaviour();

    }

    public void CutscenesLaunch(int i) {

        GameManager.Instance.tutorialManager.PlayTutorial(i);

    }


    void Start()
    {
        playerProperties = this.GetComponent<PlayerProperties>();
        GameManager.Instance.tutorialManager.currentCutscene.SetTarget(GetComponent<PlayableDirector>());
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
            CooldownManager.Instance.StartCooldown(SkillType.Jump);
            playerJumpBehaviour.Jump(lastMovementDirection, jumpPodsPressedPos);
        }
    }

    // Dash Action
    public void OnDash(InputAction.CallbackContext value)
    {
        if(value.started && playerProperties.Can(SkillType.Dash) && !CooldownManager.Instance.IsOnCooldown(SkillType.Dash))
        {
            // Do Dash
            CooldownManager.Instance.StartCooldown(SkillType.Dash);
            playerDashBehaviour.Dash();
        }
    }  

    // Attack Action
    public void OnAttack(InputAction.CallbackContext value)
    {
        if(value.started && playerProperties.Can(SkillType.Attack) && !CooldownManager.Instance.IsOnCooldown(SkillType.Attack))
        {
            // Do Attack
            CooldownManager.Instance.StartCooldown(SkillType.Attack);
            playerAttackBehaviour.Attack();
        }
    }

    // Recall Action
    public void OnRecall(InputAction.CallbackContext value)
    {
        if(value.started && playerProperties.Can(SkillType.Recall) && !CooldownManager.Instance.IsOnCooldown(SkillType.Recall))
        {
            // Do Recall
            CooldownManager.Instance.StartCooldown(SkillType.Recall);
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


    public void OnPickUp(InputAction.CallbackContext value){
        if(value.started){
            //Do Pickup
            playerPickUpBehaviour.PickUp();
        }
    }

    // PAUSE METHODS --------------

    public void EnableGameplayControls()
    {
        Debug.Log("Enabling Gameplay Controls");
        playerInput.SwitchCurrentActionMap(actionMapPlayerControls);  
    }

    public void EnablePauseMenuControls()
    {
        Debug.Log("Enabling Pause Menu Controls");
        playerInput.SwitchCurrentActionMap(actionMapMenuControls);
    }

    public void EnableDialogControls()
    {
        Debug.Log("Enabling Dialog Controls");
        playerInput.SwitchCurrentActionMap(actionMapDialogControls);
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

        DisplayInputPrompt();
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

    private GameObject jumpPrompt;
    private void DisplayInputPrompt()
    {
        if (jumpPodsPressedPos.Count > 0 && playerProperties.Can(SkillType.Jump))
        {
            if (jumpPrompt == null) {
                jumpPrompt = GameObject.Instantiate(Resources.Load<GameObject>("InputPrompts/JumpPrompt"));
            }
            jumpPrompt.transform.position = transform.position + new Vector3(0, 1, 0);
        } else if (jumpPrompt != null)
        {
            Destroy(jumpPrompt);
            jumpPrompt = null;
        }
    }
    
    public void SetSpriteDirection(Vector2 direction) {
        GetComponent<Animator>().SetFloat("DirectionX", direction.x);
        GetComponent<Animator>().SetFloat("DirectionY", direction.y);
    }
}
    