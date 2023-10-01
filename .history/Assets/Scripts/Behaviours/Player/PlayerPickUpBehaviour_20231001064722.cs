using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpBehaviour : MonoBehaviour
{

    [Header("Component References")]
    public Rigidbody2D playerRigidbody;
    private bool isPickUpTrash = false;
    public void SetupBehaviour()
    {
        // Potentially setup any other components here
        // Is called when the player is setup in the GameManager
    }


    public void PickUp(){
        if(isPickUpTrash){
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("TrashPickUp")){
            isPickUpTrash = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("TrashPickUp")){
            isPickUpTrash = false;
        }
    }
}
