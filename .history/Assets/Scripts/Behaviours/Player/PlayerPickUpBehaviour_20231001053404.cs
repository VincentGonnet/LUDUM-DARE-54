using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpBehaviour : MonoBehaviour
{

    private bool isPickUpTrash = false;
    public void SetupBehaviour()
    {
        // Potentially setup any other components here
        // Is called when the player is setup in the GameManager
    }


    public void PickUp(){

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("TrashPickUp")){

        }
    }
}
