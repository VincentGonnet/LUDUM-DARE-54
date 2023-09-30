using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerClickHandler {

    public SkillType skill;
    public Inventory playerInventory;

    public void OnPointerClick(PointerEventData eventData) {
        // toggle in player inventory
        if (playerInventory.Can(skill)) playerInventory.RemoveSkill(skill);
        else playerInventory.EquipSkill(skill);
    }
}