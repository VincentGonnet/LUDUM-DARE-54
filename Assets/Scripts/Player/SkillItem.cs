using UnityEngine;

public class SkillItem : MonoBehaviour {
    public Skill skill;

    private void Awake() {
        GetComponent<SpriteRenderer>().sprite = skill.sprite;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            other.GetComponent<PlayerProperties>().PushToBackpack(skill);
            Destroy(gameObject);
        }
    }
}