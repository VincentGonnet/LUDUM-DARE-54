using UnityEngine;
using UnityEngine.EventSystems;

public class UiTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public string tooltipName;
    public string tooltipDescription;

    public void OnPointerEnter(PointerEventData eventData) {
        UiTooltipManager.Show(tooltipDescription, tooltipName);
    }

    public void OnPointerExit(PointerEventData eventData) {
        UiTooltipManager.Hide();
    }
}