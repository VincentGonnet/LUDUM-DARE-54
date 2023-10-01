using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpBehaviour : MonoBehaviour
{

    [Header("Component References")]
    private bool isPickUpTrash = false;
    private GameObject trash;
    public void SetupBehaviour()
    {
        // Potentially setup any other components here
        // Is called when the player is setup in the GameManager
    }


    public void PickUp(){
        if(isPickUpTrash){
            Destroy(trash);
            GameManager.Instance.setNumberOfTrashPickedUp(GameManager.Instance.getNumberOfTrashPickedUp() + 1);
            Debug.Log("Trash picked up");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("TrashPickUp")){
            isPickUpTrash = true;
            trash = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("TrashPickUp")){
            isPickUpTrash = false;
        }
    }
}
