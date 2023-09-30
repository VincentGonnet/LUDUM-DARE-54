using UnityEngine;

public class Garage : MonoBehaviour {
    public GameObject SkillSelector;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            SkillSelector.SetActive(true);
        }
    }
}