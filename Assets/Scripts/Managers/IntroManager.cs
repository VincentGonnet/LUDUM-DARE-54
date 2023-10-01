using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroScript : MonoBehaviour
{
    public GameObject logo;
    private float duration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        // diplay size and logo alpha animation
        StartCoroutine(LogoAnimation());
    }

    void Update()
    {
        if(Input.anyKeyDown)
            GoToMenu();
    }

    public IEnumerator LogoAnimation(){
        float elapsedTime = 0f;
        while (elapsedTime < duration){
            if(elapsedTime > (duration / 2))
                GoToMenu();
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Min(Mathf.Lerp(0f, 1f, elapsedTime / (duration / 2)), 1f);
            logo.GetComponent<Image>().color = new Color(255f, 255f, 255f, alpha);

            float scale = Mathf.SmoothStep(0.75f, 1.25f, elapsedTime / (duration / 2));
            logo.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }
    }

    public void GoToMenu(){
        SceneManager.LoadScene("MainMenu");
    }
}
