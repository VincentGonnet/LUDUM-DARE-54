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

    // Player controllers
    public List<PlayerController> activePlayerControllers; // List of all active players, referencing their PlayerController scripts
    private bool isPaused;

    void Start()
    {
        isPaused = false;
    }

    // TODO : delete this method and all multiplayer related code if not using multiplayer
    void SetupBasedOnGameState()
    {
        switch(currentGameMode)
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
        if(inScenePlayer == true)
        {
            AddPlayerToActivePlayerList(inScenePlayer.GetComponent<PlayerController>());
        }

        SetupActivePlayers();
    }

    void SetupLocalMultiplayer()
    {   
        if(inScenePlayer == true)
        {
            Destroy(inScenePlayer);
        }

        SpawnPlayers();
        SetupActivePlayers();
    }

    void SpawnPlayers()
    {
        activePlayerControllers = new List<PlayerController>();

        for(int i = 0; i < numberOfPlayers; i++)
        {
            Vector3 spawnPosition = new Vector3(0, 0, 0); // TODO: write algorithm to calculate spawn position
            Quaternion spawnRotation = Quaternion.identity; // TODO : set a spawn rotation
            
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
        for(int i = 0; i < activePlayerControllers.Count; i++)
        {
            activePlayerControllers[i].SetupPlayer(i);
        }
    }
}
