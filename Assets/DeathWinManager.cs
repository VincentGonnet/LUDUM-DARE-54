using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathWinManager : MonoBehaviour
{
    public GameObject player;
    private GameObject logo;
    private GameObject title;
    private GameObject message;
    public float duration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0f);

        logo = this.transform.GetChild(0).gameObject;
        title = this.transform.GetChild(1).gameObject;
        message = this.transform.GetChild(2).gameObject;

        logo.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
        message.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(255f, 255f, 255f, 0f);
        title.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(255f, 255f, 255f, 0f);

        player.GetComponent<PlayerController>().EnablePauseMenuControls();
    }

    public void StartOverlay(){
        // Start FadeIn Animation
        this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0f);
        
        // Start FadeIn Coroutine
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn(){
        float elapsedTime = 0f;
        while (elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        // Show Logo
        logo.SetActive(true);
        logo.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
        StartCoroutine(FadeInLogo());
    }

    IEnumerator FadeInLogo(){
        float elapsedTime = 0f;
        while (elapsedTime < (duration/2)){
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / (duration/2));
            logo.GetComponent<Image>().color = new Color(255f, 255f, 255f, alpha);
            yield return null;
        }

        // Show Title
        title.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(255f, 255f, 255f, 1f);
        StartCoroutine(this.GetComponent<TypeText>().Type("You are dead!", title.GetComponent<TMPro.TextMeshProUGUI>(), 20f, () => StartCoroutine(FadeInMessage())));
    }

    IEnumerator FadeInMessage(){
        yield return new WaitForSeconds(0.6f);
        message.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(255f, 255f, 255f, 1f);
        StartCoroutine(this.GetComponent<TypeText>().Type("System reset..............................OK", message.GetComponent<TMPro.TextMeshProUGUI>(), 20f, () => StartCoroutine(FadeInMessage2())));
    }

    IEnumerator FadeInMessage2(){
        yield return new WaitForSeconds(1f);
        StartCoroutine(this.GetComponent<TypeText>().Type("Repairing vital functions.............OK", message.GetComponent<TMPro.TextMeshProUGUI>(), 20f, () => StartCoroutine(FadeInMessage3())));
    }

    IEnumerator FadeInMessage3(){
        yield return new WaitForSeconds(1f);
        StartCoroutine(this.GetComponent<TypeText>().Type("Connection to the last garage....OK", message.GetComponent<TMPro.TextMeshProUGUI>(), 20f, () => StartCoroutine(FadeInMessage4())));
    }
    
    IEnumerator FadeInMessage4(){
        yield return new WaitForSeconds(1f);
        StartCoroutine(this.GetComponent<TypeText>().Type("Redeployment............................OK", message.GetComponent<TMPro.TextMeshProUGUI>(), 20f, () => StartCoroutine(FadeInMessage5())));
    }

    IEnumerator FadeInMessage5(){
        yield return new WaitForSeconds(1f);
        StartCoroutine(this.GetComponent<TypeText>().Type("Welcome back, pilot!", message.GetComponent<TMPro.TextMeshProUGUI>(), 20f, () => StartCoroutine(FadeOut())));
    }

    IEnumerator FadeOut(){
        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeOutLogo());
        StartCoroutine(FadeOutTitle());
        StartCoroutine(FadeOutMessage());
        StartCoroutine(FadeOutOverlay());
    }

    IEnumerator FadeOutLogo(){
        float elapsedTime = 0f;
        while (elapsedTime < (duration/2)){
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / (duration/2));
            logo.GetComponent<Image>().color = new Color(255f, 255f, 255f, alpha);
            yield return null;
        }

        // Start FadeOut Animation
        this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1f);
        
        // Start FadeOut Coroutine
        StartCoroutine(FadeOutOverlay());
    }

    IEnumerator FadeOutTitle(){
        float elapsedTime = 0f;
        while (elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            title.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(255f, 255f, 255f, alpha);
            yield return null;
        }
    }

    IEnumerator FadeOutMessage(){
        float elapsedTime = 0f;
        while (elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            message.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(255f, 255f, 255f, alpha);
            yield return null;
        }
    }

    IEnumerator FadeOutOverlay(){
        yield return new WaitForSeconds(0.4f);
        float elapsedTime = 0f;
        while (elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        player.GetComponent<PlayerController>().EnableGameplayControls();

        player.GetComponent<PlayerProperties>().isDead = false;
    }
}
