using System;
using System.Collections;
using UnityEngine;

public class PlayerJumpBehaviour : MonoBehaviour
{
    //Stored Values
    private Vector3 jumpDirection;

    private float maxDistanceCheck = 5f;


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
            (float) Math.Ceiling(transform.position.x),
            (float) Math.Ceiling(transform.position.y),
            0
        );

        // Debug.Log("Pod Position: " + podPosition);

        // Check if the pod position is in the list of pressed pods
        bool podPressed = false;
        foreach (Vector3 pod in jumpPodsPressedPos)
        {
            Vector3 testPos = new Vector3(
                (float) Math.Ceiling(pod.x),
                (float) Math.Ceiling(pod.y),
                0
            );

            if (testPos == podPosition)
            {
                podPressed = true;
                break;
            }
        }

        // Debug.Log("Pod Pressed: " + podPressed);

        if (!podPressed) return;

        podPosition -= new Vector3(0.5f, 0.5f, 0);
        
        // Debug.Log("Pod Position: " + podPosition);
        // Debug.Log("Jump! " + jumpDirection);

        // From pod position, raycast in the jump direction to find the next pod
        Debug.DrawRay(podPosition, jumpDirection * maxDistanceCheck, Color.red, 1000f, false);
        RaycastHit2D[] pods = Physics2D.RaycastAll(podPosition, jumpDirection, maxDistanceCheck, LayerMask.GetMask("JumpPod"));

        // Debug.Log("Pods found: " + pods.Length);

        // If no pod found, return
        if (pods.Length == 0 || pods[0].collider == null)
        {
            // Debug.Log("No pod found");
            return;
        }

        // Debug.Log("Pod found: " + pods[0].collider.gameObject.name + " at " + pods[0].collider.gameObject.transform.position);

        // Find the closest pod center coordonates with a closestDistance (that is greater than 1f) and teleport to it

        float closestDistance = maxDistanceCheck;
        Vector3 closestPod = Vector3.zero;
        foreach (RaycastHit2D pod in pods)
        {
            Vector3 podCenter = pod.collider.gameObject.transform.position;
            float distance = Vector3.Distance(podCenter, podPosition);
            if (distance < closestDistance && distance > 1f)
            {
                closestDistance = distance;
                closestPod = podCenter;
            }
        }

        if(closestPod == Vector3.zero) return;

        // Calculate the jump position
        Vector3 jumpPosition = closestPod;

        GetComponent<PlayerController>().SetSpriteDirection(jumpDirection);
        transform.position = jumpPosition;
    }
}
