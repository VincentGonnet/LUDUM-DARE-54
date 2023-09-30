using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownManager : Singleton<CooldownManager>
{
    private Dictionary<string, float> cooldownDefinitions = new Dictionary<string, float>();
    private Dictionary<string, float> cooldowns = new Dictionary<string, float>();
    public void InitScriptableObjects()
    {
        Skill[] skills = Resources.LoadAll<Skill>("Skills");
        foreach (Skill skill in skills)
        {
            cooldownDefinitions.Add(skill.name, skill.cooldown); // Value of cooldowns won't change
            cooldowns.Add(skill.name, skill.cooldown); // Will keep track of cooldowns
        }
    }

    public float GetCooldown(string skillName)
    {
        return cooldowns[skillName];
    }

    public bool isOnCooldown(string skillName)
    {
        return cooldowns[skillName] < cooldownDefinitions[skillName];
    }

    public void StartCooldown(string skillName)
    {
        StartCoroutine(CooldownCoroutine(skillName));
    }

    private IEnumerator CooldownCoroutine(string skillName)
    {
        while (cooldowns[skillName] > 0)
        {
            cooldowns[skillName] -= Time.deltaTime;
            yield return null;
        }
        cooldowns[skillName] = cooldownDefinitions[skillName];
    }


}
