using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [Header("Component References")]
    public GameObject gameplayCameraObject;
    public GameObject player;

    public Camera GetGameplayCamera()
    {
        return gameplayCameraObject.GetComponent<Camera>();
    }

    public void SlideTo(GameObject targetTriggerGameObject, float slideSpeed)
    {
        float targetWidth = targetTriggerGameObject.GetComponent<BoxCollider2D>().size.x;
        float targetHeight = targetTriggerGameObject.GetComponent<BoxCollider2D>().size.y;

        float cameraWidth = gameplayCameraObject.GetComponent<Camera>().orthographicSize * gameplayCameraObject.GetComponent<Camera>().aspect * 2f;
        float cameraHeight = gameplayCameraObject.GetComponent<Camera>().orthographicSize * 2f;

        float currentZoneTriggerX = targetTriggerGameObject.transform.position.x;
        float currentZoneTriggerY = targetTriggerGameObject.transform.position.y;

        // Boundaries
        float maxCameraX = currentZoneTriggerX + targetTriggerGameObject.GetComponent<BoxCollider2D>().size.x / 2;
        float minCameraX = currentZoneTriggerX - targetTriggerGameObject.GetComponent<BoxCollider2D>().size.x / 2;
        float maxCameraY = currentZoneTriggerY + targetTriggerGameObject.GetComponent<BoxCollider2D>().size.y / 2;
        float minCameraY = currentZoneTriggerY - targetTriggerGameObject.GetComponent<BoxCollider2D>().size.y / 2;

        Vector3 targetPosition = targetTriggerGameObject.transform.position;
        targetPosition.z = -10f; // Set the z position to -10f so that the camera is always at the same depth
        player.GetComponent<PlayerMovementBehaviour>().ToggleMovement();

        // get movement direction
        Vector3 movementDirection = (targetPosition - gameplayCameraObject.transform.position).normalized;
        
        if (Math.Abs(movementDirection.x) > Math.Abs(movementDirection.y))
        {
            // Movement is horizontal
            targetPosition.y = gameplayCameraObject.transform.position.y;
            if (cameraWidth < targetWidth) {
                if (movementDirection.x > 0)
                {
                    targetPosition.x = (float) Math.Ceiling(minCameraX + cameraWidth/2f) - 0.5f; // TODO: try changing to 0.5f if it stutters
                } else
                {
                    targetPosition.x = (float) Math.Floor(maxCameraX - cameraWidth/2f) + 0.5f;
                }
            }
        }
        else
        {
            // Movement is vertical
            targetPosition.x = gameplayCameraObject.transform.position.x;
            if (cameraHeight < targetHeight) {
                if (movementDirection.y > 0)
                {
                    targetPosition.y = (float) Math.Ceiling(minCameraY + cameraHeight/2f - 0.5f);
                } else
                {
                    targetPosition.y = (float) Math.Floor(maxCameraY - cameraHeight/2f) + 0.5f;
                }
            }
        }

        StartCoroutine(SlideToCoroutine(targetPosition, slideSpeed));
    }

    private IEnumerator SlideToCoroutine(Vector3 targetPosition, float slideSpeed)
    {
        Vector3 startPosition = gameplayCameraObject.transform.position;
        float t = 0f;

        while(t < 1f)
        {
            t += Time.deltaTime * slideSpeed;
            gameplayCameraObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
        player.GetComponent<PlayerMovementBehaviour>().ToggleMovement();
    }
}
