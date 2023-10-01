using UnityEngine;

public class Garage : MonoBehaviour {
    public KnowledgeInstaller SkillSelector;
    public PlayerProperties playerInventory;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && other.GetComponent<PlayerController>().isPickingUp) {
            SkillSelector.Load(playerInventory);
        }
    }

}