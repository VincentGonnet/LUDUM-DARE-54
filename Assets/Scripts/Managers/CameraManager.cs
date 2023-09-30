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

    public void SlideTo(Vector3 targetPosition, float slideSpeed)
    {
        targetPosition.z = -10f; // Set the z position to -10f so that the camera is always at the same depth
        player.GetComponent<PlayerMovementBehaviour>().ToggleMovement();
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
