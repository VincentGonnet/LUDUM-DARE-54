using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerClickHandler {

    public SkillType skill;
    public PlayerProperties playerInventory;

    public void OnPointerClick(PointerEventData eventData) {
        // toggle in player inventory
        if (playerInventory.Can(skill)) {
            playerInventory.RemoveSkill(skill);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else transform.GetChild(1).gameObject.SetActive(!playerInventory.EquipSkill(skill));
    }
}