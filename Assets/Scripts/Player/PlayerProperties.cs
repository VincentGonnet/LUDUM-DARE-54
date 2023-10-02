using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    // List of available skills to equip
    private List<Skill> backpack = new List<Skill>();

    // List of skills currently equipped
    private List<Skill> equippedSkills = new List<Skill>();

    // Max memory the player can carry
    [SerializeField] public float maxMemory = 10f;

    // Current memory the player is carrying
    public float currentMemory = 0f;

    public GameObject healthBar;

    public int currentZone = 0;

    [SerializeField] float strength = 3f;
    public float getStrength(){
        return strength;
    }
    public float health = 10f;
    public float maxHealth = 10f;
    public bool isDead = false;

    public void SetHealth(float newHealth)
    {
        if(newHealth > maxHealth) newHealth = maxHealth;
        if(newHealth <= 0) {
            if(!isDead){
                // Player is dead
                isDead = true;
                GameObject.Find("DeathWinAnim").GetComponent<DeathWinManager>().StartOverlay(false);
                newHealth = maxHealth;
                // TP to last checkpoint using StartRecall()
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
                this.transform.position = this.gameObject.GetComponent<PlayerRecallBehaviour>().GetCurrentCheckpoint();
            } else {
                newHealth = 0;
            }
        }

        health = newHealth;
        // Update UI
        healthBar.GetComponent<HealthBar>().UpdateHealth(health / maxHealth);
    }
    
    public List<Skill> skills {
        get {
            return backpack.Union(equippedSkills).Distinct().ToList();
        }
    }

    public void PushToBackpack(Skill skill) {
        backpack.Add(skill);
    }

    // The method is mandatory for use outside of this class ><
    public bool EquipSkill(SkillType backpackSkill) {
        return EquipSkill(backpack.FindIndex((sk) => sk.type == backpackSkill));
    }

    // The method is mandatory for use outside of this class ><
    public void RemoveSkill(SkillType backpackSkill) {
        RemoveSkill(equippedSkills.FindIndex((sk) => sk.type == backpackSkill));
    }

    // Add a skill to the backpack
    public bool EquipSkill(int skillIndexInBackpack)
    {
        if (skillIndexInBackpack < 0) return false;
        if (currentMemory + backpack[skillIndexInBackpack].memory > maxMemory)
        {
            Debug.Log("Not enough memory to equip skill");
            return false;
        }

        // Add the skill to the equipped list
        equippedSkills.Add(backpack[skillIndexInBackpack]);

        // Remove the skill from the backpack
        backpack.RemoveAt(skillIndexInBackpack);

        // Update the current memory
        currentMemory += equippedSkills[equippedSkills.Count - 1].memory;
        
        OnEquipSkillChange();
        return true;
    }

    // Remove a skill from the backpack
    public void RemoveSkill(int skillIndexInEquipped)
    {
        if (skillIndexInEquipped < 0) return;
        // Add the skill to the backpack
        backpack.Add(equippedSkills[skillIndexInEquipped]);

        // Remove the skill from the equipped list
        equippedSkills.RemoveAt(skillIndexInEquipped);

        // Update the current memory
        currentMemory -= backpack[backpack.Count - 1].memory;

        OnEquipSkillChange();
    }

    public void OnEquipSkillChange() {
        // Rebuild UI
        GameManager.Instance.UpdateUI();

        // Update the ActiveSkillsUI
        // ---
        // We have 8 slots to fill in the ActiveSkillsUI
        if(Can(SkillType.UIHealth)) {
            // First, we clear the UI
            for (int i = 0; i < 8; i++) {
                GameObject.Find($"SkillUI{i + 1}").GetComponent<SkillUI>().Clear();
            }

            // Then, we fill it with the equipped skills
            for (int i = 0; i < equippedSkills.Count; i++) {
                GameObject.Find($"SkillUI{i + 1}").GetComponent<SkillUI>().SetSkill(equippedSkills[i]);
            }
        }
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
        // Add Movement skill to the backpack by default using the ScriptableObject
        Skill[] skills = Resources.LoadAll<Skill>("ScriptableObjects/Skills");
        PushToBackpack(skills.First((sk) => sk.type == SkillType.Movement));

        // Equip it
        EquipSkill(0);

        PushToBackpack(skills.First((sk) => sk.type == SkillType.UIMyopia));

        // Equip it
        EquipSkill(1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
