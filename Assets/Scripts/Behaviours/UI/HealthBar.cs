using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UpdateHealth(GameObject.Find("Player").GetComponent<PlayerProperties>().health / GameObject.Find("Player").GetComponent<PlayerProperties>().maxHealth);
    }

    public void UpdateHealth(float healthPercentage)
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.localScale = new Vector3(healthPercentage, 1, 1);
    }
}
