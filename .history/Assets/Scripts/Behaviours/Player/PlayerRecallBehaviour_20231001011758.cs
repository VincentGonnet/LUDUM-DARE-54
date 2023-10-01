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


    public void Recall(){
        Debug.Log("Recall");
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        player.transform.position = GetCurrentCheckpoint();
    }
}
