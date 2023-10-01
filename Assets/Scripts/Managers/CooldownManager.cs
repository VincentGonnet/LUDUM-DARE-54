using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownManager : Singleton<CooldownManager>
{
    private Dictionary<SkillType, float> cooldownDefinitions = new Dictionary<SkillType, float>();
    private Dictionary<SkillType, float> cooldowns = new Dictionary<SkillType, float>();
    public void InitScriptableObjects()
    {
        Skill[] skills = Resources.LoadAll<Skill>("ScriptableObjects/Skills");

        foreach (Skill skill in skills)
        {
            cooldownDefinitions.Add(skill.type, skill.cooldown); // Value of cooldowns won't change
            cooldowns.Add(skill.type, skill.cooldown); // Will keep track of cooldowns
        }
    }

    public float GetCooldown(SkillType skillType)
    {
        return cooldowns[skillType];
    }

    public bool IsOnCooldown(SkillType skillType)
    {
        return cooldowns[skillType] < cooldownDefinitions[skillType];
    }

    public void StartCooldown(SkillType skillType)
    {
        StartCoroutine(CooldownCoroutine(skillType));
    }

    private IEnumerator CooldownCoroutine(SkillType skillType)
    {
        while (cooldowns[skillType] > 0)
        {
            cooldowns[skillType] -= Time.deltaTime;
            yield return null;
        }
        cooldowns[skillType] = cooldownDefinitions[skillType];
    }


}
