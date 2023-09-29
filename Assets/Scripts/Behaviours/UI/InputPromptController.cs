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
        gameObject.GetComponent<SpriteRenderer>().sprite = getSpriteFromIndex(controller, choiceIndex);
        gameObject.transform.localScale = new Vector3(size, size, size);
    } 

    private Sprite getSpriteFromIndex(Controller controller, int index)
    {
        string spritePath = "InputPrompts/";

        switch (controller)
        {
            case Controller.KeyboardAndMouse:
                spritePath += "KeyboardAndMouse";
                break;
            case Controller.PS4:
                spritePath += "PS4";
                break;
            case Controller.Xbox:
                spritePath += "Xbox";
                break;
            case Controller.Switch:
                spritePath += "Switch";
                break;
            default:
                break;
        }

        Sprite[] sprites = Resources.LoadAll<Sprite>(spritePath);

        return sprites[index];
    }

}
