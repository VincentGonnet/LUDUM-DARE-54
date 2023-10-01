using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeText : MonoBehaviour
{
    public IEnumerator Type(string line, TextMeshProUGUI textMeshProUGUI, float charactersPerSecond, System.Action callback)
    {
        string textBuffer = null;
        foreach (char c in line)
        {
            textBuffer += c;
            textMeshProUGUI.text = textBuffer;
            yield return new WaitForSeconds(1 / charactersPerSecond);
        }
        callback();
    }
}
