using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InputPromptController))]
public class InputPromptEditor : Editor
{
    private SerializedProperty controllerProperty;
    private SerializedProperty choiceIndexProperty;
    private SerializedProperty sizeProperty;

    void OnEnable()
    {
        controllerProperty = serializedObject.FindProperty("controller");
        choiceIndexProperty = serializedObject.FindProperty("choiceIndex");
        sizeProperty = serializedObject.FindProperty("size");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawControllerGUI();
        DrawKeyGUI();

        DrawSpaceGUI(1);

        DrawSizeGUI();

        serializedObject.ApplyModifiedProperties();
    }

    void DrawControllerGUI()
    {
        EditorGUILayout.LabelField("Key Selection", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(controllerProperty);
    }

    void DrawKeyGUI()
    {
        string[] choiceNames = GetChoiceNames();
        choiceIndexProperty.intValue = EditorGUILayout.Popup("Key", choiceIndexProperty.intValue, choiceNames);
    }

    void DrawSpaceGUI(int numberOfSpaces)
    {
        for (int i = 0; i < numberOfSpaces; i++)
        {
            EditorGUILayout.Space();
        }
    }

    string[] GetChoiceNames()
    {
        string controllerPath = "KeyboardAndMouse";

        if (controllerProperty.enumValueIndex == 0)
        {
            controllerPath = "KeyboardAndMouse";
        }
        else if (controllerProperty.enumValueIndex == 1)
        {
            controllerPath = "PS4";
        }
        else if (controllerProperty.enumValueIndex == 2)
        {
            controllerPath = "Xbox";
        }
        else if (controllerProperty.enumValueIndex == 3)
        {
            controllerPath = "Switch";
        }

        Sprite[] sprites = Resources.LoadAll<Sprite>("InputPrompts/" + controllerPath);
    
        string[] names = new string[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            names[i] = sprites[i].name;
        }

        string[] choiceNames = names;
        return choiceNames;
    }

    void DrawSizeGUI()
    {
        EditorGUILayout.LabelField("Size", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(sizeProperty);
    }


}
