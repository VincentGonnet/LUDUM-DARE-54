using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPromptManager : Singleton<InputPromptManager>
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public Sprite getSpriteFromIndex(Controller controller, int index)
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
