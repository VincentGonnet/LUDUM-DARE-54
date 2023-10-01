using UnityEngine;
using UnityEngine.EventSystems;

public class UiTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public string tooltipName;
    public string tooltipDescription;
    public string tooltipMemory;

    public void OnPointerEnter(PointerEventData eventData) {
        UiTooltipManager.Show(tooltipMemory, tooltipDescription, tooltipName);
    }

    public void OnPointerExit(PointerEventData eventData) {
        UiTooltipManager.Hide();
    }
}