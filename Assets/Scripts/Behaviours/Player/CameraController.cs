using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("ZoneTrigger"))
        {
            gameObject.GetComponent<PlayerMovementBehaviour>().Slide1UnitToward(other.gameObject.transform.position);
            CameraManager.Instance.SlideTo(other.gameObject.transform.position, 2f);
        }
    }
}
