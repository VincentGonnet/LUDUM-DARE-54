using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpBehaviour : MonoBehaviour
{

    [Header("Component References")]
    private List<Collider2D> trashList = new List<Collider2D>();
    private Dictionary<Collider2D, GameObject> trashPromptDict = new Dictionary<Collider2D, GameObject>();

    public void SetupBehaviour()
    {
        // Potentially setup any other components here
        // Is called when the player is setup in the GameManager
    }

    public void PickUp(){
        if(trashList.Count > 0){
            Collider2D trash = trashList[^1];
            trashList.Remove(trash);
            Destroy(trash.gameObject);

            Destroy(trashPromptDict[trash]);
            trashPromptDict.Remove(trash);
            GameManager.Instance.setNumberOfTrashPickedUp(GameManager.Instance.getNumberOfTrashPickedUp() + 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("TrashPickUp")){
            Debug.Log(other);
            trashList.Add(other);

            Vector3 trashPos = other.gameObject.transform.position;
            trashPos.y += 1f;
            GameObject keyPrompt = GameObject.Instantiate(Resources.Load<GameObject>("InputPrompts/F_key"), trashPos, Quaternion.identity);
            trashPromptDict.Add(other, keyPrompt);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("TrashPickUp")){
            if (trashList.Count > 0)
            {
                trashList.Remove(other);
                Destroy(trashPromptDict[other]);
                trashPromptDict.Remove(other);
            }
            
        }
    }
}
