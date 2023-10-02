using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBehaviour : MonoBehaviour
{
    public bool canMove = true;

    [Header("Component References")]
    public Rigidbody2D playerRigidbody;

    [Header("Movement Settings")]
    public float movementSpeed = 3f;

    //Stored Values
    private Vector3 movementDirection;


    public void SetupBehaviour()
    {
        // Potentially setup any other components here
        // Is called when the player is setup in the GameManager
    }

    public void UpdateMovementData(Vector3 newMovementDirection)
    {
        movementDirection = newMovementDirection;
    }

    void FixedUpdate()
    {
        MoveThePlayer();
    }

    public void MoveThePlayer()
    {
        Vector3 movement = canMove ? movementDirection * movementSpeed * Time.deltaTime : Vector3.zero;
        if (movement != Vector3.zero) {
            playerRigidbody.MovePosition(transform.position + movement);
            GetComponent<PlayerController>().SetSpriteDirection(movement);
        }
    }

    public void ToggleMovement()
    {
        canMove = !canMove;
    }

    public void Slide1UnitToward(Vector3 triggerPosition, Vector3 movementDirection)
    {
        
        movementDirection.Normalize();
        Debug.Log("Movement Direction: " + movementDirection);

        if (movementDirection.x != 0 && movementDirection.y != 0)
        {
            // TODO : we could calculate which axis is closer to the trigger and slide that way
            // but it's overkill for now
        } else {
            StartCoroutine(Slide1UnitTowardCoroutine(transform.position + (movementDirection*2)));
        }

    }

    private IEnumerator Slide1UnitTowardCoroutine(Vector3 targetPosition)
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        float timeToReachTarget = distanceToTarget / movementSpeed;

        float elapsedTime = 0f;

        while (elapsedTime < timeToReachTarget)
        {
            // Calculate the interpolation factor (0 to 1) based on elapsed time and timeToReachTarget.
            float t = elapsedTime / timeToReachTarget;

            // Interpolate the player's position between the current position and the target position.
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, t);
            newPosition.z = -5f;

            // Move the player to the new position.
            gameObject.transform.position = newPosition;

            // Increment elapsed time by the time that has passed since the last frame.
            elapsedTime += Time.deltaTime;

            // Yielding null here allows the Coroutine to continue in the next frame.
            yield return null;
        }
        Vector3 finalPosition = targetPosition;
        finalPosition.z = -5f;

        // Ensure the player reaches the exact target position.
        gameObject.transform.position = targetPosition;

    }
}
