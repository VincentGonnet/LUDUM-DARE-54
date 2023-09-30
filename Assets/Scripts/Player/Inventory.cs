using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // List of available skills to equip
    private List<Skill> backpack = new List<Skill>();

    // List of skills currently equipped
    private List<Skill> equippedSkills = new List<Skill>();

    // Max weight the player can carry
    [SerializeField] public float maxWeight = 10f;

    // Current weight the player is carrying
    private float currentWeight = 0f;

    // Add a skill to the backpack
    public void EquipSkill(int skillIndexInBackpack)
    {
        if (currentWeight + backpack[skillIndexInBackpack].weight > maxWeight)
        {
            Debug.Log("Not enough weight to equip skill");
            return;
        }

        // Add the skill to the equipped list
        equippedSkills.Add(backpack[skillIndexInBackpack]);

        // Remove the skill from the backpack
        backpack.RemoveAt(skillIndexInBackpack);

        // Update the current weight
        currentWeight += equippedSkills[equippedSkills.Count - 1].weight;
    }

    // Remove a skill from the backpack
    public void RemoveSkill(int skillIndexInEquipped)
    {
        // Add the skill to the backpack
        backpack.Add(equippedSkills[skillIndexInEquipped]);

        // Remove the skill from the equipped list
        equippedSkills.RemoveAt(skillIndexInEquipped);

        // Update the current weight
        currentWeight -= backpack[backpack.Count - 1].weight;
    }

    public bool Can(SkillType skillType) {
        foreach (Skill skill in equippedSkills) {
            if (skill.type == skillType) {
                return true;
            }
        }
        return false;
    }

    public float GetSkillLevel(SkillType skillType) {
        float level = 0f;
        foreach (Skill skill in equippedSkills) {
            if (skill.type == skillType) {
                level += skill.level;
            }
        }
        return level;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
