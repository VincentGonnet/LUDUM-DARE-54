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

    [SerializeField] public GameObject errorCanvas;
    [SerializeField] public GameObject errorDialogue;
    [SerializeField] public GameObject errorMemoryText;

    [SerializeField] public Inventory inventory;
    float maxMemory;
    float currentMemory;


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
        if(other.gameObject.tag == "Player") {
            Debug.Log("Player entered error event");
            currentMemory = other.gameObject.GetComponent<Inventory>().currentMemory;
            maxMemory = other.gameObject.GetComponent<Inventory>().maxMemory;
            maxMemory -= 1;
            ErrorTrigger(currentMemory, maxMemory);
            Destroy(this.gameObject);
        }
    }


    void ErrorTrigger(float currentMemory, float maxMemory) {
        errorCanvas.SetActive(true);
        errorCanvas.transform.GetChild(0).gameObject.SetActive(true);
        errorCanvas.transform.GetChild(1).gameObject.SetActive(false);

        Debug.Log("Current memory: " + currentMemory + " Max memory: " + maxMemory);
        if (currentMemory > maxMemory) {
            errorCanvas.transform.GetChild(1).gameObject.SetActive(true);
            errorCanvas.transform.position = new Vector3(errorCanvas.transform.position.x, errorCanvas.transform.position.y + 100, errorCanvas.transform.position.z);
        }

        int i = UnityEngine.Random.Range(0, dialogues.Count);
        errorDialogue.GetComponent<TextMeshProUGUI>().text = dialogues[i];
        errorMemoryText.GetComponent<TextMeshProUGUI>().text = "Your memory decreased by 1. You now have " + maxMemory + " memory.";
    }
}
