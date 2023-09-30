using System;
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

    public void Jump(Vector3 jumpDirection, ArrayList jumpPodsPressedPos)
    {
        jumpDirection.Normalize();
        // if on 8 axis, set to 4 axis
        if (jumpDirection.x != 0 && jumpDirection.y != 0)
        {
            jumpDirection.x = Mathf.Round(jumpDirection.x);
            jumpDirection.y = Mathf.Round(jumpDirection.y);
        }

        // if x and y != 0, prioritize y
        if (jumpDirection.x != 0 && jumpDirection.y != 0)
        {
            jumpDirection.x = 0;
        }

        // Calculate the pod position
        Vector3 podPosition = new Vector3(
            (float)(Math.Floor(transform.position.x) + 0.5f),
            (float)(Math.Floor(transform.position.y) + 0.5f),
            0
        );

        // Check if the pod position is in the list of pressed pods
        bool podPressed = false;
        foreach (Vector3 pod in jumpPodsPressedPos)
        {
            if (pod == podPosition)
            {
                podPressed = true;
                break;
            }
        }

        if (!podPressed) return;

        // Raycast in the jump direction, 1 unit
        RaycastHit2D foundHole = Physics2D.Raycast(podPosition, jumpDirection, 1f, LayerMask.GetMask("Hole"));

        // If the raycast hits nothing, return
        if (foundHole.collider == null) {
            Debug.Log("No hole found");
            return;
        }

        RaycastHit2D hitWall = Physics2D.Raycast(podPosition, jumpDirection, 2f, LayerMask.GetMask("Wall"));
        RaycastHit2D hitFire = Physics2D.Raycast(podPosition, jumpDirection, 2f, LayerMask.GetMask("Fire"));

        // If the raycast hits an obstacle, return

        if (hitWall.collider != null) {
            Debug.Log("Wall found");
            return;
        }
        if (hitFire.collider != null) {
            Debug.Log("Fire found");
            return;
        }

        RaycastHit2D[] holes = Physics2D.RaycastAll(podPosition, jumpDirection, 2f, LayerMask.GetMask("Hole"));

        // If hit more than 1 hole, return
        if (holes.Length > 2) { // 2 because the raycast, for an unknown reason, hits each hole twice
            Debug.Log("More than 1 hole found");
            return;
        }

        // If the raycast hits only one hole, jump

        // Calculate the jump position
        Vector3 jumpPosition = podPosition + jumpDirection * 2f;

        Debug.Log("Pod Position: " + podPosition);
        Debug.Log("Jump! " + jumpDirection);

        transform.position = jumpPosition;
    }
}
