using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Canvas dialogCanvas;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Image portrait;
    [SerializeField] private float charactersPerSecond = 10f;
    
    // Stored Values
    public bool isInDialog = false;
    public int dialogId = -1;
    public int dialogLine = -1;
    public bool isTyping = false;

    void Start()
    {
        Debug.Log("TutorialManager.Start");
        Debug.Log("TutorialManager.Start: dialogUI = " + dialogCanvas.gameObject.name);
        dialogCanvas.enabled = false;
        dialogText.text = "hey";  
        StartCoroutine(waitThePlay());
    }

    public void OnSubmit(InputAction.CallbackContext value)
    {
        if(value.started && isInDialog)
        {
            NextLine();
        }
    } 

    private IEnumerator waitThePlay()
    {
        yield return new WaitForSeconds(2);
        PlayTutorial(1);
    }

    public void PlayTutorial(int dialogId)
    {
        if (!isInDialog)
        {
            isInDialog = true;
            dialogCanvas.enabled = true;
        }

        GameManager.Instance.ToggleDialogControls(playerController, true);

        string dialogPath = "Dialogs/Tutorial/Step" + dialogId;
        Debug.Log("TutorialManager.PlayTutorial: dialogPath = " + dialogPath);

        DialogSet dialogSet = Resources.Load<DialogSet>(dialogPath);
        Debug.Log("TutorialManager.PlayTutorial: dialogSet = " + dialogSet.name);

        portrait.sprite = dialogSet.portrait;
        this.dialogId = dialogId;
        dialogLine = 0;
        isTyping = true;
        StartCoroutine(TypeText(dialogSet.dialogs[0]));
    }

    private IEnumerator TypeText(string line)
    {
        string textBuffer = null;
        foreach (char c in line)
        {
            if (!isTyping)
            {
                dialogText.text = line;
                yield break;
            }
            textBuffer += c;
            dialogText.text = textBuffer;
            yield return new WaitForSeconds(1 / charactersPerSecond);
        }
        isTyping = false;
    }

    public void NextLine()
    {
        if (!isInDialog) return;
        if (isTyping)
        {
            isTyping = false;
            return;
        }
        dialogLine++;
        string dialogPath = "Dialogs/Tutorial/Step" + dialogId;
        DialogSet dialogSet = Resources.Load<DialogSet>(dialogPath);
        if (dialogLine < dialogSet.dialogs.Count)
        {
            isTyping = true;
            StartCoroutine(TypeText(dialogSet.dialogs[dialogLine]));
        }
        else
        {
            dialogCanvas.enabled = false;
            this.dialogId = -1;
            this.dialogLine = -1;
            isInDialog = false;
            GameManager.Instance.ToggleDialogControls(playerController, false);
        }
    }
}
