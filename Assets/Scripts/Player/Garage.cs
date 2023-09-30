using UnityEngine;
using UnityEngine.UI;

public class Garage : MonoBehaviour {
    public GameObject SkillSelector;
    public Inventory playerInventory;
    public GameObject buttonPrefab;
    public Transform layoutParent;
    public GameManager gManager;
    public PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            for (int i = 0;  i < layoutParent.transform.childCount; i++)
                Destroy(layoutParent.transform.GetChild(layoutParent.transform.childCount - 1 - i).gameObject);
            SkillSelector.SetActive(true);
            gManager.TogglePauseState(playerController);
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