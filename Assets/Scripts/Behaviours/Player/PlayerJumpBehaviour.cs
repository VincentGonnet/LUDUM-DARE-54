using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpBehaviour : MonoBehaviour
{

    [Header("Component References")]
    public Rigidbody2D playerRigidbody;

    [Header("Jump Settings")]
    public float jumpDistance = 3f;

    //Stored Values
    private Vector3 jumpDirection;


    public void SetupBehaviour()
    {
        // Potentially setup any other components here
        // Is called when the player is setup in the GameManager
    }

    public void UpdateJumpData(Vector3 newJumpDirection)
    {
        jumpDirection = newJumpDirection;
    }

    public void Jump()
    {
        Vector3 jump = jumpDirection * jumpDistance;
        playerRigidbody.MovePosition(transform.position + jump);
    }
}
