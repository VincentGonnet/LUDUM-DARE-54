using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBehaviour : MonoBehaviour
{

    [Header("Component References")]
    public Rigidbody2D playerRigidbody;

    [Header("Attack Settings")]
    public float attackDistance = 3f;

    //Stored Values
    private Vector3 attackDirection;

    public Collider2D[] attackTargets;


    public void SetupBehaviour()
    {
        // Potentially setup any other components here
        // Is called when the player is setup in the GameManager
    }

    public void UpdateAttackData(Vector3 newAttackDirection)
    {
        attackDirection = newAttackDirection;
    }

    public void Attack()
    {
        Debug.Log("Attack launched");
        attackTargets = Physics2D.OverlapCircleAll(transform.position, attackDistance, LayerMask.GetMask("Enemies"));
        foreach (var target in attackTargets)
        {
            Debug.Log("Attack hit : " + target.name);
            target.GetComponent<EnemyController>().takeHit();
        }
            
        
    }
}
