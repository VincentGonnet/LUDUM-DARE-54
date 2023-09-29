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

}
