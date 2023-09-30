using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Movement,
    Attack,
    Jump,
    Dash,
    UIHealth,
    UIMyopia,
}

[CreateAssetMenu(fileName = "Skill", menuName = "Skill", order = 0)]
public class Skill : ScriptableObject
{
    public string title = "";
    public string description = "";
    public float memory = 3f;
    public Sprite sprite = null;
    public float level = 1f;
    public SkillType type = SkillType.Attack;
    public float cooldown = 0f;
}
