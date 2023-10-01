using UnityEngine;

public class SkillItem : MonoBehaviour {
    public Skill skill;

    private void Awake() {
        GetComponent<SpriteRenderer>().sprite = skill.sprite;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            // Tutorial check
            if (!GameManager.Instance.tutorialManager.hasSeenDashTutorial && skill.type == SkillType.Dash) {
                GameManager.Instance.tutorialManager.DisplayDashTutorial();
            } else if (!GameManager.Instance.tutorialManager.hasSeenAttackTutorial && skill.type == SkillType.Attack) {
                GameManager.Instance.tutorialManager.DisplayAttackTutorial();
            }

            other.GetComponent<PlayerProperties>().PushToBackpack(skill);
            Destroy(transform.parent.gameObject);
        }
    }
}