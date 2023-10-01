using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecallBehaviour : MonoBehaviour
{

    [Header("Component References")]
    public Rigidbody2D playerRigidbody;

    [Header("Recall Settings")]
    public GameObject player;
    private Vector2 checkpointPosition;
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

    void SetCurrentCheckpoint(Vector2 position){
        this.checkpointPosition = position;
    }

    Vector2 GetCurrentCheckpoint(){
        return this.checkpointPosition;
    }


    public void StartRecall(){
        Debug.Log("Recall");
        StartCoroutine(WaitRecall(2f))
    }

    IEnumerator WaitRecall(float seconds){
        waitSystem = seconds;
        while(waitSystem > 0){
            if(player.GetComponent<PlayerController>().isAttacked){
                StopCoroutine();
            }
            waitSystem -= Time.deltaTime;
            yield return StartCoroutine(WaitRecall(waitSystem));
        }
        yield return StartCoroutine(Recall());
        
    }

    IEnumerator Recall(){
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        player.transform.position = GetCurrentCheckpoint();
        yield return 0;
    }
}
