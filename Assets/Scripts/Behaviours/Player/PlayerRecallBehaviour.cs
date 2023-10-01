using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRecallBehaviour : MonoBehaviour
{

    [Header("Component References")]
    public Rigidbody2D playerRigidbody;

    [Header("Recall Settings")]
    public GameObject player;
    private Vector3 checkpointPosition;
    private float waitSystem;

    [Header("Timer Settings")]
    [SerializeField] private float recallTime = 2f;
    [SerializeField] Image timerImage;

    public void SetupBehaviour()
    {
        // Potentially setup any other components here
        // Is called when the player is setup in the GameManager
        player = GameObject.FindWithTag("Player");
        SetCurrentCheckpoint(player.transform.position);
        timerImage.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Garage")){
            SetCurrentCheckpoint(other.transform.position);
        }
    }

    void SetCurrentCheckpoint(Vector3 position){
        this.checkpointPosition = position;
    }

    public Vector3 GetCurrentCheckpoint(){
        return this.checkpointPosition;
    }


    public void StartRecall(){
        timerImage.gameObject.SetActive(true);
        player.GetComponent<PlayerController>().setIsRecalling(true);
        player.GetComponent<PlayerMovementBehaviour>().ToggleMovement();
        player.GetComponent<PlayerController>().SetSpriteDirection(new Vector2(-1,-1)); 
        StartCoroutine(WaitRecall(recallTime));
    }
    
    IEnumerator WaitRecall(float seconds){
        waitSystem = seconds;
        while(waitSystem > 0){
            if(player.GetComponent<PlayerController>().isAttackedWhileRecall){
                timerImage.gameObject.SetActive(false);
                StopAllCoroutines();
                player.GetComponent<PlayerMovementBehaviour>().ToggleMovement();
                player.GetComponent<PlayerController>().setIsRecalling(false);
                player.GetComponent<PlayerController>().setIsAttackedWhileRecall(false);
            }
            timerImage.fillAmount = waitSystem / seconds;
            waitSystem -= Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(Recall());
        
    }

    IEnumerator Recall(){
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        player.transform.position = GetCurrentCheckpoint();
        player.GetComponent<PlayerMovementBehaviour>().ToggleMovement();
        player.GetComponent<PlayerController>().setIsRecalling(false);
        yield return 0;
    }
}
