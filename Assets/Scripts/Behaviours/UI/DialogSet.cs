using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "DialogSet", menuName = "DialogSet", order = 0)]
public class DialogSet : ScriptableObject
{
    public Sprite portrait;
    public List<string> dialogs;
}
