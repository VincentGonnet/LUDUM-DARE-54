using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool isPaused;
    private PlayerController focusedPlayerController;

    // UI
    public GameObject errorCanvas;

    void Start()
    {
        isPaused = false;

        errorCanvas = GameObject.Find("ErrorEvent");
        errorCanvas.SetActive(false);

        SetupBasedOnGameState();
    }

    void UpdateUI()
    {
        if (inScenePlayer.GetComponent<Inventory>().Can(SkillType.UIHealth))
        {
            // TODO: Update UI
        }
        else
        {
            // TODO: Update UI
        }

        if (!inScenePlayer.GetComponent<Inventory>().Can(SkillType.UIMyopia))
        {
            myopia.SetActive(true);
        }
        else
        {
            myopia.SetActive(false);
        }
    }

    // TODO : delete this method and all multiplayer related code if not using multiplayer
    void SetupBasedOnGameState()
    {
        switch (currentGameMode)
        {
            case GameMode.SinglePlayer:
                SetupSinglePlayer(); // to extract if no multiplayer
                break;

            case GameMode.LocalMultiplayer:
                SetupLocalMultiplayer();
                break;
        }
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

    void SetupLocalMultiplayer()
    {
        if (inScenePlayer == true)
        {
            Destroy(inScenePlayer);
        }

        SpawnPlayers();
        SetupActivePlayers();
    }

    void SpawnPlayers()
    {
        activePlayerControllers = new List<PlayerController>();

        for (int i = 0; i < numberOfPlayers; i++)
        {
            Vector3 spawnPosition = new Vector3(0, 0, 0); // TODO: write algorithm to calculate spawn position
            Quaternion spawnRotation = Quaternion.identity; // TODO: set a spawn rotation (if we use rotations)

            GameObject spawnedPlayer = Instantiate(playerPrefab, spawnPosition, spawnRotation);
            AddPlayerToActivePlayerList(spawnedPlayer.GetComponent<PlayerController>());
        }
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
            activePlayerControllers[i].SetupPlayer(i);
        }
    }


    // Pause methods

    public void TogglePauseState(PlayerController newFocusedPlayerController)
    {
        focusedPlayerController = newFocusedPlayerController;

        isPaused = !isPaused;

        ToggleTimeScale();

        UpdateActivePlayerInputs(); // Will only deactivate input on the player's current action map

        SwitchFocusedPlayerControlScheme(); // Switch the action map for the player that triggered the pause

        // UpdateUIMenu();
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

    void SwitchFocusedPlayerControlScheme()
    {
        switch (isPaused)
        {
            case true:
                focusedPlayerController.EnablePauseMenuControls();
                break;

            case false:
                focusedPlayerController.EnableGameplayControls();
                break;
        }
    }

    // Deactivate input for all players if paused, activate if not paused
    void UpdateActivePlayerInputs()
    {
        for (int i = 0; i < activePlayerControllers.Count; i++)
        {
            if (activePlayerControllers[i] != focusedPlayerController)
            {
                activePlayerControllers[i].SetInputActiveState(isPaused);
            }

        }
    }
}
