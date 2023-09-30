using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashBehaviour : MonoBehaviour
{

    [Header("Component References")]
    public Rigidbody2D playerRigidbody;

    [Header("Dash Settings")]
    public float dashForce = 10f;

    //Stored Values
    private Vector3 dashDirection;
    public bool isDashing = false;
    private Vector3 startPosition;
    public int holeCounters = 0;

    public void SetupBehaviour()
    {
        // Potentially setup any other components here
        // Is called when the player is setup in the GameManager
    }

    public void UpdateDashData(Vector3 newDashDirection)
    {
        dashDirection = newDashDirection;
    }

    public void Dash()
    {
        playerRigidbody.gameObject.GetComponent<PlayerMovementBehaviour>().ToggleMovement();
        Vector3 dash = dashDirection * dashForce;
    
        isDashing = true;
        startPosition = playerRigidbody.transform.position;
        playerRigidbody.AddForce(dash, ForceMode2D.Impulse);

        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        
        yield return new WaitForSeconds(0.2f);
        playerRigidbody.velocity = Vector3.zero;
        isDashing = false;
        StartCoroutine(CheckHole());

        playerRigidbody.gameObject.GetComponent<PlayerMovementBehaviour>().ToggleMovement();
    }

    private IEnumerator CheckHole()
    {
        yield return new WaitForSeconds(0.2f);
        if (holeCounters > 0)
        {
            Debug.Log("In hole");
            playerRigidbody.transform.position = startPosition;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hole"))
        {
            holeCounters++;
        }
        if (collision.CompareTag("Hole") && isDashing)
        {
            StartCoroutine(PassThroughHole(collision));
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Hole") && isDashing)
        {
            StartCoroutine(PassThroughHole(collision));
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hole"))
        {
            holeCounters--;
        }
    }

    private IEnumerator PassThroughHole(Collider2D collision)
    {
        GameObject hole = collision.gameObject;
        Collider2D[] colliders = hole.GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
        yield return new WaitForSeconds(0.17f);
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = true;
        }
        yield return new WaitForSeconds(0.17f);
    }
}
