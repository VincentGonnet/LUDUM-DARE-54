using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using System.Runtime.CompilerServices;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Canvas dialogCanvas;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Image portrait;
    [SerializeField] private float charactersPerSecond = 20f;

    public int tutorialStep = 0;
    
    // Stored Values
    public bool isInDialog = false;
    public int dialogId = -1;
    public int dialogLine = -1;
    public bool isTyping = false;

    void Start()
    {
        DisableCanvas();
    }

    public void DisableCanvas() {
        dialogCanvas.enabled = false;
    }

    public void OnSubmit(InputAction.CallbackContext value)
    {
        if(value.started && isInDialog)
        {
            NextLine();
        }
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
        DialogSet dialogSet = Resources.Load<DialogSet>(dialogPath);

        portrait.sprite = dialogSet.portrait;
        this.dialogId = dialogId;
        dialogLine = 0;
        isTyping = true;
        StartCoroutine(TypeText(dialogSet.dialogs[0], TypeCallback));
    }

    private void TypeCallback()
    {
        isTyping = false;
    }
    private IEnumerator TypeText(string line, System.Action callback)
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
        callback();
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
            StartCoroutine(TypeText(dialogSet.dialogs[dialogLine], TypeCallback));
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
