using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    private GameManager gameManager;
    
    //Game Mode
    private SerializedProperty currentGameModeProperty;

    //Single Player
    private SerializedProperty inScenePlayerProperty;
    private SerializedProperty singlePlayerCameraModeProperty;

    //Local Multiplayer
    private SerializedProperty playerPrefabProperty;
    private SerializedProperty numberOfPlayersProperty;

    private SerializedProperty myopiaProperty;
    private SerializedProperty healthBarProperty;
    private SerializedProperty skillsUIProperty;
    private SerializedProperty numberOfTrashPickedUpProperty;
    private SerializedProperty maxNumberOfTrashProperty;

    void OnEnable()
    {
        //Game Mode
        currentGameModeProperty = serializedObject.FindProperty("currentGameMode");

        //Single Player
        inScenePlayerProperty = serializedObject.FindProperty("inScenePlayer");
        singlePlayerCameraModeProperty = serializedObject.FindProperty("singlePlayerCameraMode");

        //Local Multiplayer
        playerPrefabProperty = serializedObject.FindProperty("playerPrefab");
        numberOfPlayersProperty = serializedObject.FindProperty("numberOfPlayers");

        myopiaProperty = serializedObject.FindProperty("myopia");

        healthBarProperty = serializedObject.FindProperty("healthBar");

        skillsUIProperty = serializedObject.FindProperty("skillsUI");

        numberOfTrashPickedUpProperty = serializedObject.FindProperty("numberOfTrashPickedUp");
    }


    public override void OnInspectorGUI()
    {

        gameManager = (GameManager)target;

        serializedObject.Update();

        DrawGameModeGUI();
        
        DrawSpaceGUI(3);

        EditorGUILayout.LabelField("Initialization Mode Settings", EditorStyles.boldLabel);

        if(gameManager.currentGameMode == GameMode.SinglePlayer)
        {
            DrawSinglePlayerGUI();
        }

        if(gameManager.currentGameMode == GameMode.LocalMultiplayer)
        {
            DrawLocalMultiplayerGUI();
        }

        DrawSpaceGUI(3);

        EditorGUILayout.LabelField("UI Settings", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(myopiaProperty);

        EditorGUILayout.PropertyField(healthBarProperty);

        EditorGUILayout.PropertyField(skillsUIProperty);

        EditorGUILayout.PropertyField(numberOfTrashPickedUpProperty);

        serializedObject.ApplyModifiedProperties();
    }

    void DrawGameModeGUI()
    {
        EditorGUILayout.LabelField("Game Mode", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(currentGameModeProperty);
    }

    void DrawSinglePlayerGUI()
    {
        EditorGUILayout.PropertyField(inScenePlayerProperty);
    }

    void DrawLocalMultiplayerGUI()
    {

        EditorGUILayout.PropertyField(playerPrefabProperty);

        EditorGUILayout.PropertyField(numberOfPlayersProperty);
    }

    void DrawSpaceGUI(int amountOfSpace)
    {
        for(int i = 0; i < amountOfSpace; i++)
        {
            EditorGUILayout.Space();
        }
    }
}

