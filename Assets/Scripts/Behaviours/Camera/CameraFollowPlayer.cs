using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float slidingSpeed = 2f;
    private bool isCameraSliding = false;

    [Header("Debug")]
    [SerializeField] private bool touchesBottom;
    [SerializeField] private bool touchesTop;
    [SerializeField] private bool touchesLeft;
    [SerializeField] private bool touchesRight;

    void FixedUpdate()
    {
        if (isCameraSliding) return; // We don't want the camera to follow the player during zone transitions

        GameObject currentZoneTrigger = player.GetComponent<PlayerController>().GetCurrentZoneTrigger();
        if (currentZoneTrigger == null)
        {
            transform.position = player.transform.position;
        }
        else
        {
            transform.position = GetNextCameraPosition(player.transform.position, currentZoneTrigger);
        }
    }

    public void StartCameraTransition(Vector3 endPosition, GameObject zoneTrigger)
    {
        isCameraSliding = true;
        Vector3 targetPosition = GetNextCameraPosition(endPosition, zoneTrigger, true);
        StartCoroutine(CameraTransitionCoroutine(transform.position, targetPosition));
    }

    private IEnumerator CameraTransitionCoroutine(Vector3 startPosition, Vector3 endPosition)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * slidingSpeed;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }
        isCameraSliding = false;
    }

    public Vector3 GetNextCameraPosition(Vector3 playerPos, GameObject zoneTrigger, bool isZoneTransition = false)
    {
        // Get the center of the zone trigger
        float currentZoneTriggerX = zoneTrigger.transform.position.x;
        float currentZoneTriggerY = zoneTrigger.transform.position.y;

        // Get the boundaries of the zone trigger
        float rightZoneBoundary = currentZoneTriggerX + zoneTrigger.GetComponent<BoxCollider2D>().size.x / 2;
        float leftZoneBoundary = currentZoneTriggerX - zoneTrigger.GetComponent<BoxCollider2D>().size.x / 2;
        float upperZoneBoundary = currentZoneTriggerY + zoneTrigger.GetComponent<BoxCollider2D>().size.y / 2;
        float lowerZoneBoundary = currentZoneTriggerY - zoneTrigger.GetComponent<BoxCollider2D>().size.y / 2;

        float playerX = playerPos.x;
        float playerY = playerPos.y;

        // Fix the screen width and height variables to avoid camera stuttering
        float screenHeigth = Camera.main.orthographicSize;
        float screenWidth = screenHeigth * Camera.main.aspect;

        // Get the bounds of the camera's view
        float upperCamBoundary = transform.position.y + screenHeigth;
        float lowerCamBoundary = transform.position.y - screenHeigth;
        float rightCamBoundary = transform.position.x + screenWidth;
        float leftCamBoundary = transform.position.x - screenWidth;

        // Check if the camera is touching the zone trigger's boundaries
        touchesRight = rightCamBoundary >= rightZoneBoundary;
        touchesLeft = leftCamBoundary <= leftZoneBoundary;
        touchesTop = upperCamBoundary >= upperZoneBoundary;
        touchesBottom = lowerCamBoundary <= lowerZoneBoundary;

        // Calculate the new camera position
        Vector3 newCameraPosition = transform.position;

        if (touchesLeft && touchesRight)
        {
            newCameraPosition.x = transform.position.x;
        }
        else if (touchesLeft)
        {
            newCameraPosition.x = Math.Max(playerX, leftZoneBoundary + screenWidth);
        }
        else if (touchesRight)
        {
            newCameraPosition.x = Math.Min(playerX, rightZoneBoundary - screenWidth);
        }
        else if (!touchesLeft && !touchesRight)
        {
            newCameraPosition.x = playerX;
        }

        if (touchesTop && touchesBottom)
        {
            newCameraPosition.y = transform.position.y;
        }
        else if (touchesTop)
        {
            newCameraPosition.y = Math.Min(playerY, upperZoneBoundary - screenHeigth);
        }
        else if (touchesBottom)
        {
            newCameraPosition.y = Math.Max(playerY, lowerZoneBoundary + screenHeigth);
        }
        else if (!touchesTop && !touchesBottom)
        {
            newCameraPosition.y = playerY;
        }

        // When in zone transition, we want to make sure that the future position will be centered on the zone trigger
        if (isZoneTransition)
        {
            upperCamBoundary = newCameraPosition.y + screenHeigth;
            lowerCamBoundary = newCameraPosition.y - screenHeigth;
            rightCamBoundary = newCameraPosition.x + screenWidth;
            leftCamBoundary = newCameraPosition.x - screenWidth;

            touchesRight = rightCamBoundary >= rightZoneBoundary;
            touchesLeft = leftCamBoundary <= leftZoneBoundary;
            touchesTop = upperCamBoundary >= upperZoneBoundary;
            touchesBottom = lowerCamBoundary <= lowerZoneBoundary;

            if (touchesLeft && touchesRight)
            {
                Debug.Log("Touches left and right");
                Debug.Log("CameraX: " + newCameraPosition.x);
                newCameraPosition.x = currentZoneTriggerX;
            }
            if (touchesTop && touchesBottom)
            {
                Debug.Log("Touches top and bottom");
                Debug.Log("CameraY: " + newCameraPosition.y);
                newCameraPosition.y = currentZoneTriggerY;
            }
        }

        newCameraPosition.z = -10f;

        return newCameraPosition;
    }

    public void ZoomIn(bool zoomIn)
    {

        StartCoroutine(ZoomInCoroutine(zoomIn));

    }

    IEnumerator ZoomInCoroutine(bool zoomIn)
    {

        float duration = 0.5f;
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            if (zoomIn)
            {
                Camera.main.orthographicSize = Mathf.SmoothStep(2f, 5f, t / duration);
            }
            else
            {
                Camera.main.orthographicSize = Mathf.SmoothStep(5f, 2f, t / duration);
            }
            yield return null;
        }

    }
}
