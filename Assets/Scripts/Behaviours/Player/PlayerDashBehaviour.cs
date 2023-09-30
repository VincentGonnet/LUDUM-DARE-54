using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashBehaviour : MonoBehaviour
{

    [Header("Component References")]
    public Rigidbody2D playerRigidbody;

    [Header("Dash Settings")]
    public float dashDistance = 20f;

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

    void FixedUpdate()
    {
        Dash();
    }

    public void Dash()
    {
        Vector3 dash = dashDirection * dashDistance * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + dash);
    }
}
