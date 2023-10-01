using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecallBehaviour : MonoBehaviour
{

    [Header("Component References")]
    public Rigidbody2D playerRigidbody;

    [Header("Recall Settings")]
    public GameObject player;
    private Vector3 checkpointPosition;
    private float waitSystem;

    public void SetupBehaviour()
    {
        // Potentially setup any other components here
        // Is called when the player is setup in the GameManager
        player = GameObject.FindWithTag("Player");
        SetCurrentCheckpoint(player.transform.position);
    }

    // TODO: Fix the trigger
    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Garage")){
            Debug.Log("In garage");
            SetCurrentCheckpoint(other.transform.position);
        }
    }

    void SetCurrentCheckpoint(Vector3 position){
        this.checkpointPosition = position;
    }

    Vector3 GetCurrentCheckpoint(){
        return this.checkpointPosition;
    }


    public void StartRecall(){
        Debug.Log("Recall");
        player.GetComponent<PlayerController>().setIsRecalling(true);
        player.GetComponent<PlayerMovementBehaviour>().ToggleMovement();
        StartCoroutine(WaitRecall(2f));
    }

    IEnumerator WaitRecall(float seconds){
        waitSystem = seconds;
        while(waitSystem > 0){
            if(player.GetComponent<PlayerController>().isAttackedWhileRecall){
                Debug.Log("Stop recall");
                StopAllCoroutines();
                player.GetComponent<PlayerMovementBehaviour>().ToggleMovement();
                player.GetComponent<PlayerController>().setIsAttackedWhileRecall(false);
            }
            waitSystem -= Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(Recall());
        
    }

    IEnumerator Recall(){
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        player.transform.position = GetCurrentCheckpoint();
        player.GetComponent<PlayerMovementBehaviour>().ToggleMovement();
        yield return 0;
    }
}
