using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    private GameObject cooldownElement;


    // Start is called before the first frame update
    void Start()
    {
        cooldownElement = this.transform.GetChild(2).gameObject;
        cooldownElement.GetComponent<Image>().fillAmount = 0;
    }

    // Start Colwndown
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
    }
}
