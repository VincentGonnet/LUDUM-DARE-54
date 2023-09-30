using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDetector : MonoBehaviour
{

    [SerializeField] PlayerProperties playerProperties;
    public int zoneNumber;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Player entered zone " + zoneNumber);
            playerProperties.currentZone = zoneNumber;
        }
    }
}
