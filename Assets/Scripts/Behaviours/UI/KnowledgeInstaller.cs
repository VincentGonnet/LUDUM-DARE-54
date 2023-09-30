using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KnowledgeInstaller : MonoBehaviour {
    public TextMeshProUGUI slots;
    public TextMeshProUGUI maxSlots;
    public GameObject layout;
    public GameManager gManager;
    public PlayerController playerController;
    public GameObject buttonPrefab;
    public KeyCode ClosingKey;

    public void SetMemoryData(int currentUsedMemory, int currentMaxMemory) {
        slots.SetText($"{currentUsedMemory}");
        maxSlots.SetText($"{currentMaxMemory}");
        slots.color = (currentUsedMemory > currentMaxMemory * 3 / 4) ? Color.red : Color.white;
    }

    public void Load(PlayerProperties playerInventory) {
        for (int i = 0;  i < layout.transform.childCount; i++)
            Destroy(layout.transform.GetChild(layout.transform.childCount - 1 - i).gameObject);
        SetMemoryData(Mathf.RoundToInt(playerInventory.currentMemory), Mathf.RoundToInt(playerInventory.maxMemory));
        gameObject.SetActive(true);
        gManager.TogglePauseState(playerController);
        foreach (var skill in playerInventory.skills) {
            GameObject go = Instantiate(buttonPrefab, layout.transform);
            go.transform.GetChild(0).GetComponent<Image>().sprite = skill.sprite;
            go.transform.GetChild(1).gameObject.SetActive(!playerInventory.Can(skill.type));
            go.GetComponent<SkillButton>().playerInventory = playerInventory;
            go.GetComponent<SkillButton>().skill = skill.type;
            go.GetComponentInChildren<UiTooltip>().tooltipName = skill.name;
            go.GetComponentInChildren<UiTooltip>().tooltipDescription = skill.description;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(ClosingKey)) {
            gameObject.SetActive(false);
            gManager.TogglePauseState(playerController);
            UiTooltipManager.Hide();
        }
    }
}