using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScene : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GoToMainMenu());
    }

    IEnumerator GoToMainMenu() {
        yield return new WaitForSeconds(16);
        SceneManager.LoadScene("MainMenu");
    }
}
