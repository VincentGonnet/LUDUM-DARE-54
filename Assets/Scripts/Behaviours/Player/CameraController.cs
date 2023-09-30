using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("ZoneTrigger"))
        {
            gameObject.GetComponent<PlayerMovementBehaviour>().Slide1UnitToward(other.gameObject.transform.position);
            CameraManager.Instance.SlideTo(other.gameObject.transform.position, 2f);
        }
    }
}
