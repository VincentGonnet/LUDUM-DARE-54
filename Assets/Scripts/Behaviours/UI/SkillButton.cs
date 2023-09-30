using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerClickHandler {

    public SkillType skill;
    public PlayerProperties playerInventory;

    private KnowledgeInstaller Installer {
        get {
            return transform.parent.parent.parent.GetComponent<KnowledgeInstaller>();
        }
    }
    private GameObject Mask {
        get {
            return transform.GetChild(1).gameObject;
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        // toggle in player inventory
        if (playerInventory.Can(skill)) {
            playerInventory.RemoveSkill(skill);
            Mask.SetActive(true);
        }
        else Mask.SetActive(!playerInventory.EquipSkill(skill));

        // Update memory
        Installer.SetMemoryData(Mathf.RoundToInt(playerInventory.currentMemory), Mathf.RoundToInt(playerInventory.maxMemory));
    }
}