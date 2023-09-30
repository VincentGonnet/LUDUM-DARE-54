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

    void MoveThePlayer()
    {
        Vector3 movement = canMove ? movementDirection * movementSpeed * Time.deltaTime : Vector3.zero;
        playerRigidbody.MovePosition(transform.position + movement);
    }

    public void ToggleMovement()
    {
        canMove = !canMove;
    }

    public void Slide1UnitToward(Vector3 targetPosition)
    {
        // Determine if the player should move horizontally or vertically
        float xDiff = Math.Abs(targetPosition.x - transform.position.x);
        float yDiff = Math.Abs(targetPosition.y - transform.position.y);

        Vector3 movementDirection = (targetPosition - transform.position).normalized;
        float newXDirection = movementDirection.x > 0 
            ? (float) Math.Ceiling(transform.position.x + movementDirection.x)
            : (float) Math.Floor(transform.position.x + movementDirection.x);

        float newYDirection = movementDirection.y > 0
            ? (float) Math.Ceiling(transform.position.y + movementDirection.y)
            : (float) Math.Floor(transform.position.y + movementDirection.y);

        targetPosition = new Vector3(newXDirection, newYDirection, -5f);

        if(xDiff > yDiff)
        {
            targetPosition.y = transform.position.y;
        }
        else
        {
            targetPosition.x = transform.position.x;
        }

        StartCoroutine(Slide1UnitTowardCoroutine(targetPosition));
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
