using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEditor.PackageManager;
using System;

public class ErrorEvent : MonoBehaviour
{

    GameObject canvas;
    string errorText;

    [SerializeField] string diag1;
    [SerializeField] string diag2;
    [SerializeField] string diag3;
    [SerializeField] string diag4;

    List<string> dialogues;


    // Start is called before the first frame update
    void Start()
    {
        dialogues = new List<string>(){diag1, diag2, diag3, diag4};
        ErrorTrigger();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ErrorTrigger() {
        canvas = GameObject.Find("Canvas");

        int i = UnityEngine.Random.Range(0, dialogues.Count);
        canvas.GetComponentInChildren<TextMeshProUGUI>().text = dialogues[i];
    }
}

