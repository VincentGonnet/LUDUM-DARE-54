using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public Sprite circleSprite;
    private GameObject cooldownElement;
    private SkillType skillType;
    private bool isOnCooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        cooldownElement = this.transform.GetChild(2).gameObject;
        cooldownElement.GetComponent<Image>().fillAmount = 0;

        Clear(); 
    }

    public void Clear() {

        this.transform.GetChild(1).GetComponent<Image>().sprite = circleSprite;
        StopAllCoroutines();
        isOnCooldown = false;
        cooldownElement.GetComponent<Image>().fillAmount = 0;
    }

    public void SetSkill(Skill skill) {
        this.transform.GetChild(1).GetComponent<Image>().sprite = skill.sprite;
        skillType = skill.type;
        cooldownElement.GetComponent<Image>().fillAmount = 0;
    }

    void Update() {
        if(CooldownManager.Instance.IsOnCooldown(skillType) && !isOnCooldown) {
            isOnCooldown = true;
            StartCooldown(CooldownManager.Instance.GetCooldown(skillType));
        }
    }


    // Start Cooldown
    public void StartCooldown(float cooldownTime)
    {
        StartCoroutine(Cooldown(cooldownTime));
    }

    // Cooldown
    IEnumerator Cooldown(float cooldownTime)
    {
        cooldownElement.GetComponent<Image>().fillAmount = 1;
        float time = 0;
        while (time < cooldownTime)
        {
            time += Time.deltaTime;
            cooldownElement.GetComponent<Image>().fillAmount = 1 - time / cooldownTime;
            yield return null;
        }
        cooldownElement.GetComponent<Image>().fillAmount = 0;
        isOnCooldown = false;
    }
}
