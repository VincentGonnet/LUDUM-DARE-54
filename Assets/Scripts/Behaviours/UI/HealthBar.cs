using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UpdateHealth(GameObject.Find("Player").GetComponent<PlayerProperties>().health / GameObject.Find("Player").GetComponent<PlayerProperties>().maxHealth);
    }

    public void UpdateHealth(float healthPercent)
    {
        // RectTransform rt = this.transform.GetChild(1).GetComponent<RectTransform>();
        // rt.localScale = new Vector3(healthPercent, 1, 1);

        // Remove using Fill Amount and move recttransform to the left
        this.transform.GetChild(1).GetComponent<Image>().fillAmount = healthPercent + 0.085f * (1 - healthPercent);

        RectTransform rt = this.transform.GetChild(1).GetComponent<RectTransform>();
        rt.localPosition = new Vector3(-188.3f + 274.7f * healthPercent, rt.localPosition.y, rt.localPosition.z);


        this.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = (healthPercent * 100).ToString() + "%";
    }
}