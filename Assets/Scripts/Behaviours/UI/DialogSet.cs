using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogSet", menuName = "DialogSet", order = 0)]
public class DialogSet : ScriptableObject
{
    public Sprite portrait;
    public List<string> dialogs;
}
