using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneStart : MonoBehaviour
{

    private bool hasPlayed = false;

    void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && !hasPlayed)
        {
            Debug.Log("Triggered");
            hasPlayed = true;
            this.GetComponent<PlayableDirector>().Play();
        }
    }
}