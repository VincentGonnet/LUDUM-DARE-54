using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPod : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Debug.Log("Player entered pod : " + gameObject.transform.position);
            other.GetComponent<PlayerController>().AddPodPressed(gameObject.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            other.GetComponent<PlayerController>().RemovePodPressed(gameObject.transform.position);
        }
    }
}
