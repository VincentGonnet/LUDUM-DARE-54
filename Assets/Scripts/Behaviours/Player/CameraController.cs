using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool firstLoad = true;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (firstLoad)
        {
            firstLoad = false;
            return;
        }
        if(other.CompareTag("ZoneTrigger"))
        {
            gameObject.GetComponent<PlayerMovementBehaviour>().Slide1UnitToward(other.gameObject.transform.position, gameObject.GetComponent<PlayerController>().lastMovementDirection);
            CameraManager.Instance.SlideTo(other.gameObject, 2f);
        }
    }
}
