using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiTooltipMaterial : MonoBehaviour
{
    public int characterWrapLimit;
    private void Update() {
        if (Application.isEditor) {
            int titleLength = transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Length;
            int descriptionLength = transform.GetChild(1).GetComponent<TextMeshProUGUI>().text.Length;

            GetComponent<LayoutElement>().enabled = titleLength > characterWrapLimit || descriptionLength > characterWrapLimit;
        }

        transform.position = Input.mousePosition + new Vector3(10, -20);
    }

    public void SetText(string content, string title) {
        if (string.IsNullOrEmpty(title)) transform.GetChild(0).gameObject.SetActive(false);
        else {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(title);
        }
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(content);
    }
}