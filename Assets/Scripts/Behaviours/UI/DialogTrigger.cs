using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private int dialogId = 0;
    [SerializeField] private bool isOneTime = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {   
            GameManager.Instance.tutorialManager.PlayTutorial(dialogId);
            if (isOneTime) Destroy(gameObject);
        }
    }
}
