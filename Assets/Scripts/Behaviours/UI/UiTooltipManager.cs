using UnityEngine;

public class UiTooltipManager : MonoBehaviour {
    private static UiTooltipManager instance;
    public UiTooltipMaterial tooltip;

    private void Awake() {
        instance = this;
    }

    public static void Show(string memory, string content, string header) {
        instance.tooltip.SetText(memory, content, header);
        instance.tooltip.gameObject.SetActive(true);
    }

    public static void Hide() {
        instance.tooltip.gameObject.SetActive(false);
    }
}