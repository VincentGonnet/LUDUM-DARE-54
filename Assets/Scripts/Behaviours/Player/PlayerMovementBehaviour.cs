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
    // [SerializeField] private float lerpStop = 0.5f;

    //Stored Values
    private Vector3 movementDirection;
    private bool isSliding = false;


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
        if (movement != Vector3.zero)
        {
            playerRigidbody.MovePosition(transform.position + movement);
            GetComponent<PlayerController>().SetSpriteDirection(movement);
        }
    }

    public void ToggleMovement()
    {
        canMove = !canMove;
    }

    public void ChangeZone(Vector3 movementDirection, float unitsToSlide, GameObject enteringZoneTrigger)
    {
        if (isSliding) return;
        Vector3 position = transform.position;
        Vector3 newPosition = position + movementDirection * unitsToSlide;

        // Start camera transition
        CameraFollowPlayer cameraFollowPlayer = CameraManager.Instance.gameplayCameraObject.GetComponent<CameraFollowPlayer>();
        cameraFollowPlayer.StartCameraTransition(newPosition, enteringZoneTrigger);
        StartCoroutine(SlideTowardCoroutine(newPosition));
    }

    private IEnumerator SlideTowardCoroutine(Vector3 targetPosition)
    {
        isSliding = true;


        float journeyLength = Vector3.Distance(transform.position, targetPosition);
        float startTime = Time.time;

        while (transform.position != targetPosition)
        {
            float distanceCovered = (Time.time - startTime) * movementSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, targetPosition, fractionOfJourney);

            yield return null;
        }

        isSliding = false;
    }
}
