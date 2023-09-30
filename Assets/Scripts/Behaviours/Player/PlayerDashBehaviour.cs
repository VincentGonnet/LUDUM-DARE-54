using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashBehaviour : MonoBehaviour
{

    [Header("Component References")]
    public Rigidbody2D playerRigidbody;

    [Header("Dash Settings")]
    public float dashDistance = 3f;

    //Stored Values
    private Vector3 dashDirection;


    public void SetupBehaviour()
    {
        // Potentially setup any other components here
        // Is called when the player is setup in the GameManager
    }

    public void UpdateDashData(Vector3 newDashDirection)
    {
        dashDirection = newDashDirection;
    }

    public void Dash()
    {
        Vector3 dash = dashDirection * dashDistance;

        Debug.Log("Force added : " + dash + " to " + playerRigidbody.name);

        playerRigidbody.AddForce(dash, ForceMode2D.Force);
    }
}
