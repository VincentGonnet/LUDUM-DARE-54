using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    public bool touchesBottom;
    public bool touchesTop;
    public bool touchesLeft;
    public bool touchesRight;

    void FixedUpdate() {
        GameObject currentZoneTrigger = player.GetComponent<PlayerController>().GetCurrentZoneTrigger();
        if (currentZoneTrigger != null) {
            float currentZoneTriggerX = currentZoneTrigger.transform.position.x;
            float currentZoneTriggerY = currentZoneTrigger.transform.position.y;

            float maxCameraX = currentZoneTriggerX + currentZoneTrigger.GetComponent<BoxCollider2D>().size.x / 2;
            float minCameraX = currentZoneTriggerX - currentZoneTrigger.GetComponent<BoxCollider2D>().size.x / 2;
            float maxCameraY = currentZoneTriggerY + currentZoneTrigger.GetComponent<BoxCollider2D>().size.y / 2;
            float minCameraY = currentZoneTriggerY - currentZoneTrigger.GetComponent<BoxCollider2D>().size.y / 2;

            float playerX = player.transform.position.x;
            float playerY = player.transform.position.y;

            float screenHeigth = Camera.main.orthographicSize;
            float screenWidth = screenHeigth * Camera.main.aspect;

            // Debug.Log("Screen Height: " + screenHeigth);
            // Debug.Log("Screen Width: " + screenWidth);

            // Get the bounds of the camera's view
            float upperSeen = transform.position.y + screenHeigth;
            float lowerSeen = transform.position.y - screenHeigth;
            float rightSeen = transform.position.x + screenWidth;
            float leftSeen = transform.position.x - screenWidth;

            // Move the camera to the player's position smoothly
            float cameraX = Mathf.Lerp(transform.position.x, playerX, 0.1f);
            float cameraY = Mathf.Lerp(transform.position.y, playerY, 0.1f);

            touchesRight = rightSeen >= maxCameraX;
            touchesLeft = leftSeen <= minCameraX;
            touchesTop = upperSeen >= maxCameraY;
            touchesBottom = lowerSeen <= minCameraY;

            Vector3 newCameraPosition = transform.position;

            if (touchesLeft && touchesRight) {
                newCameraPosition.x = transform.position.x;
            } else if (touchesLeft) {
                newCameraPosition.x = Math.Max(playerX , minCameraX + Camera.main.orthographicSize * Camera.main.aspect);
            } else if (touchesRight) {
                newCameraPosition.x = Math.Min(playerX, maxCameraX - Camera.main.orthographicSize * Camera.main.aspect);
            } else if (!touchesLeft && !touchesRight) {
                newCameraPosition.x = playerX;
            }

            if (touchesTop && touchesBottom) {
                newCameraPosition.y = transform.position.y;
            } else if (touchesTop) {
                newCameraPosition.y = Math.Min(playerY, maxCameraY - Camera.main.orthographicSize);
            } else if (touchesBottom) {
                newCameraPosition.y = Math.Max(playerY, minCameraY + Camera.main.orthographicSize);
            } else if (!touchesTop && !touchesBottom) {
                newCameraPosition.y = playerY;
            }

            transform.position = newCameraPosition;
        }
    }

    public void ZoomIn(bool zoomIn) {

        StartCoroutine(ZoomInCoroutine(zoomIn));

    }

    IEnumerator ZoomInCoroutine(bool zoomIn) {

        float duration = 0.5f;
        float t = 0;
        while (t < duration) {
            t += Time.deltaTime;
            if (zoomIn) {
                Camera.main.orthographicSize = Mathf.SmoothStep(2f, 5f, t / duration);
            } else {
                Camera.main.orthographicSize = Mathf.SmoothStep(5f, 2f, t / duration);
            }
            yield return null;
        }

    }
}
