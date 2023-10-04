using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDetector : MonoBehaviour
{

    [SerializeField] PlayerProperties playerProperties;
    public int zoneNumber;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player entered " + gameObject.name + " ( " + zoneNumber + ")");
            playerProperties.currentZone = zoneNumber;
        }
    }
}
