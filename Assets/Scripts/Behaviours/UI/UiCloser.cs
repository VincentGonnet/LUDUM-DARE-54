using UnityEngine;

public class UICloser : MonoBehaviour {
    public KeyCode ClosingKey;
    public GameManager gManager;
    public PlayerController playerController;

    private void Update() {
        if (Input.GetKeyDown(ClosingKey)) {
            gameObject.SetActive(false);
            gManager.TogglePauseState(playerController);
        }
    }
}