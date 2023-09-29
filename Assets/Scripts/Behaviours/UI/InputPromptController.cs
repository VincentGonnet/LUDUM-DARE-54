using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Controller
{
    KeyboardAndMouse,
    PS4,
    Xbox,
    Switch
}

public class InputPromptController : MonoBehaviour
{
    public Controller controller;
    [SerializeField] private int choiceIndex = 0;
    [SerializeField] private float size = 1f;

    public void Start() {
        gameObject.GetComponent<SpriteRenderer>().sprite = InputPromptManager.Instance.getSpriteFromIndex(controller, choiceIndex);
        gameObject.transform.localScale = new Vector3(size, size, size);
    } 

}
