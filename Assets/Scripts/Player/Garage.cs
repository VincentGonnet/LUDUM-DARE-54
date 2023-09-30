using UnityEngine;
using UnityEngine.UI;

public class Garage : MonoBehaviour {
    public GameObject SkillSelector;
    public Inventory playerInventory;
    public GameObject buttonPrefab;
    public Transform layoutParent;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            SkillSelector.SetActive(true);
            foreach (var skill in playerInventory.skills) {
                GameObject go = Instantiate(buttonPrefab, layoutParent);
                go.transform.GetChild(0).GetComponent<Image>().sprite = skill.sprite;
                go.transform.GetChild(1).gameObject.SetActive(!playerInventory.Can(skill.type));
                go.GetComponent<SkillButton>().playerInventory = playerInventory;
                go.GetComponent<SkillButton>().skill = skill.type;
                go.GetComponentInChildren<UiTooltip>().tooltipName = skill.name;
                go.GetComponentInChildren<UiTooltip>().tooltipDescription = skill.description;
            }
        }
    }
}