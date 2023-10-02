using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    SinglePlayer,
    LocalMultiplayer
}

public class GameManager : Singleton<GameManager>
{
    public GameMode currentGameMode; // Selected between single player and local multiplayer

    // Single player
    public GameObject inScenePlayer;

    //Local Multiplayer
    public GameObject playerPrefab;
    public int numberOfPlayers;

    // Myopia gameobject
    public GameObject myopia;

    // Player controllers
    public List<PlayerController> activePlayerControllers; // List of all active players, referencing their PlayerController scripts
    public bool isPaused;
    private PlayerController focusedPlayerController;

    // UI
    public GameObject errorCanvas;

    public GameObject healthBar;
    public GameObject skillsUI;
    public TutorialManager tutorialManager;
    public bool memoryOverload = false;
    [SerializeField] private GameObject pauseMenu;

    // Timer
    public float timer = 0f;
    private bool timerOn = true;

    public int numberOfTrashPickedUp = 0;
    public void setNumberOfTrashPickedUp(int value){
        numberOfTrashPickedUp = value;
        if (numberOfTrashPickedUp >= maxNumberOfTrash){
            Debug.Log("You win!");

            // Stop Timer
            timerOn = false;

            // Start Overlay as win
            GameObject.Find("DeathWinAnim").GetComponent<DeathWinManager>().StartOverlay(true);
        }
    }
    public int getNumberOfTrashPickedUp(){
        return numberOfTrashPickedUp;
    }
    public int maxNumberOfTrash = 3;

    private void Update()
    {
        if (timerOn)
        {
            timer += Time.deltaTime;
        }
    }

    void Start()
    {
        tutorialManager.DisableCanvas();
        isPaused = false;

        errorCanvas = GameObject.Find("ErrorEvent");
        errorCanvas.SetActive(false);

        SetupBasedOnGameState();

        UpdateUI();

        // Instanciate Cooldown Manager
        CooldownManager.Instance.InitScriptableObjects();
    }

    public void UpdateUI()
    {
        if (inScenePlayer.GetComponent<PlayerProperties>().Can(SkillType.UIHealth))
        {
            healthBar.SetActive(true);
            skillsUI.SetActive(true);
        }
        else
        {
            healthBar.SetActive(false);
            skillsUI.SetActive(false);
        }

        if (!inScenePlayer.GetComponent<PlayerProperties>().Can(SkillType.UIMyopia))
        {
            myopia.SetActive(true);
            GameObject.Find("Global Light 2D").GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0f;
        }
        else
        {
            myopia.SetActive(false);
            GameObject.Find("Global Light 2D").GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 1f;
        }
    }

    // TODO: delete this method and all multiplayer related code if not using multiplayer
    void SetupBasedOnGameState()
    {
        SetupSinglePlayer();
    }

    void SetupSinglePlayer()
    {
        activePlayerControllers = new List<PlayerController>();

        // Setup player
        if (inScenePlayer == true)
        {
            AddPlayerToActivePlayerList(inScenePlayer.GetComponent<PlayerController>());
        }

        SetupActivePlayers();
    }

    void AddPlayerToActivePlayerList(PlayerController newPlayer)
    {
        activePlayerControllers.Add(newPlayer);
    }

    // Start setup script for active players
    void SetupActivePlayers()
    {
        for (int i = 0; i < activePlayerControllers.Count; i++)
        {
            activePlayerControllers[i].SetupPlayer();
        }
    }


    // Pause methods

    public void TogglePauseState(bool pauseMenu = false)
    {

        PlayerController playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        isPaused = !isPaused;

        ToggleTimeScale();

        // UpdateActivePlayerInputs(); // Will only deactivate input on the player's current action map

        switch (isPaused)
        {
            case true:
                playerController.EnablePauseMenuControls();
                break;

            case false:
                playerController.EnableGameplayControls();
                break;
        }

        if (pauseMenu) UpdateUIMenu();
    }

    void ToggleTimeScale()
    {
        float newTimeScale = 0f;

        switch (isPaused)
        {
            case true:
                newTimeScale = 0f;
                break;

            case false:
                newTimeScale = 1f;
                break;
        }

        Time.timeScale = newTimeScale;
    }

    public void ToggleDialogControls(PlayerController newlyFocusedPlayerController, bool toggle) {
        focusedPlayerController = newlyFocusedPlayerController;
        if (toggle) {
            focusedPlayerController.EnableDialogControls();
        } else {
            focusedPlayerController.EnableGameplayControls();
        }
    }

    void UpdateUIMenu()
    {
        pauseMenu.SetActive(isPaused);
    }

    public void ToMainMenu()
    {
        TogglePauseState(true);
        SceneManager.LoadScene("MainMenu");
    }
}
