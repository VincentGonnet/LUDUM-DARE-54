using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using System;
using System.Runtime.CompilerServices;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Canvas dialogCanvas;
    [SerializeField] private Canvas dashTutorialCanvas;
    [SerializeField] private Canvas attackTutorialCanvas;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Image portrait;
    [SerializeField] private float charactersPerSecond = 20f;
    public WeakReference<PlayableDirector> currentCutscene = new(null);

    public int tutorialStep = 0;
    public bool hasSeenDashTutorial = false;
    public bool hasSeenAttackTutorial = false;
    
    // Stored Values
    public bool isInDialog = false;
    public int dialogId = -1;
    public int dialogLine = -1;
    public bool isTyping = false;
    private bool isInDashTutorial = false;
    private bool isInAttackTutorial = false;

    public void DisableCanvas() {
        dialogCanvas.enabled = false;
    }

    public void OnCancel(InputAction.CallbackContext value)
    {
        if (value.started && isInDashTutorial)
        {
            HideDashTutorial();
        }
        if (value.started && isInAttackTutorial)
        {
            HidAttackTutorial();
        }
    }


    public void OnSubmit(InputAction.CallbackContext value)
    {
        if(value.started && isInDialog)
        {
            NextLine();
        }

        if (value.started && isInDashTutorial)
        {
            HideDashTutorial();
        }
        if (value.started && isInAttackTutorial)
        {
            HidAttackTutorial();
        }
    }

    public void DisplayAttackTutorial()
    {
        Debug.Log("Displaying attack tutorial");
        attackTutorialCanvas.enabled = true;
        hasSeenAttackTutorial = true;
        isInAttackTutorial = true;
        GameManager.Instance.TogglePauseState(playerController);
    }

    public void DisplayDashTutorial()
    {
        dashTutorialCanvas.enabled = true;
        isInDashTutorial = true;
        hasSeenDashTutorial = true;
        GameManager.Instance.TogglePauseState(playerController);
    }

    private void HideDashTutorial()
    {
        dashTutorialCanvas.enabled = false;
        isInDashTutorial = false;
        GameManager.Instance.TogglePauseState(playerController);
    }

    private void HidAttackTutorial()
    {
        attackTutorialCanvas.enabled = false;
        isInAttackTutorial = false;
        GameManager.Instance.TogglePauseState(playerController);
    }

    // public void GetRootPlayable(int dialogId) {

    //     string dialogPath = "Dialogs/Tutorial/Step" + dialogId;
    //     DialogSet dialogSet = Resources.Load<DialogSet>(dialogPath);
    //     dialogSet.playableGraph.GetRootPlayable(0).SetSpeed(0);

    // }

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
        string dialogPath = "Dialogs/Tutorial/Step" + dialogId;
        DialogSet dialogSet = Resources.Load<DialogSet>(dialogPath);
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

            Debug.Log("On est là");
            PlayableDirector cutscene;
            currentCutscene.TryGetTarget(out cutscene);
            Debug.Log(cutscene);
            cutscene?.Resume();
        }
    }
}
