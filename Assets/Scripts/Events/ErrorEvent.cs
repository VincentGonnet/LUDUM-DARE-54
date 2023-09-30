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
    GameObject errorCanvas;
    string errorText;

    [SerializeField] string diag1;
    [SerializeField] string diag2;
    [SerializeField] string diag3;
    [SerializeField] string diag4;

    List<string> dialogues;

    [SerializeField] Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        dialogues = new List<string>(){diag1, diag2, diag3, diag4};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Error event triggered");
        if(other.gameObject.tag == "Player") {
            Debug.Log("Player entered error event");
            other.gameObject.GetComponent<Inventory>().maxMemory -= 1;
            ErrorTrigger(other);
            Destroy(this.gameObject);
        }
    }


    void ErrorTrigger(Collider2D Player) {
        errorCanvas = GameObject.Find("ErrorEvent");
        errorCanvas.SetActive(true);

        int i = UnityEngine.Random.Range(0, dialogues.Count);
        errorCanvas.GetComponentInChildren<TextMeshProUGUI>().text = dialogues[i];
        errorCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Your memory decreased by 1. You now have " + Player.gameObject.GetComponent<Inventory>().maxMemory + " memory.";
    }
}

