using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecallBehaviour : MonoBehaviour
{

    [Header("Component References")]
    public Rigidbody2D playerRigidbody;

    [Header("Recall Settings")]
    public float recallDistance = 3f;

    public void SetupBehaviour()
    {
        // Potentially setup any other components here
        // Is called when the player is setup in the GameManager
    }

    // public void UpdateAttackData(Vector3 newMovementDirection)
    // {
    //     attackDirection = newAttackDirection;
    // }

    public void Recall()
    {
        // TODO: add recall code here
        Debug.Log("Recall");
    }
}
